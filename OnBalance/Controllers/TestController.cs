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
            var db = new BalanceItemRepository();
            db.Save(new BalanceItem { ProductName = "Test BI", Quantity = 2, InternalCode = "IA_12345", Price = 666, PosId = 1, IsNew = true });
            return Content("Done...");
        }

    }
}
