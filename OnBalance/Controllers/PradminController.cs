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
using OnBalance.ViewModels.Product;

namespace OnBalance.Controllers
{
    [Authorize]
    public class PradminController : Controller
    {
        //
        // GET: /pradmin/

        public ActionResult Index()
        {
            return View();
        }

        //
        // GET: /pradmin/balance/123

        public ActionResult Balance(int id)
        {
            ProductBalanceViewModel pb = new ProductBalanceViewModel();
            ProductRepository db = new ProductRepository();
            PosRepository dbPos = new PosRepository();

            pb.Products = db.Items.Where(x => x.pos_id == id && x.status_id == (byte)Status.Approved).ToList();
            pb.Shops = dbPos.Items.ToList();
            return View(pb);
        }

        //
        // GET: /pradmin/get/100001

        public ActionResult Get(int id)
        {
            ProductRepository db = new ProductRepository();
            PosRepository dbPos = new PosRepository();

            StringBuilder sbMain = new StringBuilder();
            StringBuilder sb = new StringBuilder();
            var products = db.Items.Where(x => x.pos_id == id && x.status_id == (byte)Status.Approved).ToList();
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

                sbMain.AppendFormat("{{ name: \"{0}\", code: \"{1}\", price_minor: '{2}', amount: {3} {4} }},", p.name, p.internal_code, p.price, 0, sb.ToString());
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

        public ActionResult Parse(int id)
        {
            return View();
        }

        // POST: /pradmin/parse

        [HttpPost]
        public ActionResult Parse(TextParserViewModel model)
        {
            List<BalanceItem> parsed = new List<BalanceItem>();
            BalanceItem exi = null;

            try
            {
                StringBuilder sb = new StringBuilder();
                string[] lines = model == null ? null : model.Text.Split(Environment.NewLine.ToCharArray());
                int temp = 0;
                decimal tempPrice = 0m;
                string codeAndName = "";
                int pos = 0;

                for(int i = 0; (lines != null) && (i < lines.Length); i++)
                {
                    string line = lines[i].Trim();
                    string lineNr = (i + i).ToString();
                    if(line.Length < 5)
                    {
                        //ModelState.AddModelError("Text", MyMessages.Parser.LineIsTooShortFmt.Replace("%NR%", lineNr).Replace("%LENGTH%", line.Length.ToString()));
                    } else
                    {
                        string[] ar = line.Split(new char[] { '\t' });
                        if(ar.Length < 4)
                        {
                            ModelState.AddModelError("Text", MyMessages.Parser.SplittedLineHasTooLessArgumentsFmt.Replace("%NR%", lineNr));
                        } else
                        {
                            exi = new BalanceItem();
                            codeAndName = ar[0].Trim();
                            exi.Quantity = int.TryParse(ar[2].Trim(), out temp) ? temp : -1;
                            exi.Price = decimal.TryParse(ar[3].Trim(), System.Globalization.NumberStyles.Currency, CultureInfo.InvariantCulture, out tempPrice) ? tempPrice : -1;
                            exi.PriceOfRelease = decimal.TryParse(ar[ar.Length - 1].Trim(), NumberStyles.Currency, CultureInfo.InvariantCulture, out tempPrice) ? tempPrice : -1;

                            // Search for N-digit code
                            pos = codeAndName.LastIndexOf(' ');
                            if(pos >= 0)
                            {
                                exi.InternalCode = "---";
                                if(int.TryParse(codeAndName.Substring(pos), out temp))
                                {
                                    exi.InternalCode = temp.ToString();
                                    exi.ProductName = codeAndName.Substring(0, codeAndName.Length - temp.ToString().Length);
                                }
                                sb.AppendFormat("Code: {0}, name: {1}, qnt: {2}, price: {3}, price of release: {4}", exi.InternalCode, exi.ProductName, exi.Quantity, exi.Price, exi.PriceOfRelease);

                                if( !string.IsNullOrEmpty(exi.ProductName) && !string.IsNullOrEmpty(exi.InternalCode) )
                                {
                                    parsed.Add(exi);
                                }
                            } else
                            {
                                ModelState.AddModelError("Text", MyMessages.Parser.CouldNotParseCodeForLineFmt.Replace("%NR%", lineNr));
                            }

                        }
                    }
                }

                if( parsed.Count > 0 )
                {
                    TempData["ExchangeItems"] = parsed;
                    if(Request.Params.AllKeys.Contains("ExportBtn"))
                    {
                        return RedirectToAction("export", new { id = 100001 });
                    }
                    return RedirectToAction("balance", new { id = 100001 });
                }

                return Content(sb.ToString());

            } catch(Exception ex)
            {
                return Content("Error: " + ex.ToString());
            }

            return View(model);
        }

        //
        // GET: /pradmin/export/100001

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
    }
}
