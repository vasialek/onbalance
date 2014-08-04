using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using OnBalance.ViewModels;
using OnBalance.Models;
using System.Text;
using System.Globalization;
using System.Text.RegularExpressions;
using System.Net;
using System.Collections.Specialized;
using OnBalance.ViewModels.Products;
using OnBalance.ViewModels.Categories;
//using OnBalance.Domain.Entities;
using OnBalance.Domain.Abstract;
using OnBalance.Core;
using OnBalance.Domain.Concrete;

namespace OnBalance.Controllers
{

    public class PradminController : BaseController
    {
        private IOrganizationRepository _organizationRepository = null;
        private IProductRepository _productRepository = null;
        private IBalanceItemRepository _balanceRepository = null;

        //public PradminController()
        //    : this(new OnBalance.Domain.Concrete.EfProductRepository(), new EfOrganizationRepository())
        //{

        //}

        public PradminController(IProductRepository productRepository, IOrganizationRepository organisationRepository, IBalanceItemRepository balanceRepository)
        {
            if(productRepository == null)
            {
                throw new ArgumentNullException("productRepository");
            }
            if(organisationRepository == null)
            {
                throw new ArgumentNullException("organisationRepository");
            }
            if (balanceRepository == null)
            {
                throw new ArgumentNullException("balanceRepository");
            }

            _productRepository = productRepository;
            _organizationRepository = organisationRepository;
            _balanceRepository = balanceRepository;
        }

        //
        // GET: /pradmin/

        [Authorize]
        public ActionResult Index()
        {
            return List(_organizationRepository.Organizations.First().Id);
        }

        //
        // GET: /pradmin/balance/500000

        [OutputCache(Duration = 120)]
        public ActionResult Balance(int id)
        {
            try
            {
                var pos = _organizationRepository.GetById(id);
                var productsByCategories = GetProductsByCategories(pos.Id);
                productsByCategories.PosId = id;
                //var products = _productRepository.GetLastInPos(pos.Id, 0, 20);
                //return View("Balance", new ViewModels.Products.ProductsByCategoryViewModel(products.ToList()));
                return View("Balance", productsByCategories);
            }
            catch (Exception ex)
            {
                Error("Error showing balance for POS #" + id, ex);
                throw;
            }
        }

        //
        // GET: /pradmin/list/100003

        [Authorize]
        public ActionResult List(int id)
        {
            ProductsInPosViewModel productsList = new ProductsInPosViewModel();
            productsList.Pos = _organizationRepository.Organizations.SingleOrDefault(x => x.Id == id);
            if( productsList.Pos == null )
            {
                ErrorFormat("Trying to list products in non-existing POS #{0}!", id);
                return HttpNotFound();
            }

            //productsList.Categories = _productRepository.Categories.Select(x => new CategoryListItem(null)).ToList();

            //productsList.Categories = _productRepository.Categories
            //    .Where(x => x.OrganizationId == productsList.Pos.Id)
            //    .Select(x => new CategoryListItem(x))
            //    //.Select(x => new OnBalance.Models.Category {
            //    //    Id = x.Id,
            //    //    Name = x.Name,
            //    //    OrganizationId = x.OrganizationId,
            //    //    ParentId = x.ParentId,
            //    //    StatusId = x.StatusId,
            //    //    CategoryTypeId = x.CategoryTypeId,
            //    //})
            //    .ToList();

            int perPage = 50;
            int page = 0;
            if( int.TryParse(Request["p"], out page) == false )
            {
                page = 1;
            }
            int offset = (page - 1) * perPage;

            InfoFormat("Displaying list of products in POS #{0}, skipping {1}, taking {2} products", id, offset, perPage);
            productsList.Products = _productRepository.GetLastInPos(id, offset, perPage)
                .OrderBy(x => x.Id)
                .Select(x => new OnBalance.Models.Product
                {
                    Id = x.Id,
                    Name = x.Name,
                    PosId = x.PosId,
                    Price = x.Price,
                    StatusId = x.StatusId,
                    InternalCode = x.InternalCode,
                    CategoryId = x.CategoryId,
                    Uid = x.Uid,
                    UserId = x.UserId,
                    Pos = new OnBalance.Domain.Entities.Organization { Configuration = new OnBalance.Domain.Entities.OrganizationConfiguration() }
                })
                .ToList();
            DebugFormat("  got {0} products...", productsList.Products.Count);

            return View("List", Layout, productsList);
        }

