using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using OnBalance.ViewModels.User;
using OnBalance.Models;
using System.ComponentModel.DataAnnotations;
using OnBalance.Core;

namespace OnBalance.Controllers
{

    [Authorize]
    public class UseradminController : BaseController
    {
        //
        // GET: /useradmin/

        public ActionResult Index()
        {
            // Messages and errors
            SetTempMessagesToViewBag();

            var users = Membership.GetAllUsers().Cast<MembershipUser>().ToList();
            return View(users);
        }

        //
        // GET: /useradmin/password/user

        [Authorize(Roles = "Administrator")]
        public ActionResult Password(string id)
        {
            InfoFormat("User #{0} is going to change password for user #{1}...", User.Identity.Name, id);
            MembershipUser user = Membership.GetUser(id);
            if(user == null)
            {
                WarnFormat("Non-existing user #{0}!", id);
                return new HttpNotFoundResult();
            }

            return View(new ChangePassword { UserName = user.UserName });
        }

        //
        // POST: /useradmin/password/user

        [Authorize(Roles = "Administrator")]
        [HttpPost]
        public ActionResult Password(string id, ChangePassword model)
        {
            InfoFormat("User #{0} is changing password for user #{1}...", User.Identity.Name, id);
            MembershipUser user = Membership.GetUser(id);
            if(user == null)
            {
                WarnFormat("Non-existing user #{0}!", id);
                return new HttpNotFoundResult();
            }

            try
            {
                string reset = user.ResetPassword();
                user.ChangePassword(reset, model.NewPassword);
                SetTempOkMessage("Password for user {0} was changed", model.UserName);
                return RedirectToAction("index");
            }catch(MembershipPasswordException mpex)
            {
                Error("Password exception, while changing password for user #" + id, mpex);
                if(mpex.Message.Contains("locked"))
                {
                    user.UnlockUser();
                    SetTempErrorMessage("User {0} was locked, tried to unlock it. Please try changing password again.", model.UserName);
                    return RedirectToAction("index");
                }
                ModelState.AddModelError("", mpex.Message);
            }catch(Exception ex)
            {
                Error("Error changing password for user #" + id, ex);
                ModelState.AddModelError("", ex.Message);
            }

            return View(new ChangePassword { UserName = user.UserName });
        }

        //
        // GET: /useradmin/unlock/user

        [Authorize(Roles = "Administrator")]
        public ActionResult Unlock(string id)
        {
            InfoFormat("User #{0} is going to unlock user #{1}...", User.Identity.Name, id);
            MembershipUser user = Membership.GetUser(id);
            if(user == null)
            {
                WarnFormat("Non-existing user #{0}!", id);
                return new HttpNotFoundResult();
            }

            return View(user);
        }

        //
        // POST: /useradmin/unlock/user

        [Authorize(Roles = "Administrator")]
        [HttpPost]
        public ActionResult Unlock(string id, bool confirm)
        {
            InfoFormat("User #{0} is unlocking user #{1} (is cofirm checkbox checked {2})...", User.Identity.Name, id, confirm);
            MembershipUser user = Membership.GetUser(id);
            if(user == null)
            {
                WarnFormat("Non-existing user #{0}!", id);
                return new HttpNotFoundResult();
            }

            if(confirm)
            {
                user.UnlockUser();
                SetTempOkMessage("User {0} was successfully unlocked", id);
                return RedirectToAction("index");
            }

            ModelState.AddModelError("confirm", "Please confirm unlocking!");
            return View(user);
        }

        //
        // GET: /useradmin/roles/user

        [Authorize(Roles = "Administrator")]
        public ActionResult Roles(string id)
        {
            InfoFormat("User #{0} is looking for user #{1} roles...", User.Identity.Name, id);
            MembershipUser model = Membership.GetUser(id);
            if(model == null)
            {
                WarnFormat("Non-existing user #{0}!", id);
                return new HttpNotFoundResult();
            }

            return View(model);
        }

        //
        // POST: /useradmin/dotogglerole/user

        [Authorize(Roles = "Administrator")]
        [HttpPost]
        public ActionResult DoToggleRole(string id)
        {
            Status s = Status.Failed;

            try
            {
                InfoFormat("Got request to toggle user #{0} role '{1}'", id, Request["role"]);
                string rolename = Request["role"];
                bool isAddRole = bool.Parse(Request["activate"]);
                if(isAddRole)
                {
                    System.Web.Security.Roles.AddUserToRole(id, rolename);
                } else
                {
                    System.Web.Security.Roles.RemoveUserFromRole(id, rolename);
                }
                s = Status.Approved;
            } catch(Exception ex)
            {
                Error("Error toggling user role!", ex);
                s = Status.Failed;
            }

            return Json(new { Status = s });
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
        // GET: /useradmin/createinpos/500002

        [Authorize(Roles = "PosAdministrator")]
        public ActionResult CreateInPos(int id)
        {
            var model = new UserCreate();
            model.OrganizationId = id;
            return View(model);
        }

        [Authorize(Roles = "PosAdministrator")]
        [HttpPost]
        public ActionResult CreateInPos(UserCreate model)
        {
            try
            {
                MembershipCreateStatus status;
                Membership.CreateUser(model.UserName, model.Password, model.Email, null, null, true, out status);
                if( status == MembershipCreateStatus.Success )
                {
                    InfoFormat("User {0} was created, going to add to POS #{1}", model.UserName, model.OrganizationId);
                    new UserOrganizationRepository().AddUserToOrganization(model.UserName, model.OrganizationId);
                    SetTempOkMessage("User {0} was successfully created adn added to POS", model.UserName);
                    return RedirectToAction("edit", "organization", new { id = model.OrganizationId });
                }
            } catch(Exception ex)
            {
                Error("Error creating user in POS", ex);
                ModelState.AddModelError("", ex.Message);
            }

            return View(model);
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
