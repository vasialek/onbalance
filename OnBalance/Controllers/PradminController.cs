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

namespace OnBalance.Controllers
{

    public class PradminController : BaseController
    {
        //
        // GET: /pradmin/

        [Authorize]
        public ActionResult Index()
        {
            return List(new PosRepository().Items.First().Id);
        }

        //
        // GET: /pradmin/list/100003

        [Authorize]
        public ActionResult List(int id)
        {
            ProductsInPosViewModel productsList = new ProductsInPosViewModel();
            productsList.Pos = new PosRepository().Items.SingleOrDefault(x => x.Id == id);
            if( productsList.Pos == null )
            {
                ErrorFormat("Trying to list products in non-existing POS #{0}!", id);
                return HttpNotFound();
            }

            int perPage = 50;
            int page = 0;
            if( int.TryParse(Request["p"], out page) == false )
            {
                page = 1;
            }
            int offset = (page - 1) * perPage;

            InfoFormat("Displaying list of products in POS #{0}, skipping {1}, taking {2} products", id, offset, perPage);
            productsList.Products = new ProductRepository().GetLastInPos(id, offset, perPage)
                .OrderBy(x => x.id)
                .ToList();

            return View("List", Layout, productsList);
        }

        //
        // GET: /pradmin/balance/123

        public ActionResult Balance(int id)
        {
            ProductsInPosViewModel pb = new ProductsInPosViewModel();
            ProductRepository db = new ProductRepository();
            PosRepository dbPos = new PosRepository();

            InfoFormat("Selecting products for POS #{0}", id);
            pb.Products = db.Items
                .Where(x => x.pos_id == id && x.status_id == (byte)Status.Approved)
                .Take(100)
                .ToList();
            pb.Shops = dbPos.Items.ToList();
            return View(pb);
        }

        //
        // GET: /pradmin/get?posid=100001

        public ActionResult Get()
        {
            // ID of POS must be!
            int posId = int.Parse(Request["posid"]);
            InfoFormat("Loading products for POS ID #{0}...", posId);

            ProductRepository db = new ProductRepository();
            PosRepository dbPos = new PosRepository();

            StringBuilder sbMain = new StringBuilder();
            StringBuilder sb = new StringBuilder();
            var products = db.Items
                .Where(x => x.pos_id == posId && x.status_id == (byte)Status.Approved)
                .Take(200)
                .ToList();
            InfoFormat("Got {0} products for POS ID #{1}", products == null ? "NULL" : products.Count.ToString(), posId);
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

        //
        // GET: /pradmin/edit/1234

        [Authorize]
        public ActionResult Edit(int id)
        {
            InfoFormat("Editing product with ID #{0}", id);
            ProductRepository db = new ProductRepository();
            Product model = db.GetById(id);

            return View(model);
        }

        //
        // POST: /pradmin/edit

        [Authorize]
        [HttpPost]
        public ActionResult Edit(Product model)
        {
            if( ModelState.IsValid )
            {
                ProductRepository db = new ProductRepository();
                model = db.Items.SingleOrDefault(x => x.id == model.id);
                UpdateModel<Product>(model);
                //db.Update(model);
                db.SubmitChanges();
                return RedirectToAction("Edit", new { id = model.id });
            }

            return View(model);
        }
    }
}
