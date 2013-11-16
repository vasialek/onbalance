using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using OnBalance.Models;
using OnBalance.ViewModels.Categories;
using OnBalance.Domain.Abstract;

namespace OnBalance.Controllers
{
    public class CategoryController : BaseController
    {
        private ICategoryRepository _repository = null;

        public int PageSize { get; set; }

        public CategoryController(ICategoryRepository repository)
        {
            _repository = repository;
            PageSize = 30;
        }

        //
        // GET: /category/

        public ActionResult Index()
        {
            var model = new PosCategoriesListViewModel();
            model.Organization = new Organization { Name = "ALL" };
            model.Categories = _repository.Categories.ToList();
            //var db = new ProductRepository();
            return View("List", Layout, model);
        }

        public ViewResult List(int? id)
        {
            var model = new PosCategoriesListViewModel();

            if(id.HasValue && id.Value > 0)
            {
                throw new NotImplementedException("");
                //model.Organization = new OrganizationRepository().Items.Single(x => x.Id == id);
                //model.Categories = _repository.GetCategoriesBy(model.Organization.Id, 0, 0, 100).ToList();
            } else
            {
                model.Organization = new Organization { Name = "ALL" };
                model.Categories = _repository.Categories.OrderBy(x => x.Name).ToList();
            }
            return View("List", Layout, model);
        }

        //
        // GET: /category/create/500001

        [Authorize]
        public ActionResult Create(int id)
        {
            var model = new PosCategoryViewModel();
            model.Organization = new OrganizationRepository().Items.Single(x => x.Id == id);
            model.Category = new Category { OrganizationId = id };
            return View(model);
        }

        //
        // POST: /category/create

        [Authorize]
        [HttpPost]
        public ActionResult Create(PosCategoryViewModel model)
        {
            try
            {
                InfoFormat("User #{0} creating category...", User.Identity.Name);
                var db = new ProductRepository();
                model.Organization = new OrganizationRepository().GetById(model.Category.OrganizationId);
                model.Category = db.Save(model.Category);
            } catch(Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return View(model);
            }

            SetTempOkMessage("Category was successfully saved");
            return RedirectToAction("list", new { id = model.Category.OrganizationId });
        }

        public ActionResult DoSave(/*int id, */CategoryStructureViewModel vm, CategoryStructure newItem)
        {
            try
            {
                var db = new CategoryStructureRepository();
                var dbProduct = new ProductRepository();
                //Category model = dbProduct.GetCategory(id);
                //string newName = Request["NewName"];
                //bool isNewApproved = false;

                InfoFormat("User #{0} updates Category #{1} to name: {2}, status: {3} ({4}), organization: #{5}", User.Identity.Name, vm.Category.Id, vm.Category.Name, vm.Category.StatusName, vm.Category.StatusId, vm.Category.OrganizationId);
                //InfoFormat("User #{0} updates Category #{1} to name: {2}, status: {3} ({4}), organization: #{5}", User.Identity.Name, model.Id, model.Name, model.StatusName, model.StatusId, model.OrganizationId);
                dbProduct.Save(vm.Category);
                //UpdateModel<Category>(model, "Category__");
                //dbProduct.SubmitChanges();

                //if(CategoryStructure != null)
                //{
                Info("Updating category structure...");
                foreach(var item in vm.CategoryStructure)
                {
                    InfoFormat("  Category structure: #{0, -8}. {1}", item.Id, item.FieldName);
                    db.Update(item);
                }
                //}

                db.Add(newItem);
/*
                if(string.IsNullOrEmpty(newName) == false)
                {
                    bool.TryParse(Request["NewStatus"], out isNewApproved);
                    CategoryStructure cs = new CategoryStructure();
                    cs.FieldName = newName;
                    cs.StatusId = isNewApproved ? (byte)Status.Approved : (byte)Status.Deleted;
                    cs.CategoryId = vm.Category.Id;
                    db.Add(cs);
                }
*/
                db.SubmitChanges();

                return PartialView("CategoryStructure", vm.Category);
            } catch(Exception ex)
            {
                Error("Error updating Category structure!", ex);
                throw ex;
            }
        }

        //
        // GET: /category/edit/10001

        [Authorize]
        public ActionResult Edit(int id)
        {
            CategoryStructureViewModel vm = new CategoryStructureViewModel();
            //var model = new Category();
            var db = new ProductRepository();
            vm.Category = db.GetCategory(id);
            if(vm.Category == null)
            {
                ErrorFormat("User #{0} tries to edit non-existing category #{1}!", User.Identity.Name, id);
                return RedirectToAction("notfound", "help");
            }

            //model.Organization = new OrganizationRepository().Items.Single(x => x.Id == model.Category.id);
            return View(vm);
        }

        //
        // POST: /category/edit

        [HttpPost]
        [Authorize]
        public ActionResult Edit(CategoryStructureViewModel vm)
        {
            var db = new ProductRepository();
            vm.Category = db.GetCategory(vm.Category.Id);
            if(vm.Category == null)
            {
                ErrorFormat("User #{0} tries to update non-existing category #{1}!", User.Identity.Name, vm.Category.Id);
                return RedirectToAction("notfound", "help");
            }

            db.Save(vm.Category);
            SetTempOkMessage("Category {0} was successfully updated", vm.Category.Name);

            return RedirectToAction("edit", new { id = vm.Category.Id });
        }

        //
        // GET: /category/reset/1015

        public ActionResult Reset(int id)
        {
            // TODO: ViewModel
            InfoFormat("User #{0} going to reset Category #{1} structure...", User.Identity.Name, id);
            return View(new ProductRepository().GetCategory(id));
        }

        //
        // POST: /category/reset

        [HttpPost]
        public ActionResult Reset(int id, string confirm)
        {
            var db = new ProductRepository();
            WarnFormat("User #{0} reset Category #{1} structure!", User.Identity.Name, id);
            // TODO: delete products under this category
            var c = db.GetCategory(id);
            c.CategoryTypeId = new CategoryTypeRepository().Items.OrderBy(x => x.Id).First().Id;
            db.SubmitChanges();

            return RedirectToAction("edit", new { id = id });
        }
    }
}