        //
        // GET: /pradmin/categories/500001

        [Authorize]
        public ActionResult Categories(int id)
        {
            var model = new PosCategoriesListViewModel();
            var db = new ProductRepository();
            throw new NotImplementedException("Need to Ninject CategoryRepository!");
            //model.Organization = new OrganizationRepository().Items.Single(x => x.Id == id);
            //model.Categories = db.Categories.Where(x => x.OrganizationId == model.Organization.Id).ToList();
            return View(model);
        }

        //
        // GET: /pradmin/allcategories

        [Authorize(Roles = "Administrator")]
        public ActionResult AllCategories()
        {
            var db = new ProductRepository();
            return View(db.Categories);
        }

        //
        // GET: /pradmin/editcategory/1001

        [Authorize]
        public ActionResult EditCategory(int id)
        {
            CategoryStructureViewModel vm = new CategoryStructureViewModel();
            //var model = new Category();
            var db = new ProductRepository();
            throw new NotImplementedException("vm.Category = db.GetCategory(id)");
            //vm.Category = db.GetCategory(id);
            if( vm.Category == null )
            {
                ErrorFormat("User #{0} tries to edit non-existing category #{1}!", User.Identity.Name, id);
                return RedirectToAction("notfound", "help");
            }

            //model.Organization = new OrganizationRepository().Items.Single(x => x.Id == model.Category.id);
            return View(vm);
        }

        //
        // POST: /pradmin/editcategory/1003
        [Authorize]
        [HttpPost]
        public ActionResult EditCategory(int id, FormCollection f)
        {
            OnBalance.Domain.Entities.Category model = null;
            try
            {
                var db = new ProductRepository();
                model = _productRepository.GetCategory(id);
                if(model == null)
                {
                    ErrorFormat("User #{0} tries to update non-existing category #{1}!", User.Identity.Name, id);
                    return RedirectToAction("notfound", "help");
                }

                UpdateModel(model);
                db.SubmitChanges();
                return RedirectToAction("editcategory", new { id = id });
            } catch(Exception ex)
            {
                Error(string.Concat("Error updating category #", id), ex);
                ModelState.AddModelError("", ex.Message);
            }

            return View(model);
        }

        //
        // POST: /pradmin/dosavestructure

        //[HttpPost]
        public ActionResult DoSaveStructure(int id, CategoryStructureViewModel vm /*FormCollection f, List<CategoryStructure> CategoryStructure*/)
        {
            try
            {
                var db = new OnBalance.Domain.Entities.CategoryStructureRepository();
                var dbProduct = new ProductRepository();
                //Category model = dbProduct.GetCategory(id);
                string newName = Request["NewName"];
                bool isNewApproved = false;

                //InfoFormat("User #{0} updates Category #{1} to name: {2}, status: {3} ({4}), organization: #{5}", User.Identity.Name, vm.Category.Id, vm.Category.Name, vm.Category.StatusName, vm.Category.StatusId, vm.Category.OrganizationId);
                //InfoFormat("User #{0} updates Category #{1} to name: {2}, status: {3} ({4}), organization: #{5}", User.Identity.Name, model.Id, model.Name, model.StatusName, model.StatusId, model.OrganizationId);
                throw new NotImplementedException("dbProduct.Save(vm.Category)");
                //dbProduct.Save(vm.Category);
                //UpdateModel<Category>(model, "Category__");
                //dbProduct.SubmitChanges();

                //if(CategoryStructure != null)
                //{
                    Info("Updating category structure...");
                    foreach(var item in vm.CategoryStructure)
                    {
                        InfoFormat("  Category structure: #{0, -8}. {1}", item.Id, item.FieldName);
                        throw new NotImplementedException("db.Update(item)");
                        //db.Update(item);
                    }
                //}

                if(string.IsNullOrEmpty(newName) == false)
                {
                    bool.TryParse(Request["NewStatus"], out isNewApproved);
                    OnBalance.Domain.Entities.CategoryStructure cs = new OnBalance.Domain.Entities.CategoryStructure();
                    cs.FieldName = newName;
                    cs.StatusId = isNewApproved ? (byte)Status.Approved : (byte)Status.Deleted;
                    cs.CategoryId = vm.Category.Id;
                    db.Add(cs);
                }

                db.SubmitChanges();

                return PartialView("CategoryStructure", vm.Category);
            } catch(Exception ex)
            {
                Error("Error updating Category structure!", ex);
                throw ex;
            }
        }
        
