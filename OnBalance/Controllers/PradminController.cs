using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

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
            return View();
        }
    }
}
