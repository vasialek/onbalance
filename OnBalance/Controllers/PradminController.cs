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
            if(TempData["ExchangeItems"] != null)
            {
                Session["ExchangeItems"] = TempData["ExchangeItems"];
            }

            if(Session["ExchangeItems"] == null)
            {
                return Content("Error during parsing!");
                Session["ExchangeItems"] = new List<ExchangeItem> {
                    new ExchangeItem{ InternalCode = "00001", Name = "Adidas", Price = 250, PriceOfRelease = 340, Quantity = 2 }
                };
            }

            return View(Session["ExchangeItems"]);
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
            List<ExchangeItem> parsed = new List<ExchangeItem>();
            ExchangeItem exi = null;

            try
            {
                StringBuilder sb = new StringBuilder();
                string[] lines = model.Text.Split(Environment.NewLine.ToCharArray());
                int temp = 0;
                decimal tempPrice = 0m;
                string codeAndName = "";
                int pos = 0;

                for(int i = 0; i < lines.Length; i++)
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
                            exi = new ExchangeItem();
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
                                    exi.Name = codeAndName.Substring(0, codeAndName.Length - temp.ToString().Length);
                                }
                                sb.AppendFormat("Code: {0}, name: {1}, qnt: {2}, price: {3}, price of release: {4}", exi.InternalCode, exi.Name, exi.Quantity, exi.Price, exi.PriceOfRelease);

                                if( !string.IsNullOrEmpty(exi.Name) && !string.IsNullOrEmpty(exi.InternalCode) )
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
        // GET: /pradmin/confirm

        public ActionResult Confirm()
        {
            return View(TempData["ExchangeItems"]);
        }
    }
}
