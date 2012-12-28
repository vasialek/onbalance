using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using OnBalance.ViewModels.User;

namespace OnBalance.Controllers
{

    [Authorize]
    public class UseradminController : BaseController
    {
        //
        // GET: /useradmin/

        public ActionResult Index()
        {
            var users = Membership.GetAllUsers().Cast<MembershipUser>().ToList();
            return View(users);
        }

        //
        // GET: /useradmin/details/5

        public ActionResult Details(int id)
        {
            return View();
        }

        //
        // GET: /useradmin/create

        public ActionResult Create()
        {
            return View();
        } 

        //
        // POST: /useradmin/create

        [HttpPost]
        public ActionResult Create(UserViewModel model)
        {
            try
            {
                if(ModelState.IsValid)
                {
                    MembershipUser user = Membership.CreateUser(model.Name, model.Password, model.Email);
                    return RedirectToAction("edit", new { id = user.UserName });
                }
            }
            catch
            {
                
            }
            return View(model);
        }
        
        //
        // GET: /useradmin/edit/test
 
        public ActionResult Edit(string id)
        {
            MembershipUser user = Membership.GetUser(id);
            UserViewModel model = new UserViewModel();
            model.Name = user.UserName;
            model.Email = user.Email;
            model.IsApproved = user.IsApproved;
            // Hide password
            model.Password = "";
            return View(model);
        }

        //
        // POST: /useradmin/edit/5

        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add update logic here
 
                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        //
        // GET: /useradmin/delete/5
 
        public ActionResult Delete(int id)
        {
            return View();
        }

        //
        // POST: /useradmin/delete/5

        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here
 
                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
}
