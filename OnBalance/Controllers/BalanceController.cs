using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Net;
using System.Collections.Specialized;
using System.Text;
using System.Text.RegularExpressions;
using OnBalance.Helpers;
using OnBalance.ViewModels.Balance;
using OnBalance.ViewModels.Products;
using OnBalance.Domain.Abstract;
using Newtonsoft.Json;
using OnBalance.Models;
using OnBalance.Core;

namespace OnBalance.Controllers
{

    public class BalanceController : BaseController
    {
        private IOrganizationRepository _organizationRepository = null;
        private IProductRepository _productRepository = null;
        private IBalanceItemRepository _balanceItemsRepository = null;

        public BalanceController(IProductRepository productRepository, IOrganizationRepository organisationRepository, IBalanceItemRepository balanceItemsRepository)
        {
            if (productRepository == null)
            {
                throw new ArgumentNullException("productRepository");
            }
            if (organisationRepository == null)
            {
                throw new ArgumentNullException("organisationRepository");
            }
            if (balanceItemsRepository == null)
            {
                throw new ArgumentNullException("balanceItemsRepository");
            }

            _productRepository = productRepository;
            _organizationRepository = organisationRepository;
            _balanceItemsRepository = balanceItemsRepository;
        }

        //
        // GET: /balance/get

        public ActionResult Get()
        {
            // ID of POS must be!
            int posId = int.Parse(Request["posid"]);
            InfoFormat("Loading products for POS ID #{0}...", posId);

            int categoryId = 0;
            if( int.TryParse(Request["categoryid"], out categoryId) )
            {
                InfoFormat("  Applying filter by category ID: {0}", categoryId);
            }

            StringBuilder sbMain = new StringBuilder();
            StringBuilder sb = new StringBuilder();
            var products = _productRepository.Products
                .Where(x => x.PosId == posId && x.StatusId == (byte)Status.Approved);
            if( categoryId > 0 )
            {
                //products = products.Where(x => x.category_id == categoryId);
            }
            products = products.Take(200);
            InfoFormat("Got {0} products for POS ID #{1}", products == null ? "NULL" : products.Count().ToString(), posId);
            //var shops = dbPos.Items.ToList();
            string callback = Request["callback"];

            // Returns JSON array of all products for specified POS

            foreach(var p in products)
            {

                sb.Clear();
                foreach(var kvp in p.GetQuantityForAllSizes())
                {
                    sb.AppendFormat(", '{0}': {1}", kvp.Key, kvp.Value);
                }

                sbMain.AppendFormat("{{ name: \"{0}\", code: \"{1}\", price_minor: '{2}', amount: {3} {4} }},", p.Name, p.InternalCode, p.Price, 0, sb.ToString());
            }

            return Content(string.Format("{0}({{'data': [ {1} ]}})", callback, sbMain.ToString()));
        }

        public JsonpResult GetSchemaOfOrganizationsList(int id)
        {
            List<OrganizationSchemaViewModel> pos = new List<OrganizationSchemaViewModel>();
            var orgs = _organizationRepository.Organizations
                .Where(x => x.Id.Equals(id) || x.ParentId.Equals(id) && x.StatusId == (byte)Status.Approved)
                .ToList();

            foreach(var item in orgs)
            {
                var categories = _productRepository.Categories.Where(x => x.OrganizationId == item.Id);
                List<CategoryStructureViewModel> cs = new List<CategoryStructureViewModel>();
                foreach (var c in categories)
	            {
                    cs.Add(new CategoryStructureViewModel
                    {
                        Id = c.Id,
                        Name = c.Name,
                        Sizes = _productRepository.GetAvailableSizes(c.Id) ?? new string[] { "" }
                    });
	            }
                var orgVm = new OrganizationSchemaViewModel
                {
                    Id = item.Id,
                    Name = item.Name,
                    Categories = cs.ToArray()
                };

                pos.Add(orgVm);
            }

            return new JsonpResult
            {
                Data = new
                {
                    Results = pos
                },
                JsonRequestBehavior = JsonRequestBehavior.AllowGet
            };
        }

        //
        // GET: /balance/getorganizationstructure/100002