        //
        // GET: /pradmin/get?posid=100001

        public ActionResult Get()
        {
            // ID of POS must be!
            int posId = int.Parse(Request["posid"]);
            InfoFormat("Loading products for POS ID #{0}...", posId);

            ProductRepository db = new ProductRepository();

            StringBuilder sbMain = new StringBuilder();
            StringBuilder sb = new StringBuilder();
            var products = db.Items
                .Where(x => x.PosId == posId && x.StatusId == (byte)Status.Approved)
                .Take(200)
                .ToList();
            InfoFormat("Got {0} products for POS ID #{1}", products == null ? "NULL" : products.Count.ToString(), posId);
            //var shops = dbPos.Items.ToList();
            string callback = Request["callback"];

            // Returns JSON array of all products for specified POS

            foreach(var p in products)
            {

                sb.Clear();
                //foreach(var kvp in p.GetQuantityForAllSizes())
                //{
                //    sb.AppendFormat(", '{0}': {1}", kvp.Key, kvp.Value);
                //}

                sbMain.AppendFormat("{{ name: \"{0}\", code: \"{1}\", price_minor: '{2}', amount: {3} {4} }},", p.Name, p.InternalCode, p.PosId, 0, sb.ToString());
            }

            return Content(string.Format("{0}({{'data': [ {1} ]}})", callback, sbMain.ToString()));
        }

        //
        // GET: /pradmin/testsync

        public ActionResult TestSync()
        {
            List<BalanceItem> updates = new BalanceItemRepository().GetLastUpdated().ToList();
            NameValueCollection nvc = new NameValueCollection();

            nvc.Add("_token", "12345");

            for(int i = 0; (updates != null) && (i < updates.Count); i++)
            {
                nvc.Add(string.Format("results[{0}][pr]", i), updates[i].Price.ToString());
                nvc.Add(string.Format("results[{0}][qnt]", i), updates[i].Quantity.ToString());
            }

            WebClient wc = new WebClient();
            byte[] ba = wc.UploadValues("http://gjsportland.com/index.php/lt/product/dobalance", nvc);

            return Content("Finished: " + Encoding.UTF8.GetString(ba));
        }

        //
        // GET: /pradmin/getbalance/100001

        public ActionResult GetBalance(int id)
        {
            return Json(new
            {
                ResultSet = new
                {
                    totalResultsAvailable = 0
                    ,
                    totalResultsReturned = 0
                    ,
                    firstResultPosition = 0
                    ,
                    Result = new object[]{ }
                }
            }, JsonRequestBehavior.AllowGet);
        }

        //
        // GET: /pradmin/parse/100002

        [Authorize]
        public ActionResult Parse(int id)
        {
            return View();
        }

        // POST: /pradmin/parse

