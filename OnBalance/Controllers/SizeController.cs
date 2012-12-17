using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using OnBalance.Models;
using OnBalance.ViewModels;

namespace OnBalance.Controllers
{
    public class SizeController : Controller
    {
        //
        // GET: /size/

        public ActionResult Index()
        {
            SizeConvertorRepository db = new SizeConvertorRepository();
            return RedirectToAction("edit", new { id = db.GetCategories().First().Id });
        }

        //
        // GET: /size/edit

        public ActionResult Edit(int id)
        {
            SizeConvertorRepository db = new SizeConvertorRepository();
            SizeConvertorViewModel szViewModel = new SizeConvertorViewModel();
            szViewModel.Categories = db.GetCategories();
            szViewModel.SelectedCategory = szViewModel.Categories.FirstOrDefault(x => x.Id == id);
            szViewModel.Sizes = db.GetAll(id).OrderBy(x => x.euro_size).ToList();
            return View("edit", szViewModel);
        }

        //
        // POST: /size/edit

        [HttpPost]
        public ActionResult Edit(List<SizeConvertor> model)
        {
            if( ModelState.IsValid )
            {
                SizeConvertorRepository db = new SizeConvertorRepository();

                db.Update(model);

                SizeConvertor newSize = new SizeConvertor();
                newSize.category_id = int.Parse(Request["new[CategoryId]"]);
                newSize.euro_size = Request["new[EuroSize]"];
                newSize.uk_size = Request["new[UkSize]"];
                newSize.us_size = Request["new[UsSize]"];

                db.Insert(newSize);
            }
            return RedirectToAction("edit");
        }

        //
        // GET: /size/getlist

        //public ActionResult GetList()
        //{
        //    SizeConvertorRepository db = new SizeConvertorRepository();
        //    var model = db.GetAll().OrderBy(x => x.EuroSize).ToList();
        //    if(!string.IsNullOrEmpty(Request["add"]))
        //    {
        //        model.Add(new SizeConvertor { EuroSize = "e", UkSize = "k", UsSize = "s" });
        //    }
        //    return View(model);
        //}
    }
}
