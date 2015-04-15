using CounterWebSite.Models;
using DatabaseContext;
using FoursquareOAuth;
using FoursquareOAuth.ApiClasses;
using log4net;
using Microsoft.Web.WebPages.OAuth;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading;
using System.Web;
using System.Web.Mvc;
using WebMatrix.WebData;

namespace CounterWebSite.Controllers
{
    [Authorize(Roles = "Administrators")]
    public class VenuesController : BaseController
    {
        //
        // GET: /Venue/

        public ActionResult Index()
        {
            return View(db.Venues.ToList().Select(v => new VenueDetails(v)));
        }

        //
        // GET: /Venue/Details/5

        public ActionResult Details(string id)
        {
            Venue venue = db.Venues.Find(id);

            if (venue == null)
            {
                string warnMessage = String.Format("There was no venue found with the id {0}.", id);
                log.Warn(warnMessage);
                SetStatusMessage(warnMessage);
                RedirectToAction("Index");
            }

            return View(new VenueDetails(venue));
        }

        public ActionResult UpdateCheckins(string id)
        {
            Venue venue = db.Venues.Find(id);

            //make sure the venue id is valid
            if (venue == null)
            {
                string warnMessage = String.Format("Failed to update venue checkin count: there was no venue found with the id {0}.", id);
                log.Warn("Error updating venue checkins:" + warnMessage);
                SetStatusMessage(warnMessage);
                RedirectToAction("Index");
            }

            int checkins = _FSqApi.getVenueCheckins(GetLoggedInUserProfile().AccessToken, id);

            //update database record
            if (checkins > venue.CheckinCount)
            {
                log.Warn(String.Format("Updating venue checkins for '{0}' reduced the checkin count", venue.Name));
            }
            venue.CheckinCount = checkins;
            db.SaveChanges();
            log.Info(string.Format("Updated checkins for venue '{0}': {1}", venue.Name, venue.CheckinCount));

            SetStatusMessage("Successfully updated checkin count.", StatusType.Success);
            return RedirectToAction("Details", new { id = venue.VenueId });
        }
    }
}
