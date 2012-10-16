using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace OnBalance.Controllers
{
    public class HomeController : Controller
    {
        //
        // GET: /home/

        [Authorize]
        public ActionResult Index()
        {
            return View();
        }

    }
}
