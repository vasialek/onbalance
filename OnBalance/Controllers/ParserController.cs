using OnBalance.Core;
using OnBalance.Domain.Concrete;
using OnBalance.Parsers;
using OnBalance.Parsers.Parsers;
using System;
using System.Collections.Generic;
using System.Linq;
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
            string[] lines = data.Split("\n".ToCharArray());
            IBalanceParser p = new GjExcelParser();
            var items = p.ParseFileContent(lines);

            if (items.Count > 0)
            {
                items = items.OrderBy(x => x.CategoryName).ToList();
                TempData["ExchangeItems"] = items;
            }

            return View("Preview", "_LayoutLogin", items);
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
                    product.StatusId = (byte)OnBalance.Status.Pending;
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
                        pd.StatusId = (byte)OnBalance.Status.Pending;
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
                Logger.Error("Error importing parsed products", ex);
                return Content(ex.Message);
            }
        }
    }
}