        public JsonpResult GetOrganizationSchema(int id)
        {
            throw new Exception("Use GetSchemaOfOrganizationsList method instead!");

            InfoFormat("Preparing JSON schema for main page (shops, categories). POS ID is {0}", id);
            OrganizationSchemaViewModel orgVm = new OrganizationSchemaViewModel();

            var pos = _organizationRepository.GetById(id);
            orgVm.Id = pos.Id;
            orgVm.Name = pos.Name;

            //orgVm.Id = 100002;
            //orgVm.Name = "GJ";
            orgVm.ReceivedAt = Common.GetTimestamp(DateTime.Now);

            var categories = new List<CategoryStructureViewModel>();
            foreach(var c in _productRepository.Categories.Where(x => x.OrganizationId == pos.Id))
            {
                categories.Add(new CategoryStructureViewModel
                {
                    Id = c.Id,
                    Name = c.Name,
                    Sizes = _productRepository.GetAvailableSizes(c.Id)
                });
            }
            //int catId = 1000;
            
            //categories.Add(new CategoryStructureViewModel
            //{
            //    Id = ++catId,
            //    Name = "Avalyne",
            //    Sizes = dbProduct.GetAvailableSizes(catId)
            //});

            //categories.Add(new CategoryStructureViewModel
            //{
            //    Id = ++catId,
            //    Name = "Apranga vyrams ir apranga moteris",
            //    Sizes = dbProduct.GetAvailableSizes(catId)
            //});

            //categories.Add(new CategoryStructureViewModel
            //{
            //    Id = ++catId,
            //    Name = "Apranga vaikams",
            //    Sizes = dbProduct.GetAvailableSizes(catId)
            //});

            //categories.Add(new CategoryStructureViewModel
            //{
            //    Id = ++catId,
            //    Name = "Kepures (priedai)",
            //    Sizes = dbProduct.GetAvailableSizes(catId)
            //});

            ////categories.Add(new ViewModels.Balance.CategoryStructureViewModel
            ////{
            ////    Id = ++catId,
            ////    Name = "Kojines (priedai)",
            ////    Sizes = new string[] { "122cm", "128", "134", "140", "152", "158", "164", "170", "176" }
            ////});

            //categories.Add(new CategoryStructureViewModel
            //{
            //    Id = ++catId,
            //    Name = "Kamuoliai (kita)",
            //    Sizes = dbProduct.GetAvailableSizes(catId)
            //});

            List<OrganizationViewModel> shops = new List<OrganizationViewModel>();
            //var q = dbOrg.Items
            //    .Where(x => x.StatusId == (byte)Status.Approved && (x.Id == id || x.ParentId == id));
            var q = _organizationRepository.GetByParentId(pos.ParentId);
            InfoFormat("Organization which belong to organization #{0}", id);
            foreach(var item in q)
            {
                InfoFormat("  #{0}. {1}", item.Id, item.Name);
                shops.Add(new OrganizationViewModel
                {
                    Id = item.Id,
                    Name = item.Name
                });
            }

            orgVm.Categories = categories.ToArray();
            orgVm.Shops = shops;
            return new JsonpResult
            {
                Data = new { Results = orgVm },
                JsonRequestBehavior = JsonRequestBehavior.AllowGet
            };
        }

        //
        // GET: /balance/getremotechanges/1234

        public ActionResult GetRemoteChanges(int id)
        {
            try
            {
                string getBalanceUrl = "http://gjsportland.com/index.php/lt/balance/get?_token=12345";
                InfoFormat("Going to download changes from POS: {0}", getBalanceUrl);
                WebClient wc = new WebClient();
                string resp = wc.DownloadString(getBalanceUrl);
                DebugFormat("Changes: {0}", resp);

                var exchanges = JsonConvert.DeserializeObject<List<ExchangeJson>>(resp);
                var model = exchanges.Select(x => new OnBalance.Models.BalanceItem
                {
                    InternalCode = x.uid,
                    ProductName = string.Concat(x.code, ". ", x.name),
                    Price = x.pr / 100,
                    QuantityForSizes = x.sizes
                }).ToList();
                return PartialView("", model);
            } catch(Exception ex)
            {
                return Content(ex.Message);
            }
        }

        //
        // GET: /balance/dosyncfrompos/1234

