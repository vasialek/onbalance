using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using OnBalance.Models;
using OnBalance.Domain.Abstract;
using System.Text;

namespace OnBalance.Controllers
{
    public class TestController : BaseController
    {
        private ICategoryRepository _repository = null;

        public TestController(ICategoryRepository repository)
        {
            _repository = repository;
        }

        //
        // GET: /test/

        public ActionResult Index()
        {
            _repository.Categories.ToList();
            return Content("OK");
            StringBuilder sb = new StringBuilder();
            foreach(var item in _repository.Categories.ToList())
            {
                sb.AppendLine(item.Name);
            }
            return Content(sb.ToString());
            //InfoFormat("Test/Index...");
            //BalanceItem bi = new BalanceItem();
            //return View(bi);
        }

        //
        // POST: /test/index

        [HttpPost]
        public ActionResult Index(BalanceItem model)
        {
            return RedirectToAction("Index");
        }

    }
}
