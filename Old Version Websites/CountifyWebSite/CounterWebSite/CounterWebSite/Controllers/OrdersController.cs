using CounterWebSite.Models;
using DatabaseContext;
using Resources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace CounterWebSite.Controllers
{
    [Authorize(Roles = "Administrators")]
    public class OrdersController : BaseController
    {
        //
        // GET: /Order/

        public ActionResult Index()
        {
            return View(db.PreOrders.ToList());
        }

        //
        // GET: /Order/Details/5

        public ActionResult Details(int id)
        {
            PreOrder order = db.PreOrders.Find(id);
            if (order == null)
            {
                string warnMessage = "Error retrieving order details: no order exists with id " + id;
                log.Warn(warnMessage);
                SetStatusMessage(warnMessage, StatusType.Warning);
                return RedirectToAction("Index");
            }
            else
            {
                return View(order);
            }
        }

        //
        // GET: /Order/New

        [AllowAnonymous]
        public ActionResult New()
        {
            return PartialView();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult New(PreOrderModel model)
        {
            //make sure the model state is valid
            if (!ModelState.IsValid)
            {
                return PartialView(model);
            }

            //make sure a phone number OR email was provided
            if (string.IsNullOrWhiteSpace(model.PhoneNumber) &&
               string.IsNullOrWhiteSpace(model.EmailAddress))
            {
                ModelState.AddModelError(String.Empty, OrderStrings.new_emailOrPhone);
                return PartialView(model);
            }

            //create the order and save it to the databse
            PreOrder newOrder = new PreOrder()
            {
                BusinessName = model.BusinessName,
                Name = model.Name,
                EmailAddress = model.EmailAddress,
                PhoneNumber = model.PhoneNumber,
                Address = model.Address,
                Message = model.Message
            };

            try
            {
                db.PreOrders.Add(newOrder);
                db.SaveChanges();
                log.InfoFormat("Saved new preorder from {0}({1}) to database successfully", model.BusinessName, model.Name);
            }
            catch (Exception e)
            {
                log.Error("Error creating order", e);
                throw e;
            }

            //TODO: send confirmation email

            //send notification emails
            try
            {
                SubscriptionManager sm = new SubscriptionManager(db);
                //TODO: this doesn't work due to limitations in MVCMailer...find a workaround or alternative
                //sm.SendEmailsAsync(SubscriptionType.NewOrders, new PreOrderEmail(newOrder));
                sm.SendEmails(SubscriptionType.NewOrders, new PreOrderEmail(newOrder));
            }
            catch(Exception e)
            {
                Elmah.ErrorSignal.FromCurrentContext().Raise(e);
                log.Error("Error sending pre-order notification emails", e);
                //swallow error so we can still return
            }

            return PartialView("Success");
        }
    }
}