        public ActionResult DoSyncFromPos()
        {
            int temp;
            List<OnBalance.Models.BalanceItem> list = new List<OnBalance.Models.BalanceItem>();
            StringBuilder sb = new StringBuilder();
            //OnBalance.Models.BalanceItemRepository db = new OnBalance.Models.BalanceItemRepository();
            try
            {
                string getBalanceUrl = "http://gjsportland.com/index.php/lt/balance/get?_token=12345";
                InfoFormat("Going to download changes from POS: {0}", getBalanceUrl);
                WebClient wc = new WebClient();
                string resp = wc.DownloadString(getBalanceUrl);
                DebugFormat("Changes: {0}", resp);
                Regex rx = new Regex(@"(\{)([^}]+)", RegexOptions.IgnoreCase | RegexOptions.CultureInvariant | RegexOptions.Singleline);
                // ("uid"\:")([^""]+)([\"\,\ ]*)(code\"\:\")([^"]+)([\"\,\ ]*)(pr\"\:)(\d+)([,"\s]+)(posid"\:)(\d+)([\"\,\ ]*)(name\"\:\")([^"]+)
                Regex regex = new Regex(@"(""uid""\:"")([^""""]+)([\""\,\ ]*)(code\""\:\"")([^""]+)([\""\,\ ]*)(pr\""\:)(\d+)([,""\s]+)(posid""\:)(\d+)([\""\,\ ]*)(name\""\:\"")([^""]+)", RegexOptions.IgnoreCase | RegexOptions.CultureInvariant | RegexOptions.Multiline | RegexOptions.Singleline);
                Regex rxSizes = new Regex(@"([\d\.]+)(=)(\d)(\:)");
                Match m = rx.Match(resp);
                // List all products in JSON
                while( m.Success )
                {
                    OnBalance.Models.BalanceItem bi = new OnBalance.Models.BalanceItem();
                    string line = m.Groups[2].Value;
                    Match matchLine = regex.Match(line);
                    string uid = matchLine.Groups[2].Value;
                    bi.InternalCode = matchLine.Groups[5].Value;
                    int.TryParse(matchLine.Groups[8].Value, out temp);
                    bi.Price = temp / 100.0m;
                    int.TryParse(matchLine.Groups[11].Value, out temp);
                    bi.PosId = temp;
                    bi.ProductName = matchLine.Groups[14].Value.Trim();
                    int start = line.IndexOf('[', matchLine.Groups[5].Index);
                    int end = line.LastIndexOf(']');
                    string sizes = line.Substring(start, end - start);
                    Match mSize = rxSizes.Match(sizes);
                    while( mSize.Success )
                    {
                        bi.QuantityForSizes[mSize.Groups[1].Value] = mSize.Groups[3].Value;
                        mSize = mSize.NextMatch();
                    }

                    //db.Save(bi);

                    list.Add(bi);
                    m = m.NextMatch();
                }

                //Regex regex = new Regex(@"(\""uid\""\:\"")([^""]+)([\""\,\ ]*)(code\""\:\"")([^""]+)", RegexOptions.IgnoreCase | RegexOptions.CultureInvariant | RegexOptions.Multiline | RegexOptions.Singleline);
                //MatchCollection matchCollection = regex.Matches(resp);
                //foreach( Match match in matchCollection )
                //{
                //    string uid = match.Groups[2].Value;
                //    string code = match.Groups[5].Value;
                //    Log4cs.Log("Uid: {0}, code: {1}", uid, code);
                //}

            } catch( Exception ex )
            {
                throw ex;
                return Json(new
                {
                    Status = (int)Status.Failed,
                    Msg = "Error getting from POS!"
                }, JsonRequestBehavior.AllowGet);
            }

            return Json(new
            {
                Status = (int)Status.Completed,
                Msg = "Got from POS",
                Results = list.ToArray()
            }, JsonRequestBehavior.AllowGet);
        }

        //
        // GET: /balance/dosend

        //[HttpPost]
        public ActionResult DoSend(OnBalance.Models.BalanceItem[] items)
        {
            try
            {
                InfoFormat("Balance/DoSend [{0}] was called...", Request.HttpMethod);
                InfoFormat("Got {0} items to post to POS...", items == null ? -1 : items.Length);
                foreach(string key in Request.Form.AllKeys)
                {
                    DebugFormat("  {0}: {1}", key, Request.Form[key]);
                }
                string callback = Request["callback"] as string;
                PostBalanceViewModel bal = new PostBalanceViewModel();
                bal.Results = items;

                NameValueCollection nvc = new NameValueCollection();
                nvc.Add("_token", bal._token);
                nvc.Add("Total", bal.Total.ToString());
                for(int i = 0; i < bal.Total; i++)
                {
                    nvc.Add(string.Format("Results[{0}][InternalCode]", i), bal.Results[i].InternalCode);
                    nvc.Add(string.Format("Results[{0}][Name]", i), bal.Results[i].ProductName);
                    nvc.Add(string.Format("Results[{0}][Price]", i), (bal.Results[i].Price * 100).ToString("#####"));
                    nvc.Add(string.Format("Results[{0}][Details]['39']", i), "1");
                    nvc.Add(string.Format("Results[{0}][Details]['41']", i), "2");

                }
                //WebClient wc = new WebClient();
                //string posBalanceUrl = "http://gjsportland.com/test.php/lt/product/dobalance";
                //byte[] ba = wc.UploadValues(posBalanceUrl, "POST", nvc);
                ////return Json(bal, JsonRequestBehavior.AllowGet);
                //Log.InfoFormat("Got response from POS: {0}", Encoding.ASCII.GetString(ba));
                //return Content("Got: " + Encoding.ASCII.GetString(ba));

                if(string.IsNullOrEmpty(callback))
                {
                    return Json(new
                    {
                        Status = Status.Completed
                    }, JsonRequestBehavior.AllowGet);
                }
                return Content(string.Format("{0}({{ \"Status\": \"{1}\" }})", callback, Status.Completed));

            } catch(Exception ex)
            {
                Error("Error sending local changes to POS!", ex);
                throw ex;
            }
        }

        //
        // GET: /balance/confir/10843

        [Authorize]
        public ActionResult Confirm(int id)
        {
            //var db = new OnBalance.Models.BalanceItemRepository();
            var itemDbo = _balanceItemsRepository.BalanceItems.SingleOrDefault(x => x.Id == id);
            if(itemDbo == null)
            {
                return HttpNotFound();
            }

            //var product = _productRepository.Products
            //    .Where(x => x.InternalCode == itemDbo.InternalCode)
            //    .Where(x => x.PosId == itemDbo.PosId)
            //    .FirstOrDefault();
            //if (product == null)
            //{
            //    return HttpNotFound();
            //}

            //// TODO: build model for this view
            //ViewBag.Product = new Product(product);
            return View("Confirm", new BalanceItem(itemDbo));
        }

        //
        // POST: /balance/confirm

        [HttpPost]
        [Authorize]
        public ActionResult Confirm(int id, string confirm, string sync)
        {
            if (String.IsNullOrEmpty(confirm))
            {
                return RedirectToAction("confirm", new { id = id });
            }

            bool doSyncWithEshop = String.IsNullOrEmpty(sync) == false;

            InfoFormat("Going to confirm updated product ID #{0} from POS", id);
            var item = _balanceItemsRepository.BalanceItems.SingleOrDefault(x => x.Id == id);
            if(item == null)
            {
                return HttpNotFound();
            }

            InfoFormat("Searching for product with code: [{0}]", item.InternalCode);
            var product = _productRepository.Products
                .Where(x => x.PosId == item.PosId)
                .SingleOrDefault(x => x.InternalCode == item.InternalCode);
            if(product == null)
            {
                return HttpNotFound();
            }

            try
            {
                if (!string.IsNullOrWhiteSpace(item.ProductName))
                {
                    product.Name = item.ProductName;
                }
                product.Price = item.Price;

                // Call and update one product http://www.gjsportland.com/index.php/en/balance/callback?_token=123&act=chq&code=qwer

                if (doSyncWithEshop)
                {
                    InfoFormat("Sending synchronization request to eshop...");
                    string updateUrl = string.Format("http://www.gjsportland.com/index.php/en/balance/callback?_token={0}&act=chq&code={1}&size={2}&qnt={3}&price={4}", "gj-fake-token", item.InternalCode, item.SizeName, item.Quantity, item.Price * 100);
                    WebClient wc = new WebClient();
                    string s = wc.DownloadString(updateUrl);
                    if (String.IsNullOrEmpty(s))
                    {
                        throw new Exception("Got empty response from e-shop");
                    }

                    var resp = Newtonsoft.Json.JsonConvert.DeserializeObject<AjaxResponse>(s);
                    if (resp.Status == false)
                    {
                        throw new Exception(resp.Message);
                    }
                }
                else
                {
                    Warn("Do NOT synchronize with eshop");
                }
                //return Content("DONE: " + resp.Message);

                InfoFormat("Going to save changes from POS to Online Balance System DB...");
                _productRepository.Update(product);

                // Change quantity for size
                var details = _productRepository.GetDetailsByProduct(product.Id);
                if (details != null)
                {
                    var pd = details.FirstOrDefault(x => x.ParameterValue == item.SizeName);
                    if (pd != null)
                    {
                        pd.Quantity += item.Quantity;
                        if (pd.Quantity < 0)
                        {
                            pd.Quantity = 0;
                        }
                        _productRepository.Update(pd);
                        _productRepository.SubmitChanges();
                    }
                }

                item.StatusId = (byte)Status.Completed;
                _balanceItemsRepository.Save(item);

                _productRepository.SubmitChanges();
                _balanceItemsRepository.SubmitChanges();

                SetTempOkMessage("Product {0} is updated", item.InternalCode);
                return RedirectToAction("list", new { id = item.PosId });
            }
            catch (Exception ex)
            {
                var itemDbo = _balanceItemsRepository.BalanceItems.SingleOrDefault(x => x.Id == id);
                if (itemDbo == null)
                {
                    return HttpNotFound();
                }
                this.SetErrorMessage(ex.Message);
                return View("Confirm", new BalanceItem(itemDbo));
            }
        }

        //
        // GET: /balance/

        [Authorize]
        public ActionResult Index()
        {
            return List(_organizationRepository.Organizations.FirstOrDefault().Id);
        }

        //
        // GET: /balance/edit/100002

        [Authorize]
        public ActionResult Edit(int id)
        {
            ProductsInPosViewModel pb = new ProductsInPosViewModel();

            InfoFormat("Selecting products for POS #{0}", id);
            pb.Pos = _organizationRepository.GetById(id);
            pb.Products = _productRepository.Products
                .Where(x => x.PosId == id && x.StatusId == (byte)Status.Approved)
                .Take(100)
                .Select(x => new Models.Product {
                    
                })
                .ToList();
            pb.Organizations = _organizationRepository.Organizations.ToList();
            return View(pb);
        }

        //
        // GET: /balance/list/100001

        [Authorize]
        public ActionResult List(int id)
        {
            // List all not-synced products between OBS and POS

            int currentPage = 0;
            int.TryParse(Request["p"], out currentPage);
            
            currentPage = currentPage > 0 ? currentPage : 1;
            int perPage = 50;
            int offset = (currentPage - 1) * perPage;
            var list = _balanceItemsRepository.BalanceItems
                .Where(x => x.PosId == id)
                .OrderByDescending(x => x.Id)
                .ThenBy(x => x.InternalCode)
                .Skip(offset)
                .Take(perPage)
                .ToList()
                .Select(x => new BalanceItem(x));

            SetTempMessagesToViewBag();
            return View("List", Layout, list);
        }

        //
        // GET: /balance/delete/1000

        [Authorize]
        public ActionResult Delete(int id)
        {
            BalanceItem bi = null;
            try
            {
                bi = new BalanceItem(_balanceItemsRepository.BalanceItems.First(x => x.Id.Equals(id)));
            }
            catch (Exception ex)
            {
                SetErrorMessage("Error cancelling product change: {0}", ex.Message);
                Error("Error showing confirmation to delete from balance_item", ex);
            }

            return View("Delete", bi);
        }

        //
        // POST: /balance/delete

        [HttpPost]
        [Authorize]
        public ActionResult Delete(int id, string confirm)
        {
            Domain.Entities.BalanceItem bi = null;
            int posId = -1;
            
            if (string.IsNullOrEmpty(confirm))
            {
                return RedirectToAction("delete", new { id = id });
            }

            try
            {
                InfoFormat("Going to delete balance_item #{0}...", id);
                bi = _balanceItemsRepository.BalanceItems.First(x => x.Id.Equals(id));
                posId = bi.PosId;

                _balanceItemsRepository.Delete(bi);
                _balanceItemsRepository.SubmitChanges();
            }
            catch (Exception ex)
            {
                SetErrorMessage("Error cancelling product change: {0}", ex.Message);
                Error("Error deleting from balance_item", ex);
                return View("Delete", new BalanceItem(bi));
            }

            return posId > 0 ? RedirectToAction("list", new { id = posId }) : RedirectToAction("list");
        }

        public ActionResult GetProductInfo(string id)
        {
            Status s = Status.Approved;
            var productDbo = _productRepository.GetByUid(id);
            if (productDbo == null)
            {
                return Json(new { Status = Status.Unknown }, JsonRequestBehavior.AllowGet);
            }

            return Json(new {
                Status = s,
                Name = productDbo.Name,
                ProductStatus = productDbo.StatusId
            }, JsonRequestBehavior.AllowGet);
        }
    }

    public class PostBalanceViewModel
    {
        public PostBalanceViewModel()
        {
            _token = "123456";
        }

        public string _token { get; set; }

        public int Total { get { return Results == null ? 0 : Results.Length; } }

        public OnBalance.Models.BalanceItem[] Results { get; set; }
    }

}
