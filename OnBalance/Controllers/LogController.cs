using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using OnBalance.Models;

namespace OnBalance.Controllers
{

    [Authorize]
    public class LogController : BaseController
    {
        //
        // GET: /Log/

        public ActionResult Index()
        {
            return List(1);
        }

        public ActionResult List(int id)
        {
            var db = new ObLogRepository();
            int page = id > 0 ? id : 1;
            int perPage = 100;
            return View("List", Layout, db.GetLast( (page - 1) * perPage, perPage));
        }
    }
}
