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
            try
            {
                string getBalanceUrl = "http://gjsportland.com/index.php/lt/balance/get?_token=12345";
                WebClient wc = new WebClient();
                Log4cs.Log("Dowloading from POS: {0}", getBalanceUrl);
                string resp = wc.DownloadString(getBalanceUrl);
                Log4cs.Log("Response from POS: {0}", resp);
                Regex rx = new Regex(@"(\{)([^}]+)", RegexOptions.IgnoreCase | RegexOptions.CultureInvariant | RegexOptions.Singleline);
                // ("uid"\:")([^""]+)([\"\,\ ]*)(code\"\:\")([^"]+)([\"\,\ ]*)(pr\"\:)(\d+)([,"\s]+)(posid"\:)(\d+)
                Regex regex = new Regex(@"(""uid""\:"")([^""""]+)([\""\,\ ]*)(code\""\:\"")([^""]+)([\""\,\ ]*)(pr\""\:)(\d+)([,""\s]+)(posid""\:)(\d+)", RegexOptions.IgnoreCase | RegexOptions.CultureInvariant | RegexOptions.Multiline | RegexOptions.Singleline);
                Log4cs.Log("{0}", regex.ToString());
                Regex rxSizes = new Regex(@"([\d\.]+)(=)(\d)(\:)");
                Match m = rx.Match(resp);
                // List all products in JSON
                while( m.Success )
                {
                    BalanceItem bi = new BalanceItem();
                    string line = m.Groups[2].Value;
                    Log4cs.Log("Product to upadate: {0}", line);
                    Match matchLine = regex.Match(line);
                    string uid = matchLine.Groups[2].Value;
                    bi.InternalCode = matchLine.Groups[5].Value;
                    Log4cs.Log("Uid: {0}, code: {1}", uid, bi.InternalCode);
                    Log4cs.Log("Price (minor): {0}", matchLine.Groups[8].Value);
                    int.TryParse(matchLine.Groups[8].Value, out temp);
                    bi.Price = temp / 100.0m;
                    Log4cs.Log("ID of POS: {0}", matchLine.Groups[11].Value);
                    int.TryParse(matchLine.Groups[11].Value, out temp);
                    bi.PosId = temp;
                    int start = line.IndexOf('[', matchLine.Groups[5].Index);
                    int end = line.LastIndexOf(']');
                    string sizes = line.Substring(start, end - start);
                    Log4cs.Log("Sizes to extract: {0}", sizes);
                    Match mSize = rxSizes.Match(sizes);
                    while( mSize.Success )
                    {
                        Log4cs.Log("  {0} = {1}", mSize.Groups[1].Value, mSize.Groups[3].Value);
                        bi.QuantityForSizes[mSize.Groups[1].Value] = mSize.Groups[3].Value;
                        mSize = mSize.NextMatch();
                    }

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
                Log4cs.Log(Importance.Error, "Error parsing syncs from POS!");
                Log4cs.Log(Importance.Debug, ex.ToString());
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
