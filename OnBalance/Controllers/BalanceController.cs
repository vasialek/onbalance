using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Net;
using System.Collections.Specialized;
using OnBalance.Models;
using System.Text;
using System.Text.RegularExpressions;
using OnBalance.Helpers;

namespace OnBalance.Controllers
{

    public class BalanceController : BaseController
    {
        //
        // GET: /balance/get

        public ActionResult Get()
        {
            string token = Request["_token"];
            return Json(new{
                Status = 1
            }, JsonRequestBehavior.AllowGet);
        }

        //
        // GET: /balance/getorganizationstructure/100002

        public JsonpResult GetOrganizationStructure(int id)
        {
            ViewModels.Balance.OrganizationStructureViewModel orgVm = new ViewModels.Balance.OrganizationStructureViewModel();
            orgVm.Id = 100002;
            orgVm.Name = "GJ";
            orgVm.ReceivedAt = OnBalance.Helpers.Common.GetTimestamp(DateTime.Now);

            var categories = new List<ViewModels.Balance.CategoryStructureViewModel>();
            int catId = 1000;
            
            categories.Add(new ViewModels.Balance.CategoryStructureViewModel
            {
                Id = ++catId,
                Name = "Avalyne",
                Sizes = new string[] { "33", "34", "35", "35,5", "36", "36,5", "37", "37,5", "38", "38,5", "39", "40", "41", "42", "42.5", "43", "44", "44.5", "45", "45,5", "46", "46.5", "47", "47,5", "48", "48,5", "49", "49.5", "50", "50,5", "51", "52", "52,5", "53", "54" }
            });

            categories.Add(new ViewModels.Balance.CategoryStructureViewModel
            {
                Id = ++catId,
                Name = "Apranga vyrams ir apranga moteris",
                Sizes = new string[] { "XXS", " XS", " S", " M", " L", " XL", " XXL", " XXXL" }
            });

            categories.Add(new ViewModels.Balance.CategoryStructureViewModel
            {
                Id = ++catId,
                Name = "Apranga vaikams",
                Sizes = new string[] { "122cm", "128", "134", "140", "152", "158", "164", "170", "176" }
            });

            categories.Add(new ViewModels.Balance.CategoryStructureViewModel
            {
                Id = ++catId,
                Name = "Kepures (priedai)",
                Sizes = new string[] { "OSFC", "OSFY", "OSFW", "OSFM", "OSFL", "MISC" }
            });

            //categories.Add(new ViewModels.Balance.CategoryStructureViewModel
            //{
            //    Id = ++catId,
            //    Name = "Kojines (priedai)",
            //    Sizes = new string[] { "122cm", "128", "134", "140", "152", "158", "164", "170", "176" }
            //});

            categories.Add(new ViewModels.Balance.CategoryStructureViewModel
            {
                Id = ++catId,
                Name = "Kamuoliai (kita)",
                Sizes = new string[] { "1", "2", "3", "4", "5", "6", "7" }
            });

            orgVm.Categories = categories.ToArray();
            return new JsonpResult{
                Data = orgVm, 
                JsonRequestBehavior = JsonRequestBehavior.AllowGet
            };
        }

        //
        // GET: /balance/dosyncfrompos/1234

