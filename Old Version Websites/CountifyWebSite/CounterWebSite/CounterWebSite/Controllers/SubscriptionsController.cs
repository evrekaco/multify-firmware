using CounterWebSite.Models;
using DatabaseContext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CounterWebSite.Controllers
{
    [Authorize(Roles = "Administrators")]
    public class SubscriptionsController : BaseController
    {
        //
        // GET: /Subscriptions/

        public ActionResult Index()
        {
            return View(db.Subscriptions.ToList().Select(s => new SubscriptionDetails(s)));
        }

        //
        // GET: /Subscriptions/Details/5

        public ActionResult Details(int id)
        {
            Subscription sub = db.Subscriptions.Find(id);
            if(sub == null)
            {
                string warnMessage = String.Format("The subscription id '{0}' is invalid", id);
                SetStatusMessage(warnMessage, StatusType.Warning);
                log.Warn("Error getting subscription details:" + warnMessage);
                return RedirectToAction("Index");
            }
            else
            {
                return View(new SubscriptionDetails(sub));
            }
        }

        //
        // GET: /Subscriptions/Notify

        [AllowAnonymous]
        public ActionResult Notify()
        {
            return PartialView();
        }

        //
        // POST: /Subscriptions/Notify

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult Notify(NotifySubscriptionModel model)
        {
            if (!ModelState.IsValid)
            {
                return PartialView(model);
            }

            try
            {
                SubscriptionManager _sm = new SubscriptionManager(db);
                ViewBag.subscriptionExists = !_sm.CreateSubscription(SubscriptionType.ProductUpdates, model.Email);

                //return success
                return PartialView("Success");
            }
            catch (Exception e)
            {
                log.Error("Error adding notification subscription", e);
                throw e;
            }
        }

        [HttpGet]
        public ActionResult Update()
        {
            var Subscriptions = new List<EditSubscription>(db.SubscriptionTypes.Count());

            if(User.IsInRole("Administrators"))
            {
                Subscriptions = db.SubscriptionTypes
                    .ToList()
                    .Select(s => new EditSubscription(s))
                    .ToList();
            }
            else
            {
                Subscriptions = db.SubscriptionTypes
                    .Where(s => s.AdministratorOnly == false)
                    .Select(s => new EditSubscription(s))
                    .ToList();
            }

            return PartialView(Subscriptions);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Update(List<EditSubscription> Model)
        {
            if(!ModelState.IsValid)
            {
                return PartialView(Model);
            }

            SubscriptionManager _sm = new SubscriptionManager(db);
            bool result = _sm.UpdateSubscriptions(Model
                .Where(s => s.Subscribed)
                .Select(s => s.TypeId).ToList());

            if(result == false)
            {
                SetAjaxStatusMessage("We're sorry, there was an error while updating your subscriptions. Please try agian later.", StatusType.Error);
                return RedirectToAction("Update");
            }

            SetAjaxStatusMessage("Updated subscriptions successfully", StatusType.Success);
            return RedirectToAction("Update");
        }
    }
}