        [Authorize]
        [HttpPost]
        public ActionResult Parse(TextParserViewModel model)
        {
            List<BalanceItem> parsed = new List<BalanceItem>();
            //BalanceItem exi = null;

            //try
            //{
            //    StringBuilder sb = new StringBuilder();
            //    string[] lines = model == null ? null : model.Text.Split(Environment.NewLine.ToCharArray());
            //    int temp = 0;
            //    decimal tempPrice = 0m;
            //    string codeAndName = "";
            //    int pos = 0;

            //    for(int i = 0; (lines != null) && (i < lines.Length); i++)
            //    {
            //        string line = lines[i].Trim();
            //        string lineNr = (i + i).ToString();
            //        if(line.Length < 5)
            //        {
            //            //ModelState.AddModelError("Text", MyMessages.Parser.LineIsTooShortFmt.Replace("%NR%", lineNr).Replace("%LENGTH%", line.Length.ToString()));
            //        } else
            //        {
            //            string[] ar = line.Split(new char[] { '\t' });
            //            if(ar.Length < 4)
            //            {
            //                ModelState.AddModelError("Text", MyMessages.Parser.SplittedLineHasTooLessArgumentsFmt.Replace("%NR%", lineNr));
            //            } else
            //            {
            //                exi = new BalanceItem();
            //                codeAndName = ar[0].Trim();
            //                exi.Quantity = int.TryParse(ar[2].Trim(), out temp) ? temp : -1;
            //                exi.Price = decimal.TryParse(ar[3].Trim(), System.Globalization.NumberStyles.Currency, CultureInfo.InvariantCulture, out tempPrice) ? tempPrice : -1;
            //                exi.PriceOfRelease = decimal.TryParse(ar[ar.Length - 1].Trim(), NumberStyles.Currency, CultureInfo.InvariantCulture, out tempPrice) ? tempPrice : -1;

            //                // Search for N-digit code
            //                pos = codeAndName.LastIndexOf(' ');
            //                if(pos >= 0)
            //                {
            //                    exi.InternalCode = "---";
            //                    if(int.TryParse(codeAndName.Substring(pos), out temp))
            //                    {
            //                        exi.InternalCode = temp.ToString();
            //                        exi.ProductName = codeAndName.Substring(0, codeAndName.Length - temp.ToString().Length);
            //                    }
            //                    sb.AppendFormat("Code: {0}, name: {1}, qnt: {2}, price: {3}, price of release: {4}", exi.InternalCode, exi.ProductName, exi.Quantity, exi.Price, exi.PriceOfRelease);

            //                    if( !string.IsNullOrEmpty(exi.ProductName) && !string.IsNullOrEmpty(exi.InternalCode) )
            //                    {
            //                        parsed.Add(exi);
            //                    }
            //                } else
            //                {
            //                    ModelState.AddModelError("Text", MyMessages.Parser.CouldNotParseCodeForLineFmt.Replace("%NR%", lineNr));
            //                }

            //            }
            //        }
            //    }

            //    if( parsed.Count > 0 )
            //    {
            //        TempData["ExchangeItems"] = parsed;
            //        if(Request.Params.AllKeys.Contains("ExportBtn"))
            //        {
            //            return RedirectToAction("export", new { id = 100001 });
            //        }
            //        return RedirectToAction("balance", new { id = 100001 });
            //    }

            //    return Content(sb.ToString());

            //} catch(Exception ex)
            //{
            //    return Content("Error: " + ex.ToString());
            //}

            return View(model);
        }

        //
        // GET: /pradmin/export/100001

        [Authorize]
        public ActionResult Export(int id)
        {
            if( TempData["ExchangeItems"] == null )
            {
                TempData["Message"] = MyMessages.Errors.DataIsEmpty;
                return RedirectToAction("Parse");
            }

            ViewBag.ExchangeItems = TempData["ExchangeItems"];
            return View();
        }

        //
        // GET: /pradmin/confirm

        [Authorize]
        public ActionResult Confirm()
        {
            if(TempData["ExchangeItems"] == null)
            {
                TempData["Message"] = MyMessages.Errors.DataIsEmpty;
                return RedirectToAction("Parse");
            }

            ViewBag.ExchangeItems = TempData["ExchangeItems"];
            return View();
        }

        [Authorize]
        public ActionResult Create(int id)
        {
            InfoFormat("Creating product in POS with ID #{0}", id);
            var pos = _organizationRepository.GetById(id);
            if( pos == null )
            {
                return RedirectToAction("notfound", "help");
            }

            OnBalance.Domain.Entities.Product model = new OnBalance.Domain.Entities.Product();
            model.PosId = pos.Id;
            model.CategoryId = int.Parse(Request["category"]);
            return View(model);
        }

        [Authorize]
        [HttpPost]
        public ActionResult Create(OnBalance.Domain.Entities.Product model)
        {
            try
            {
                InfoFormat("User #{0} saving new product", User.Identity.Name);
                SetTempOkMessage("New product '{0}' was created", model.Name);
                return RedirectToAction("edit", new { id = model.Id });
            } catch(Exception ex)
            {
                Error("Error creating product!", ex);
                ModelState.AddModelError("", ex.Message);
            }

            return View(model);
        }

