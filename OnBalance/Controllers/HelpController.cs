using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace OnBalance.Controllers
{
    public class HelpController : BaseController
    {
        //
        // GET: /help/

        public ActionResult Index()
        {
            return View();
        }

        //
        // GET: /help/notfound

        public ActionResult NotFound()
        {
            return View();
        }

    }
}
