using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using OnBalance.Models;

namespace OnBalance.Controllers
{
    public class OrganizationController : BaseController
    {
        //
        // GET: /organization/

        public ActionResult Index()
        {
            return List();
        }

        //
        // GET: /organization/list

        public ActionResult List(/*int id*/)
        {
            OrganizationRepository db = new OrganizationRepository();
            var companies = db.Companies;
            int parentId = 0;
            if(int.TryParse(Request["parent"], out parentId))
            {
                companies = db.Items
                    .Where(x => x.ParentId == parentId || x.Id == parentId)
                    .OrderBy(x => x.ParentId);
            }
            //if( id.HasValue )
            //{
            //    companies = companies.Where(x => x.ParentId == id.Value);
            //}

            return View("List", companies);
        }

        //
        // GET: /organization/create

        public ActionResult Create()
        {
            Organization model = new Organization();
            return View(model);
        }

        //
        // POST: /organization/create

        [HttpPost]
        public ActionResult Create(Organization model)
        {
            try
            {
                new OrganizationRepository().Save(model);
                SetTempOkMessage("New company {0} was created", model.Name);
                return RedirectToAction("list");
            } catch( Exception ex )
            {
                //Log4cs.Log(Importance.Error, "");
                //Log4cs.Log(Importance.Debug, ex.ToString());
                ModelState.AddModelError("", ex.Message);
            }

            return View(model);
        }

        //
        // GET: /organization/moveto/500001

        public ActionResult MoveTo(int id)
        {
            InfoFormat("User #{0} is going to move Organization #{1}", User.Identity.Name, id);
            Organization model = new OrganizationRepository().Items.SingleOrDefault(x => x.Id == id);
            if(model == null)
            {
                WarnFormat("Non-existing organization #{0}!", id);
                return Content("Not found!");
            }

            return View(model);
        }

        //
        // POST: /organization/moveto/500010

        [HttpPost]
        public ActionResult MoveTo(int id, int newParentId)
        {
            OrganizationRepository db = new OrganizationRepository();
            Organization model = db.Items.SingleOrDefault(x => x.Id == id);
            if(model == null)
            {
                SetTempErrorMessage("Could not move non-existing organization #{0}!", id);
                return RedirectToAction("list");
            }

            try
            {
                // Select new parent from companies (where parent_id == 0)
                Organization parent = db.Companies.Single(x => x.Id == newParentId);
                UpdateModel(model);
                model.ParentId = parent.Id;
                db.SubmitChanges();

                SetTempOkMessage("Organization {0} was moved under company {1}", model.Name, parent.Name);
                return RedirectToAction("list");
            } catch( Exception ex )
            {
                ModelState.AddModelError("", ex.Message);
            }

            return View(model);
        }
    }
}
