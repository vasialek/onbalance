using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using OnBalance.Models;

namespace OnBalance.Controllers
{
    public class PosController : BaseController
    {
        //
        // GET: /pos/

        public ActionResult Index()
        {
            return View();
        }

        //
        // GET: /pos/list

        [Authorize]
        public ActionResult List()
        {
            var db = new OrganizationRepository();
            return View(db.Organizations);
        }

        //
        // GET: /pos/create

        [Authorize]
        public ActionResult Create()
        {
            return View(new Organization());
        }

        //
        // POST: /pos/create

        [Authorize]
        [HttpPost]
        public ActionResult Create(Organization model)
        {
            try
            {
                model.Name = Request["Name"];
                Submit(model);
            } catch(Exception ex)
            {
                ModelState.AddModelError("", "Error saving organization!" + ex.Message);
                return View(model);
            }

            return RedirectToAction("edit", "pos", new { id = model.Id });
        }

        protected void Submit(Organization model)
        {
            if(ModelState.IsValid)
            {
                if( model.Id < 1 )
                {
                    model.CreatedAt = DateTime.Now;
                }else
                {
                    model.UpdatedAt = DateTime.Now;
                }
                new OrganizationRepository().Save(model);
            }
        
        }
    }
}
