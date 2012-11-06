using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using OnBalance.ViewModels.User;
using OnBalance.Models;

namespace OnBalance.Controllers
{
    public class UserController : Controller
    {
        //
        // GET: /user/

        [Authorize]
        public ActionResult Index()
        {
            return View();
        }

        //
        // GET: /user/login

        public ActionResult Login()
        {
            return View("Login", "_LayoutLogin");
        }

        //
        // POST: /user/login

        [HttpPost]
        public ActionResult Login(LoginViewModel model)
        {
            if(ModelState.IsValid)
            {
                FormsAuthentication.SetAuthCookie("Test", false);
                return RedirectToAction("dashboard");
            }

            return View(model);
        }

        //
        // GET: /user/dashboard

        [Authorize]
        public ActionResult Dashboard()
        {
            var dashboard = new DashboardViewModel();
            dashboard.Shops = new ShopRepository().Shops.ToList(); //.Where(x => x.UserId == User.Identity.Name).ToList();
            dashboard.Imports = new List<Task>()
            {
                new Task{ Type = Task.TypeId.Import, Status = Status.Pending }
                , new Task{ Type = Task.TypeId.Import, Status = Status.Pending }
            };
            dashboard.Exports = new List<Task>
            {
                
            };
            return View(dashboard);
        }

    }
}
