using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using OnBalance.Import;

namespace OnBalance.Controllers
{
    public class LgfController : BaseController
    {
        //
        // GET: /lgf/

        public ActionResult Index()
        {
            return Content("L.G.F.");
        }

        public ActionResult List()
        {
            var lgf = new ImportLgf();
            ViewBag.UpdatedAt = lgf.GetPosUpdatedAt().ToShortDateString();
            InfoFormat("LGF was updated at {0}", ViewBag.UpdatedAt);
            lgf.GetData();
            var items = lgf.GetNewProducts().OrderByDescending(x => x.Price);
            return View(items);
        }

    }
}
