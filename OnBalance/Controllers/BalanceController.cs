using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace OnBalance.Controllers
{

    public class BalanceController : Controller
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

    }
}
