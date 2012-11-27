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
        public ActionResult Login(LoginViewModel model, string returnUrl)
        {
            if(ModelState.IsValid)
            {
                if(Membership.ValidateUser(model.Username, model.Password))
                {
                    FormsAuthentication.SetAuthCookie(model.Username, false);
                    if(Url.IsLocalUrl(returnUrl) && returnUrl.Length > 1 && returnUrl.StartsWith("/") && !returnUrl.StartsWith("//") && !returnUrl.StartsWith("/\\"))
                    {
                        return Redirect(returnUrl);
                    } else
                    {
                        return RedirectToAction("dashboard", "user");
                    }
                }
            }

            return View(model);
        }

        //
        // GET: /user/logout

        [Authorize]
        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();
            return Redirect("/");
        }

        //
        // GET: /user/dashboard

        [Authorize]
        public ActionResult Dashboard()
        {
            var dashboard = new DashboardViewModel();
            dashboard.Shops = new PosRepository().Items.ToList(); //.Where(x => x.UserId == User.Identity.Name).ToList();
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
