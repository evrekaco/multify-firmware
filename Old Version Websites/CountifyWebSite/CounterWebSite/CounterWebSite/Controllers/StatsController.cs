using CounterWebSite.Models;
using DatabaseContext;
using Resources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebMatrix.WebData;

namespace CounterWebSite.Controllers
{
    [Authorize]
    public class StatsController : BaseController
    {
        //
        // GET: /Stats/

        public ActionResult Index(string VenueId)
        {
            UserProfile user = GetLoggedInUserProfile();

            if (VenueId != null)
            {
                //make sure the venue exists
                if (db.Venues.Find(VenueId) == null)
                {
                    SetStatusMessage(StatStrings.InvalidVenueId, StatusType.Error);
                    return RedirectToAction("Index", "Home");
                }

                //make sure this user manages the venue
                if (!user.ManagedVenues.Select(v => v.VenueId).Contains(VenueId))
                {
                    SetStatusMessage(StatStrings.VenueNotManaged, StatusType.Error);
                    return RedirectToAction("Index", "Home");
                }
            }
            else
            {
                //by default, select the venue with the most checkins
                VenueId = user.ManagedVenues
                    .OrderByDescending(v => v.CheckinCount)
                    .Select(v => v.VenueId)
                    .FirstOrDefault();

                //make sure the user manages at least one venue
                if (VenueId == null)
                {
                    SetStatusMessage(StatStrings.NoManagedVenues, StatusType.Error);
                    return RedirectToAction("Index", "Home");
                }
            }

            return View(new StatsModel(user, VenueId));
        }

    }
}
