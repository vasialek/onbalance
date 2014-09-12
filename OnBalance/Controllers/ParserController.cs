using OnBalance.Core;
using OnBalance.Domain.Concrete;
using OnBalance.Models;
using OnBalance.Parsers;
using OnBalance.Parsers.Parsers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace OnBalance.Controllers
{

    public class ParserController : BaseController
    {
        //
        // GET: /parser/

        public ActionResult Index()
        {
            return Edit("gj");
        }

        //
        // GET: /parser/edit

        public ActionResult Edit(string id)
        {
            return View("Edit", "_LayoutLogin");
        }

        //
        // GET: /parser/preview

        public ActionResult Preview(string id)
        {
            string data = Request["Data"];
            string productType = Request["ProductType"].Trim().ToLower();
            string[] lines = data.Split("\n".ToCharArray());
            IBalanceParser p = productType == "gjdress" ? new GjExcelParserDress() : new GjExcelParserShoes();
            var items = p.ParseFileContent(lines);

            // Pass statistics
            ViewBag.ProcessedNonEmptyLines = p.TotalProcessedNonEmptyLines;
            ViewBag.CategoryLines = p.TotalCategoryLines;

            if (p.Errors.Count > 0)
            {
                TempData["ParserErrors"] = p.Errors.ToList();
                return RedirectToAction("errors", new { id = id });
            }

            // Just to beautify grid
            ViewBag.SizeNames = ExtractAvailableSizes(items);

            ViewBag.CategoryNames = items.Select(x => x.CategoryName)
                .Distinct()
                .ToList();
            //PrepareInsertSql(items, 102000);

            if (items.Count > 0)
            {
                items = items.OrderBy(x => x.CategoryName).ToList();
                TempData["ExchangeItems"] = items;

                //StringBuilder sb = new StringBuilder();
                //foreach (var e in p.Errors)
                //{
                //    sb.AppendFormat("#{0}\t{1}", e.LineNr, e.Line);
                //    sb.AppendLine("<br />");
                //    Response.Write(sb.ToString());
                //    Response.End();
                //}
            }

            return View("Preview", "_LayoutLogin", items);
        }

        //
        // GET: /parser/errors/500000

        public ActionResult Errors(string id)
        {
            IList<Parsers.BalanceParseError> errors = null;

            errors = TempData["ParserErrors"] == null ? null : (List<Parsers.BalanceParseError>)TempData["ParserErrors"];

            return View("Errors", "_LayoutLogin", errors);
        }

        //
        // POST: /parser/import

        public ActionResult Import()
        {
            try
            {
                var parsed = TempData["ExchangeItems"] as List<ParsedItem>;
                if (parsed == null)
                {
                    throw new ArgumentNullException("Information about sizes", "No parsed items. Bad data?!");
                }

                var db = new EfProductRepository();
                foreach (var item in parsed)
                {
                    var product = new OnBalance.Domain.Entities.Product();
                    //product.CategoryId = 
                    product.InternalCode = item.InternalCode;
                    product.Name = item.ProductName;
                    product.PosId = 500000;
                    product.Price = item.PriceOfRelease;
                    product.StatusId = (byte)Status.Pending;
                    product.UserId = User.Identity.Name;
                    product.Uid = Common.EncodeHex(Common.CalculateMd5(string.Format("{0}-{1}-{2}", User.Identity.Name, product.PosId, product.InternalCode)));
                    product.CreatedAt = DateTime.UtcNow;
                    db.Save(product);

                    foreach (var size in item.Sizes)
                    {
                        var pd = new OnBalance.Domain.Entities.ProductDetail();
                        pd.ProductId = product.Id;
                        pd.ParameterName = "size";
                        pd.ParameterValue = size.Size;
                        pd.Quantity = size.Quantity;
                        pd.StatusId = (byte)Status.Pending;
                        pd.PriceMinor = (int)(item.Price * 100);
                        pd.PriceReleaseMinor = (int)(item.PriceOfRelease * 100);
                        pd.CreatedAt = DateTime.UtcNow;
                        pd.UpdatedAt = DateTime.UtcNow;
                        db.Save(pd);
                    }

                    //var bi = new OnBalance.Domain.Entities.BalanceItem();
                    //bi.InternalCode = item.InternalCode;
                    //bi.PosId = 500001;
                    //bi.Price = item.Price;
                    //bi.PriceOfRelease = item.PriceOfRelease;
                    //bi.ProductName = item.ProductName;
                    //bi.Quantity = item.Quantity;
                    //bi.StatusId = (byte)OnBalance.Status.Pending;
                    //db.Save(bi);
                }
                db.SubmitChanges();

                return View("Import", "_LayoutLogin");
            }
            catch (Exception ex)
            {
                Error("Error importing parsed products", ex);
                return Content(ex.Message);
            }
        }

        protected IList<ItemSizeQuantity> ExtractAvailableSizes(IList<ParsedItem> parsed)
        {
            List<ItemSizeQuantity> sizes = new List<ItemSizeQuantity>();

            foreach (var pi in parsed)
            {
                for (int i = 0; i < pi.Sizes.Count; i++)
                {
                    // Add size name if it is not present
                    if (sizes.FirstOrDefault(x => x.Size == pi.Sizes[i].Size) == null)
                    {
                        sizes.Add(pi.Sizes[i]);
                    }
                }
            }

            return sizes.OrderBy(x => x.Size).ToList();
        }

        protected string PrepareInsertSql(IList<ParsedItem> parsed, int startId)
        {
            StringBuilder sb = new StringBuilder();
            

            string sqlFmt = "INSERT INTO [vasialek_onbalance].[vasialek_onbalance_user].[product] ([status_id], [pos_id], [internal_code], [uid], [user_id], [name], [price], [created_at], [category_id]) VALUES (%status_id%, %pos_id%, '%internal_code%', '%uid%', '%user_id%', '%name%', %price%, '%created_at%', %category_id%)";
            string sqlDetFmt = "INSERT INTO [vasialek_onbalance].[vasialek_onbalance_user].[product_detail] ([status_id], [product_id], [parameter_name], [parameter_value], [price_minor], [price_release_minor], [quantity], [updated_at], [created_at]) VALUES (%status_id%, @id, '%parameter_name%', '%parameter_value%', %price_minor%, %price_release_minor%, %quantity%, '%updated_at%', '%created_at%')";

            Category c;
            var categories = GetCategories();
            string userId = "gj";
            int posId = 500000;

            sb.AppendLine("declare @id int;");

            foreach (var pi in parsed/*.Take(10)*/)
            {
                if (string.IsNullOrEmpty(pi.InternalCode) == false)
                {
                    c = categories.FirstOrDefault(x => x.Name.Equals(pi.CategoryName.Trim()));
                    if (c == null)
                    {
                        throw new KeyNotFoundException("Could not locate category: " + pi.CategoryName);
                    }
                    string internalCode = FormatInternalCode(pi.InternalCode, "GJ_ES_{0}");
                    sb.AppendLine("--------------------------------------------------");
                    sb.AppendLine(sqlFmt
                        .Replace("%id%", startId.ToString())
                        .Replace("%status_id%", "1")
                        .Replace("%pos_id%", posId.ToString())
                        .Replace("%internal_code%", internalCode)
                        .Replace("%uid%", Common.EncodeHex(Common.CalculateMd5(string.Format("{0}-{1}-{2}", userId, posId, internalCode))))
                        .Replace("%user_id%", userId)
                        .Replace("%name%", pi.ProductName)
                        .Replace("%price%", pi.PriceOfRelease.ToString("######0.00", System.Globalization.CultureInfo.InvariantCulture.NumberFormat))
                        .Replace("%created_at%", DateTime.UtcNow.ToString())
                        .Replace("%category_id%", c.Id.ToString())
                        );
                    sb.AppendLine();
                    sb.AppendLine("-- Get inserted ID");
                    sb.AppendLine("set @id = scope_identity();");

                    sb.AppendLine("-- Sizes:");
                    foreach (var s in pi.Sizes)
                    {
                        sb.AppendLine(sqlDetFmt
                            .Replace("%status_id%", "1")
                            //.Replace("%product_id%", startId.ToString())
                            .Replace("%parameter_name%", "size")
                            .Replace("%parameter_value%", s.Size)
                            .Replace("%price_minor%", (pi.Price * 100).ToString("######0.00", System.Globalization.CultureInfo.InvariantCulture.NumberFormat))
                            .Replace("%price_release_minor%", (pi.PriceOfRelease * 100).ToString("######0.00", System.Globalization.CultureInfo.InvariantCulture.NumberFormat))
                            .Replace("%quantity%", s.Quantity.ToString())
                            .Replace("%updated_at%", DateTime.UtcNow.ToString())
                            .Replace("%created_at%", DateTime.UtcNow.ToString())
                            );
                        //sb.AppendLine("GO");
                    }
                    sb.AppendLine();

                    startId++; 
                }
            }
            Response.Write(sb.ToString().Replace(Environment.NewLine, "<br />"));
            Response.End();
            //Info(sb.ToString());
            return sb.ToString();
        }

        protected string FormatInternalCode(string code, string fmt)
        {
            return string.Format(fmt, code.Trim().Replace(" ", "_").Replace("-", "_")).ToUpper();
        }

        protected IList<Category> GetCategories()
        {
            var categories = new List<Category>();

            categories.Add(new Category { Id = 1016, Name = "ADIDAS vyr.laisvalaikio" });
            categories.Add(new Category { Id = 1017, Name = "BUCAI" });
            categories.Add(new Category { Id = 1018, Name = "Krepšinio kedai" });
            categories.Add(new Category { Id = 1019, Name = "NIKE" });
            categories.Add(new Category { Id = 1020, Name = "Mot.sp.bateliai" });
            categories.Add(new Category { Id = 1021, Name = "Laisvalaikio vyr." });
            categories.Add(new Category { Id = 1022, Name = "vaikiski ADIDAS" });
            categories.Add(new Category { Id = 1023, Name = "REEBOK vaikiski" });
            categories.Add(new Category { Id = 1024, Name = "\"slepetes\"\"ADIDAS\"\"\"" });
            categories.Add(new Category { Id = 1025, Name = "Slepetes W ADIDAS" });
            categories.Add(new Category { Id = 1026, Name = "\"vyr.slepetes\"\"NIKE\"\"\"" });
            categories.Add(new Category { Id = 1027, Name = "W slepetes NIKE" });
            categories.Add(new Category { Id = 1028, Name = "BASUTES ADIDAS" });
            categories.Add(new Category { Id = 1029, Name = "FUTBOLO APSAUGOS" });
            categories.Add(new Category { Id = 1030, Name = "PUMA" });
            categories.Add(new Category { Id = 1031, Name = "NIKE paugliniai,moteriski" });
            categories.Add(new Category { Id = 1032, Name = "Mot.sp.kelnes" });
            categories.Add(new Category { Id = 1033, Name = "Dzemp.vyr." });
            categories.Add(new Category { Id = 1034, Name = "SORTAI NIKE" });
            categories.Add(new Category { Id = 1035, Name = "SORTAI AD" });
            //categories.Add(new Category { Id = 1036, Name = "Vyriškos sp.kelnes NK" });
            categories.Add(new Category { Id = 1036, Name = "Vyriškos sp.kelnės NK" });
            categories.Add(new Category { Id = 1037, Name = "ADIDAS" });
            categories.Add(new Category { Id = 1038, Name = "VYR.MAIKES AD" });
            categories.Add(new Category { Id = 1039, Name = "NIKE MAIK" });
            categories.Add(new Category { Id = 1040, Name = "Sp.mot.kostiumai AD" });
            categories.Add(new Category { Id = 1041, Name = "VYR.STRIUKĖS" });
            categories.Add(new Category { Id = 1042, Name = "STR,VEJASTR.MOT" });
            categories.Add(new Category { Id = 1043, Name = "VYR.KOST.AD" });
            categories.Add(new Category { Id = 1044, Name = "Vyr.sp.kostiumai NK" });
            categories.Add(new Category { Id = 1045, Name = "Vyr.kreps.maikes" });
            categories.Add(new Category { Id = 1046, Name = "Vyr.maud.kelnaites" });
            categories.Add(new Category { Id = 1047, Name = "Sort.capri.moteriski" });
            categories.Add(new Category { Id = 1048, Name = "VYR.SORTAI ADIDAS" });
            categories.Add(new Category { Id = 1049, Name = "TASES/KUPR" });
            categories.Add(new Category { Id = 1050, Name = "KEPURES NIKE" });
            categories.Add(new Category { Id = 1051, Name = "DZ.NIKE" });
            categories.Add(new Category { Id = 1052, Name = "KELNES NIKE" });
            categories.Add(new Category { Id = 1053, Name = "DZ ADIDAS" });
            categories.Add(new Category { Id = 1054, Name = "FUT.APSAUGOS" });
            categories.Add(new Category { Id = 1055, Name = "MOT.SP.KOST." });
            categories.Add(new Category { Id = 1056, Name = "KOJINES" });
            categories.Add(new Category { Id = 1057, Name = "KEPURES REE" });
            categories.Add(new Category { Id = 1058, Name = "TASES/KUPR." });
            categories.Add(new Category { Id = 1059, Name = "PIRSTINES" });
            categories.Add(new Category { Id = 1060, Name = "KEPURES AD" });
            categories.Add(new Category { Id = 1061, Name = "KELNES ADIDAS" });
            categories.Add(new Category { Id = 1062, Name = "DZEMP.W" });
            categories.Add(new Category { Id = 1063, Name = "STR.AD.VYR" });
            categories.Add(new Category { Id = 1064, Name = "STR.NIKE.VYR." });
            categories.Add(new Category { Id = 1065, Name = "DŽ. PUMA" });
            categories.Add(new Category { Id = 1066, Name = "MAIK.PUMA" });
            categories.Add(new Category { Id = 1067, Name = "MAIK.NIKE" });
            categories.Add(new Category { Id = 1068, Name = "MAIK.AD" });
            categories.Add(new Category { Id = 1069, Name = "SORTAI PUMA" });
            categories.Add(new Category { Id = 1070, Name = "slepetes\"ADIDAS\"" });
            categories.Add(new Category { Id = 1071, Name = "vyr.slepetes\"NIKE\"" });
            categories.Add(new Category { Id = 1072, Name = "KAMUOLIAI" });
            categories.Add(new Category { Id = 1073, Name = "krepsinio aprangos" });

            return categories;
        }
    }
}
