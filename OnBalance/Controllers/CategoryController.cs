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
