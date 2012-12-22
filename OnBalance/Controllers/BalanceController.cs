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
                WebClient wc = new WebClient();
                string resp = wc.DownloadString(getBalanceUrl);
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
        // GET: /balance/

        [Authorize]
        public ActionResult Index()
        {
            return List(1);
        }

        //
        // GET: /balance/list

        [Authorize]
        public ActionResult List(int? id)
        {
            BalanceItemRepository db = new BalanceItemRepository();
            int currentPage = id.HasValue ? id.Value > 0 ? id.Value : 1 : 1;
            int perPage = 50;
            int offset = (currentPage - 1) * perPage;
            return View("List", Layout, db.GetLastUpdated(offset, perPage));
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
