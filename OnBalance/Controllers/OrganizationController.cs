using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using OnBalance.Models;
using OnBalance.ViewModels.Organizations;
using OnBalance.Domain.Entities;
using OnBalance.Domain.Abstract;
using OnBalance.Core;

namespace OnBalance.Controllers
{
    public class OrganizationController : BaseController
    {
        private IOrganizationRepository _repository = null;

        public OrganizationController(IOrganizationRepository repository)
        {
            _repository = repository;
        }

        //
        // GET: /organization/

        public ActionResult Index()
        {
            return List(null);
        }

        //
        // GET: /organization/list

        public ViewResult List(int? id)
        {
            SetTempMessagesToViewBag();
            var companies = new List<Organization>();
            int parentId = id.HasValue ? id.Value : 0;

            if(parentId > 0)
            {
                companies = _repository.GetByParentId(parentId, true).ToList();
            } else
            {
                companies = _repository.Organizations
                    .Where(x => x.StatusId.Equals((byte)Status.Approved))
                    .OrderBy(x => x.ParentId)
                    .OrderBy(x => x.Name)
                    .ToList();
            }
            //if( id.HasValue )
            //{
            //    companies = companies.Where(x => x.ParentId == id.Value);
            //}

            return View("List", companies);
        }

        //
        // GET: /organization/create

        public ActionResult Create(int? id)
        {
            Organization model = new Organization();
            model.ParentId = id ?? 0;
            return View(model);
        }

        //
        // POST: /organization/create

        [HttpPost]
        public ActionResult Create(Organization model)
        {
            try
            {
                _repository.Save(model);
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
        // GET: /organization/edit/500002

        [Authorize]
        public ActionResult Edit(int id)
        {
            InfoFormat("User #{0} is going to edit Organization #{1}", User.Identity.Name, id);
            Organization model = _repository.Organizations.SingleOrDefault(x => x.Id == id);
            if(model == null)
            {
                WarnFormat("Non-existing organization #{0}!", id);
                return RedirectToAction("notfound", "help");
            }

            OrganizationEditViewModel viewModel = new OrganizationEditViewModel();
            viewModel.Organization = model;
            viewModel.Parent = _repository.GetById(model.ParentId);
            viewModel.Children = _repository.GetByParentId(model.Id);
            viewModel.Users = _repository.GetUsersInOrganization(model.Id);

            return View(viewModel);
        }

        //
        // POST: /organization/edit

        [Authorize]
        [HttpPost]
        public ActionResult Edit(OrganizationEditViewModel model)
        {
            try
            {
                InfoFormat("User #{0} going to update Organization #{1}", User.Identity.Name, model.Organization.Id);
                var record = _repository.GetById(model.Organization.Id);
                UpdateModel(record, "Organization");
                _repository.SubmitChanges();
                SetTempOkMessage("Organization {0} was successfully updated", model.Organization.Name);
                return RedirectToAction("list", new { parent = model.Organization.ParentId == 0 ? model.Organization.Id : model.Organization.ParentId });
            } catch(Exception ex)
            {
                Error("Error updating Organization!", ex);
                ModelState.AddModelError("", ex.Message);
            }
            return View(model);
        }

        //
        // GET: /organization/moveto/500001

        public ActionResult MoveTo(int id)
        {
            InfoFormat("User #{0} is going to move Organization #{1}", User.Identity.Name, id);
            Organization model = _repository.Organizations.SingleOrDefault(x => x.Id == id);
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
            Organization model = _repository.Organizations.SingleOrDefault(x => x.Id == id);
            if(model == null)
            {
                SetTempErrorMessage("Could not move non-existing organization #{0}!", id);
                return RedirectToAction("list");
            }

            try
            {
                // Select new parent from companies (where parent_id == 0)
                Organization parent = _repository.Companies.Single(x => x.Id == newParentId);
                UpdateModel(model);
                model.ParentId = parent.Id;
                _repository.SubmitChanges();

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
