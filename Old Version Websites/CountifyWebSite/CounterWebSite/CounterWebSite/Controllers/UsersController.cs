using CounterWebSite.Models;
using DatabaseContext;
using FoursquareOAuth;
using log4net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using WebMatrix.WebData;

namespace CounterWebSite.Controllers
{
    [Authorize(Roles = "Administrators")]
    public class UsersController : BaseController
    {
        public ActionResult Index()
        {
            return View(db.UserProfiles.ToList().Select(u => new UserDetails(u)).ToList());
        }

        public ActionResult Details(int id)
        {

            UserProfile user = db.UserProfiles.Find(id);
            if (user == null)
            {
                string warnMessage = String.Format("Failed retrieving user details: The specified user (id={0}) does not exist", id);
                log.Warn(warnMessage);
                SetStatusMessage(warnMessage);
                return RedirectToAction("Index");
            }

            return View(new UserDetails(user));
        }

        public ActionResult Edit(int id)
        {
            UserProfile user = db.UserProfiles.Find(id);
            if (user == null)
            {
                string warnMessage = String.Format("Failed to edit user: The specified user (id={0}) does not exist", id);
                log.Warn(warnMessage);
                SetStatusMessage(warnMessage);
                return RedirectToAction("Index");
            }

            return View(new UserEdit(user));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(UserEdit userEdit)
        {
            //if the model is not valid redisplay the form
            if (!ModelState.IsValid)
            {
                return View(userEdit);
            }

            UserProfile user = db.UserProfiles.Find(userEdit.UserId);

            if (user == null)
            {
                string warnMessage = String.Format("The specified user (id={0}) does not exist", userEdit.UserId);
                log.Warn("Error editing user:" + warnMessage);
                SetStatusMessage(warnMessage);
                return RedirectToAction("Index");
            }

            //save changes to user
            user.UserName = userEdit.UserName;
            user.Email = userEdit.Email;
            user.PhoneNumber = userEdit.PhoneNumber;
            db.SaveChanges();
            log.Info(String.Format("User '{0}' updated successfully", user.UserName));

            SetStatusMessage("User updated successfully!", StatusType.Success);
            return RedirectToAction("Details", new { id = user.UserId });
        }

        [HttpGet]
        [ActionName("Delete")]
        public ActionResult DeleteConfirm(int id)
        {
            throw new NotImplementedException();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id)
        {
            throw new NotImplementedException();
        }

        //TODO: add role, remove role

    }
}
