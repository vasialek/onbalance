using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using OnBalance.Models;
using OnBalance.ViewModels.Categories;

namespace OnBalance.Controllers
{
    public class CategoryController : BaseController
    {
        //
        // GET: /category/

        public ActionResult Index()
        {
            var db = new ProductRepository();
            return View("List", Layout, db.Categories);
        }

        public ActionResult List()
        {
            var db = new ProductRepository();
            return View("List", Layout, db.Categories);
        }

        public ActionResult DoSave(int id, CategoryStructureViewModel vm)
        {
            try
            {
                var db = new CategoryStructureRepository();
                var dbProduct = new ProductRepository();
                //Category model = dbProduct.GetCategory(id);
                string newName = Request["NewName"];
                bool isNewApproved = false;

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

                if(string.IsNullOrEmpty(newName) == false)
                {
                    bool.TryParse(Request["NewStatus"], out isNewApproved);
                    CategoryStructure cs = new CategoryStructure();
                    cs.FieldName = newName;
                    cs.StatusId = isNewApproved ? (byte)Status.Approved : (byte)Status.Deleted;
                    cs.CategoryId = vm.Category.Id;
                    db.Add(cs);
                }

                db.SubmitChanges();

                return PartialView("CategoryStructure", vm.Category);
            } catch(Exception ex)
            {
                Error("Error updating Category structure!", ex);
                throw ex;
            }
        }

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
    }
}
