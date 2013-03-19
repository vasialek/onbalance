using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using OnBalance.Models;

namespace OnBalance.Controllers
{

    public class ParamController : BaseController
    {

        //
        // GET: /param/

        public ActionResult Index()
        {
            return View(new ParamRepository().Items);
        }

        //
        // GET: /param/edit/1

        public ActionResult Edit(int id)
        {
            var model = new ParamRepository().Items.Single(x => x.Id == id);

            return View(model);
        }

    }
}
