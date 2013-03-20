using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using OnBalance.ViewModels.User;
using OnBalance.Models;
using OnBalance.Helpers;
using System.Security.Cryptography;
using System.Text;

namespace OnBalance.Controllers
{
    public class UserController : BaseController
    {
        //
        // GET: /user/

        [Authorize]
        public ActionResult Index()
        {
            return View();
        }

        //
        // GET: /user/dologin

        public ActionResult DoLogin()
        {
            Status status = Status.Failed;
            StringBuilder sbHash = new StringBuilder();

            string username = Request["username"] ?? "";
            string password = Request["password"] ?? "";
            // Show only on login
            string clientIp = "";

            if(username.Equals("gj") && password.Equals("123456"))
            {
                clientIp = Request.UserHostAddress;
                string hashToCompute = string.Format("{0}_{1}_{2}", username, password, clientIp);

                MD5 md5hasher = MD5CryptoServiceProvider.Create();
                byte[] ba = md5hasher.ComputeHash(Encoding.UTF8.GetBytes(hashToCompute));
                for(int i = 0; i < ba.Length; i++)
                {
                    // JS calculate MD5 hash in lower
                    sbHash.Append(ba[i].ToString("x2"));
                }
            }

            string callback = Request["callback"];
            if(string.IsNullOrEmpty(callback))
            {
                return Json(new
                {
                    Status = status,
                    Hash = sbHash.ToString(),
                    ClientIp = clientIp
                }, JsonRequestBehavior.AllowGet);
            }


            return new JsonpResult
            {
                Data = new { Status = status, Hash = sbHash.ToString(), ClientIp = clientIp },
                JsonRequestBehavior = JsonRequestBehavior.AllowGet
            };
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
                InfoFormat("Trying to log as user {0}, providing pasword length of {1}", model.Username, model.Password.Length);
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
                } else
                {
                    ModelState.AddModelError("", "Bad username and/or password!");
                    //Log.WarnFormat("Bad password for user {0}!", model.Username);
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
            dashboard.Shops = new OrganizationRepository().Items.ToList(); //.Where(x => x.UserId == User.Identity.Name).ToList();
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