        //
        // GET: /pradmin/edit/1234

        [Authorize]
        public ActionResult Edit(int id)
        {
            InfoFormat("Editing product with ID #{0}", id);
            OnBalance.Domain.Entities.Product model = _productRepository.GetById(id);
            if(model == null)
            {
                ErrorFormat("User #{0} trying to edit non-existing product with ID: {1}!", User.Identity.Name, id);
                return new HttpNotFoundResult();
            }

            return View(new Product {
                Id = model.Id,
                Name = model.Name,
                CategoryId = model.CategoryId,
                CreatedAt = model.CreatedAt,
                InternalCode = model.InternalCode,
                PosId = model.PosId,
                Price = model.Price,
                StatusId = model.StatusId,
                UserId = model.UserId,
                Uid = model.Uid,
            });
        }

        //
        // POST: /pradmin/edit

        [Authorize]
        [HttpPost]
        public ActionResult Edit(OnBalance.Domain.Entities.Product model)
        {
            if( ModelState.IsValid )
            {
                model = _productRepository.GetById(model.Id);
                UpdateModel<OnBalance.Domain.Entities.Product>(model);
                _productRepository.Update(model);
                _productRepository.SubmitChanges();
                return RedirectToAction("Edit", new { id = model.Id });
            }

            return View(model);
        }

        //
        // POST: /pradmin/dodecorate/{PRODUCT_DETAILS_ID}?bf={BACKGROUND_COLOR}

        [HttpPost]
        public ActionResult DoDecorate(int id)
        {
            AjaxResponse r = new AjaxResponse();

            try
            {
                if (String.IsNullOrEmpty(Request["bg"]))
	            {
                    throw new ArgumentNullException("Background color is not specified");
	            }
                //var p = _productRepository.GetById(id);
                //if (p == null)
                //{
                //    throw new ArgumentNullException("Product does not exist. ID: " + id);
                //}
                var pd = _productRepository.GetDetailsById(id);
                if (pd == null)
                {
                    throw new ArgumentNullException("Details does not exist for Product ID: " + id);
                }

                var decorator = new ProductDecoratorColor();
                decorator.BackgroundColor = HttpUtility.HtmlEncode(Request["bg"].Trim());
                decorator.Color = "#000";
                decorator.Remarks = "";
                decorator.ProductId = pd.ProductId;
                decorator.SizeName = pd.ParameterValue;

                pd.DataJson = Newtonsoft.Json.JsonConvert.SerializeObject(decorator);
                _productRepository.Update(pd);
                _productRepository.SubmitChanges();
                
                r.Status = true;
                r.Message = "Product details are decorated";
            }
            catch (Exception ex)
            {
                r.Message = "Error decorating product";
                if (Request.IsLocal)
                {
                    r.Message = string.Concat(r.Message, Environment.NewLine, ex.ToString());
                }
            }

            return Json(r);
        }

        //
        // POST: /pradmin/donewsize/<PRODUCT_ID>?s=<SIZENAME>

        [HttpPost]
        public ActionResult DoNewSize(int id)
        {
            string sizeName = "";
            var pd = new OnBalance.Domain.Entities.ProductDetail();
            bool status = false;
            var sb = new StringBuilder();
            try
            {
                Info("Adding new size...");
                sizeName = Request["sname"].Trim().ToUpper();
                var details = _productRepository.GetDetailsByProduct(id);
                if (details.FirstOrDefault(x => x.ParameterValue == sizeName) != null)
                {
                    throw new ArgumentException("Size already exists: " + sizeName);
                }

                pd.Quantity = 0;
                pd.ParameterValue = sizeName;
                pd.ParameterName = "size";
                pd.ProductId = id;
                pd.CreatedAt = DateTime.UtcNow;
                pd.UpdatedAt = DateTime.UtcNow;
                pd.StatusId = (byte)Status.Pending;
                pd.DataJson = "";

                _productRepository.Save(pd);
                _productRepository.SubmitChanges();

                sb.AppendFormat("<div id=\"Qnt_{0}\" class=\"product-qnt\">{1}</div>", pd.Id, pd.Quantity);
                sb.AppendFormat("<div id=\"Decrease_{0}\" class=\"product-qnt-minus\"><img alt=\"-\" height=\"3\" src=\"/images/decrease.gif\" title=\"-\" width=\"6\"></div>", pd.Id);
                sb.AppendFormat("<div id=\"Increase_{0}\" class=\"product-qnt-plus\"><img alt=\"+\" height=\"3\" src=\"/images/increase.gif\" title=\"+\" width=\"6\"></div>", pd.Id);
                status = true;
            }
            catch (Exception ex)
            {
                Error("Error adding quantity for size: " + sizeName, ex);
                //resp.Message = ex.Message;
            }

            return Json(new
            {
                Status = status,
                HtmlData = sb.ToString(),
                NewSizeId = pd.Id
            });
        }

