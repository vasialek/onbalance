using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using OnBalance.Models;

namespace OnBalance.Controllers
{
    public class TestController : BaseController
    {
        //
        // GET: /test/

        public ActionResult Index()
        {
            Log.InfoFormat("Test/Index...");
            BalanceItem bi = new BalanceItem();
            return View(bi);
        }

        //
        // POST: /test/index

        [HttpPost]
        public ActionResult Index(BalanceItem model)
        {
            return RedirectToAction("Index");
        }

    }
}
