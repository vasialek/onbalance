﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using OnBalance.Models;
using OnBalance.Domain.Abstract;
using OnBalance.Domain.Entities;

namespace OnBalance.Controllers
{
    public class PosController : BaseController
    {
        private IOrganizationRepository _organizationRepository = null;

        protected override void OnResultExecuting(ResultExecutingContext filterContext)
        {
            throw new NotSupportedException("Use Organization instead!");
            base.OnResultExecuting(filterContext);
        }
        //
        // GET: /pos/

        public ActionResult Index()
        {
            return RedirectToAction("list");
        }

        //
        // GET: /pos/list

        [Authorize]
        public ActionResult List()
        {
            // Pass messages and errors to view
            SetTempMessagesToViewBag();

            var shops = _organizationRepository.Organizations;
            if( shops == null )
            {
                throw new Exception("No POS!");
            }
            return View("List", shops.ToList());
        }

        //
        // GET: /pos/edit/500001

        [Authorize]
        public ActionResult Edit(int id)
        {
            InfoFormat("User #{0} is editing POS #{1}...", User.Identity.Name, id);
            var pos = _organizationRepository.GetById(id);
            return View(pos);
        }

        //
        // POST: /pos/edit

        [HttpPost]
        [Authorize]
        public ActionResult Edit(int id, Organization model)
        {
            InfoFormat("User #{0} is updating POS #{1}...", User.Identity.Name, id);
            model = _organizationRepository.GetById(id);
            if( model == null )
            {
                ErrorFormat("  Can't update non-existing POS #{0}!", id);
                SetTempErrorMessage("Can't update non-existing POS #{0}!", id);
                return RedirectToAction("list");
            }

            try
            {
                UpdateModel<Organization>(model);
                if(ModelState.IsValid)
                {
                    _organizationRepository.SubmitChanges();
                }
            } catch(Exception ex)
            {
                Error("  Error updating POS!", ex);
                ModelState.AddModelError("", ex.Message);
                return View(model);
            }

            SetTempOkMessage("Point Of Service is successfully updated");
            return RedirectToAction("edit", new { id = id });
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
                _organizationRepository.Save(model);
            }
        
        }
    }
}