        /// <summary>
        /// Displays new product form (fro Ajax)
        /// </summary>
        /// <param name="id">POS ID</param>
        public ActionResult GetNewProduct(int id)
        {
            try
            {
                var model = new ProductNew();
                var pos = _organizationRepository.GetById(id);
                model.PosId = pos.Id;
                model.PosName = pos.Name;
                model.CategoryId = int.Parse(Request["categoryId"]);
                model.TotalSizes = HasRequestParameter("sizes") ? int.Parse(Request["sizes"]) : 0;
                var category = _productRepository.Categories.First(x => x.Id == model.CategoryId);
                    //.Select(x => new Category { Id = x.Id, Name = x.Name })
                    //.First(x => x.Equals(model.CategoryId));
                model.CategoryName = category.Name;

                return PartialView(model);
            }
            catch (Exception ex)
            {
                Error("Error getting new product form", ex);
                throw;
            }
        }

        [HttpPost]
        public ActionResult DoNewProduct(ProductNew model)
        {
            try
            {
                decimal d;
                var productPdo = new Domain.Entities.Product();
                productPdo.PosId = model.PosId;
                productPdo.CategoryId = model.CategoryId;
                productPdo.Name = HttpUtility.HtmlEncode(model.ProductName);
                productPdo.CreatedAt = DateTime.UtcNow;
                productPdo.UserId = "gj";
                productPdo.StatusId = (byte)Status.Pending;
                productPdo.InternalCode = model.InternalCode;
                productPdo.Uid = string.Concat("GJ_ES_", model.InternalCode);
                if (decimal.TryParse(model.PriceStr, out d))
                {
                    productPdo.Price = d;
                }
                _productRepository.Save(productPdo);
                _productRepository.SubmitChanges();

                var product = new Product(productPdo);

                ViewBag.TotalSizes = model.TotalSizes;
                return PartialView(product);
            }
            catch (Exception ex)
            {

                throw;
            }
        }

        public ActionResult ChangeQuantity(int id)
        {
            bool status = false;
            string message = "";
            int quantity = 0;

            try
            {
                var pd = _productRepository.GetDetailsById(id);
                if (pd == null)
                {
                    message = "Could not found ProductDetail";
                    ErrorFormat("Could not get ProductDetail by ID: {0}", id);
                }
                else
                {

                    var product = _productRepository.GetById(pd.ProductId);
                    if (product == null)
                    {
                        message = "Could not found Product to change quantity";
                        ErrorFormat("Could not get Product by ID: {0}", pd.ProductId);
                    }

                    var balanceItem = _balanceRepository.BalanceItems
                        .Where(x => x.InternalCode == product.InternalCode)
                        .Where(x => x.SizeName == pd.ParameterValue)
                        .Where(x => x.PosId == product.PosId)
                        .Where(x => x.StatusId == (byte)Status.Pending)
                        .FirstOrDefault();

                    int dQnt = int.Parse(Request["dQnt"]);
                    if (balanceItem == null)
                    {
                        balanceItem = new OnBalance.Domain.Entities.BalanceItem();
                        balanceItem.InternalCode = product.InternalCode;
                        balanceItem.SizeName = pd.ParameterValue;
                        balanceItem.PosId = product.PosId;
                        balanceItem.ProductName = product.Name;
                        balanceItem.StatusId = (byte)Status.Pending;
                        balanceItem.Quantity = dQnt;
                    }
                    else
                    {
                        balanceItem.Quantity += dQnt;
                    }

                    balanceItem.Price = pd.PriceMinor / 100;
                    balanceItem.PriceOfRelease = pd.PriceReleaseMinor / 100;
                    // Locally
                    balanceItem.ChangedFrom = 'L';

                    _balanceRepository.Save(balanceItem);
                    _balanceRepository.SubmitChanges();

                    quantity = balanceItem.Quantity;
                    status = true;
                }
            }
            catch (Exception ex)
            {
                message = ex.Message;
            }

            return Json(new
            {
                Staus = status,
                Message = message,
                Quantity = quantity
            }, JsonRequestBehavior.AllowGet);
        }

