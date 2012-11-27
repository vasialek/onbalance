using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace OnBalance.Controllers
{

    [Authorize]
    public class UseradminController : Controller
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
        public ActionResult Create(FormCollection collection)
        {
            try
            {
                // TODO: Add insert logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
        
        //
        // GET: /useradmin/edit/5
 
        public ActionResult Edit(int id)
        {
            return View();
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