        public ActionResult DoSyncFromPos()
        {
            int temp;
            List<BalanceItem> list = new List<BalanceItem>();
            StringBuilder sb = new StringBuilder();
            BalanceItemRepository db = new BalanceItemRepository();
            try
            {
                string getBalanceUrl = "http://gjsportland.com/index.php/lt/balance/get?_token=12345";
                Log.InfoFormat("Going to download changes from POS: {0}", getBalanceUrl);
                WebClient wc = new WebClient();
                string resp = wc.DownloadString(getBalanceUrl);
                Log.DebugFormat("Changes: {0}", resp);
                Regex rx = new Regex(@"(\{)([^}]+)", RegexOptions.IgnoreCase | RegexOptions.CultureInvariant | RegexOptions.Singleline);
                // ("uid"\:")([^""]+)([\"\,\ ]*)(code\"\:\")([^"]+)([\"\,\ ]*)(pr\"\:)(\d+)([,"\s]+)(posid"\:)(\d+)([\"\,\ ]*)(name\"\:\")([^"]+)
                Regex regex = new Regex(@"(""uid""\:"")([^""""]+)([\""\,\ ]*)(code\""\:\"")([^""]+)([\""\,\ ]*)(pr\""\:)(\d+)([,""\s]+)(posid""\:)(\d+)([\""\,\ ]*)(name\""\:\"")([^""]+)", RegexOptions.IgnoreCase | RegexOptions.CultureInvariant | RegexOptions.Multiline | RegexOptions.Singleline);
                Regex rxSizes = new Regex(@"([\d\.]+)(=)(\d)(\:)");
                Match m = rx.Match(resp);
                // List all products in JSON
                while( m.Success )
                {
                    BalanceItem bi = new BalanceItem();
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

                    db.Save(bi);

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
        public ActionResult DoSend(BalanceItem[] items)
        {
            try
            {
                Log.InfoFormat("Balance/DoSend [{0}] was called...", Request.HttpMethod);
                Log.InfoFormat("Got {0} items to post to POS...", items == null ? -1 : items.Length);
                foreach(string key in Request.Form.AllKeys)
                {
                    Log.DebugFormat("  {0}: {1}", key, Request.Form[key]);
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
                Log.Error("Error sending local changes to POS!", ex);
                throw ex;
            }
        }

        //
        // GET: /balance/confir/10843

        [Authorize]
        public ActionResult Confirm(int id)
        {
            var db = new BalanceItemRepository();
            var item = new BalanceItemRepository().Items.SingleOrDefault(x => x.Id == id);
            if(item == null)
            {
                return HttpNotFound();
            }
            return View("Confirm", item);
        }

        //
        // POST: /balance/confirm

        [HttpPost]
        public ActionResult Confirm(int id, string confirm)
        {
            var dbProducts = new ProductRepository();
            var dbBalanceItems = new BalanceItemRepository();
            Log.InfoFormat("Going to confirm updated product ID #{0} from POS", id);
            var item = dbBalanceItems.Items.SingleOrDefault(x => x.Id == id);
            if(item == null)
            {
                return HttpNotFound();
            }

            Log.InfoFormat("Searching for product with code: [{0}]", item.InternalCode);
            var product = dbProducts.Items.SingleOrDefault(x => x.internal_code == item.InternalCode);
            if(product == null)
            {
                return HttpNotFound();
            }

            if(!string.IsNullOrWhiteSpace(item.ProductName))
            {
                product.name = item.ProductName;
            }
            product.price = item.Price;
            Log.InfoFormat("Going to save changes from POS to Online Balance System DB...");
            dbProducts.Update(product);

            item.StatusId = (byte)Status.Completed;
            dbBalanceItems.Save(item);

            return RedirectToAction("list", new { id = item.PosId });
        }

        //
        // GET: /balance/

        [Authorize]
        public ActionResult Index()
        {
            return List(new OrganizationRepository().Organizations.FirstOrDefault().Id);
        }

        //
        // GET: /balance/list/100001

        [Authorize]
        public ActionResult List(int id)
        {
            BalanceItemRepository db = new BalanceItemRepository();
            int currentPage = 0;
            int.TryParse(Request["p"], out currentPage);
            
            currentPage = currentPage > 0 ? currentPage : 1;
            int perPage = 50;
            int offset = (currentPage - 1) * perPage;
            var list = db.Items
                .Where(x => x.PosId == id)
                .Skip(offset)
                .Take(perPage);
            return View("List", Layout, list);
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

        public BalanceItem[] Results { get; set; }
    }

}