        private ProductsByCategoryViewModel GetProductsByCategories(int posId)
        {
            var con = new System.Data.SqlClient.SqlConnection(MySettings.ConnectionStringDefault);
            con.Open();
            var cmd = new System.Data.SqlClient.SqlCommand(
@"select 
    top 100
    p.id as id,             -- 0
    p.internal_code, 
    p.uid, 
    p.name, 
    p.price, 
	c.id as category_id,    -- 5
    c.name as category_name,
	isnull(pd.parameter_value, '') as size_name, 
    isnull(pd.price_minor, 0) as price_minor, 
    isnull(pd.price_release_minor, 0) as price_release_minor, 
    isnull(pd.quantity, 0),             -- 10
    isnull(pd.id, 0) as price_id,
    isnull(pd.data_json, '') as data_json
from product p
	join category c on p.category_id = c.id
	left join product_detail pd on p.id = pd.product_id
where p.pos_id = " + posId + @"
order by p.id", con);

            ProductsByCategoryViewModel productsByCategories = new ProductsByCategoryViewModel();
            var products = new List<OnBalance.Domain.Entities.Product>();
            decimal priceMinor, priceReleaseMinor;
            int priceId;
            string dataJson;
            Dictionary<int, string> categoryNames = new Dictionary<int, string>();

            var r = cmd.ExecuteReader();
            while (r.Read())
            {
                var p = new Product();
                p.Id = r.GetInt32(0);
                p.InternalCode = r.GetString(1);
                p.Uid = r.GetString(2);
                p.Name = r.GetString(3);
                //p.Price = r.GetDecimal(4);
                p.CategoryId = r.GetInt32(5);
                p.Price = r.GetDecimal(9);
                priceMinor = r.GetDecimal(8);
                priceReleaseMinor = r.GetDecimal(9);
                var psq = new ProductSizeQuantity();
                psq.SizeName = r.GetString(7);
                psq.Quantity = r.GetInt32(10);
                priceId = r.GetInt32(11);
                dataJson = r.GetString(12);

                // Store category name
                categoryNames[p.CategoryId] = r.GetString(6);

                var existing = products.FirstOrDefault(x => x.Id == p.Id);
                if (existing == null)
                {
                    var newP = new Domain.Entities.Product
                    {
                        Id = p.Id,
                        InternalCode = p.InternalCode,
                        Uid = p.Uid,
                        Name = p.Name,
                        CategoryId = p.CategoryId
                        
                    };
                    newP.ProductDetails.Add(new Domain.Entities.ProductDetail
                    {
                        Id = priceId,
                        ParameterValue = psq.SizeName,
                        Quantity = psq.Quantity,
                        PriceMinor = priceMinor,
                        PriceReleaseMinor = priceReleaseMinor,
                        DataJson = dataJson,
                    });
                    products.Add(newP);
                }
                else
                {
                    existing.ProductDetails.Add(new Domain.Entities.ProductDetail {
                        Id = priceId,
                        ParameterValue = psq.SizeName,
                        Quantity = psq.Quantity,
                        PriceMinor = priceMinor,
                        PriceReleaseMinor = priceReleaseMinor,
                        DataJson = dataJson
                    });
                }
            }

            productsByCategories = new ProductsByCategoryViewModel(products);

            // Fill category names
            foreach (var c in productsByCategories.ProductsByCategories)
            {
                var firstProduct = c.Products.FirstOrDefault();
                if (firstProduct != null)
                {
                    c.CategoryName = categoryNames.Keys.Contains(firstProduct.CategoryId) ? categoryNames[firstProduct.CategoryId] : "";
                }
            }
            return productsByCategories;
        }
    }
}
