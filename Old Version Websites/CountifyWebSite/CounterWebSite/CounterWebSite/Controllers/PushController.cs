using DatabaseContext;
using FoursquareOAuth;
using FoursquareOAuth.ApiClasses;
using Newtonsoft.Json.Linq;
using System;
using System.Linq;
using System.IO;
using System.Net;
using System.Web;
using System.Collections.Generic;
using Newtonsoft.Json;
using log4net;
using System.Web.Mvc;
using CounterWebSite.Filters;
using System.Runtime.Caching;
using System.Threading.Tasks;

namespace CounterWebSite.Controllers
{
    //[RemoteRequireHttps]
    public class PushController : BaseController
    {
        private static MemoryCache _cache = MemoryCache.Default;
        private static readonly int cacheExpiryMinutes = 60;

        private static Dictionary<string, int> testingValues = new Dictionary<string, int>();

        public PushController() : base(false) { }

        //you can send a test push request by executing these commands in windows powershell:
        //NOTE: you will need to change the checkin id in the body for each request otherwise the request will fail
        /*
            [System.Net.ServicePointManager]::ServerCertificateValidationCallback = {$true}
            $body ="checkin=%7B%22id%22%3A%22536b6b50498ef63a5f43c394%22%2C%22createdAt%22%3A1399548752%2C%22type%22%3A%22checkin%22%2C%22timeZoneOffset%22%3A0%2C%22user%22%3A%7B%22id%22%3A%221%22%2C%22firstName%22%3A%22Jimmy%22%2C%22lastName%22%3A%22Foursquare%22%2C%22gender%22%3A%22male%22%2C%22photo%22%3A%7B%22prefix%22%3A%22https%3A%5C%2F%5C%2Firs1.4sqi.net%5C%2Fimg%5C%2Fuser%5C%2F%22%2C%22suffix%22%3A%22%5C%2FS54EHRPJAHQK0VHP.jpg%22%7D%2C%22tips%22%3A%7B%22count%22%3A0%7D%2C%22homeCity%22%3A%22New%20York%2C%20NY%22%2C%22bio%22%3A%22%22%2C%22contact%22%3A%7B%7D%7D%2C%22venue%22%3A%7B%22id%22%3A%224ef0e7cf7beb5932d5bdeb4e%22%2C%22name%22%3A%22Foursquare%20HQ%22%2C%22contact%22%3A%7B%22twitter%22%3A%22foursquare%22%7D%2C%22location%22%3A%7B%22address%22%3A%22568%20Broadway%2C%20Fl%2010%22%2C%22crossStreet%22%3A%22at%20Prince%22%2C%22lat%22%3A40.72412842453194%2C%22lng%22%3A-73.99726510047911%2C%22postalCode%22%3A%2210012%22%2C%22cc%22%3A%22US%22%2C%22city%22%3A%22New%20York%22%2C%22state%22%3A%22NY%22%2C%22country%22%3A%22United%20States%22%7D%2C%22categories%22%3A%5B%7B%22id%22%3A%224bf58dd8d48988d125941735%22%2C%22name%22%3A%22Tech%20Startup%22%2C%22pluralName%22%3A%22Tech%20Startups%22%2C%22shortName%22%3A%22Tech%20Startup%22%2C%22icon%22%3A%7B%22prefix%22%3A%22https%3A%5C%2F%5C%2Fss1.4sqi.net%5C%2Fimg%5C%2Fcategories_v2%5C%2Fshops%5C%2Ftechnology_%22%2C%22suffix%22%3A%22.png%22%7D%2C%22primary%22%3Atrue%7D%5D%2C%22verified%22%3Atrue%2C%22stats%22%3A%7B%22checkinsCount%22%3A51021%2C%22usersCount%22%3A9273%2C%22tipCount%22%3A198%7D%2C%22url%22%3A%22https%3A%5C%2F%5C%2Ffoursquare.com%22%2C%22storeId%22%3A%22HQHQHQHQ%22%7D%7D&user=%7B%22id%22%3A%221%22%2C%22firstName%22%3A%22Jimmy%22%2C%22lastName%22%3A%22Foursquare%22%2C%22gender%22%3A%22male%22%2C%22photo%22%3A%22https%3A%5C%2F%5C%2Fis0.4sqi.net%5C%2Fuserpix_thumbs%5C%2FS54EHRPJAHQK0VHP.jpg%22%2C%22tips%22%3A%7B%22count%22%3A0%7D%2C%22homeCity%22%3A%22New%20York%2C%20NY%22%2C%22bio%22%3A%22%22%2C%22contact%22%3A%7B%7D%7D&secret=MERAGSW5GYALB4L3CLKWUQTQEU1DZM2GD4EZ4J1ARDERYW2O"
            Invoke-WebRequest -Method POST -Uri https://countify.co/Push/ReceivePushNotification -Body $body 
        */

        [HttpPost]
        public ActionResult ReceivePushNotification(string user, string secret,
            string checkin,
            string like,
            string tip,
            string photo)
        {
            //make sure a "user" and "secret" property were given
            if (user == null)
            {
                throw new ArgumentException("Missing 'user' parameter");
            }
            if (secret == null)
            {
                throw new ArgumentException("Missing 'secret' parameter");
            }

            //make sure the push secret is correct
            if (secret != _FSqApi.PushSecret)
            {
                log.Warn("Push notification recieved with incorrect secret");
                return new HttpStatusCodeResult(HttpStatusCode.Forbidden);
            }

            //TODO: make sure the request is coming from a verified foursquare IP address (199.38.176.0/22)
            //      see "https://developer.foursquare.com/overview/realtime#sources"

            try
            {
                if (checkin != null)
                {
                    return HandleCheckin(JsonConvert.DeserializeObject<FSqCheckin>(checkin));
                }
                else if (like != null)
                {
                    return HandleLike(user, secret, like);
                }
                else if (tip != null)
                {
                    return HandleTip(user, secret, tip);
                }
                else if (photo != null)
                {
                    return HandlePhoto(user, secret, photo);
                }
                else
                {
                    return new HttpStatusCodeResult(HttpStatusCode.NotFound);
                }
            }
            catch (Exception e)
            {
                log.Error("Error handling push notification", e);
                throw e;
            }
        }

        public ActionResult GetCheckinCount(string id, string pushSecret)
        {
            //veryify the push secret
            //TODO: use something else for authentication
            if (pushSecret != _FSqApi.PushSecret)
            {
                log.WarnFormat("Attempt made from '{0}' to retrieve checkin count for venue id '{1}' whithout correct push secret.",
                    Request.ServerVariables["REMOTE_ADDR"], id);
                return new HttpStatusCodeResult(HttpStatusCode.Forbidden);
            }

            int checkinCount = getVenueCheckins(id);

            //return checkin count padded with zeroes
            string counterString = checkinCount.ToString();
            
            if ( testingValues.ContainsKey(id) )        // Add a flag character to determine if it s a test push or not
            {
                counterString = 't' + new string('0', 6 - counterString.Length) + counterString;
            }
            else
            {
                counterString = 'r' + new string('0', 6 - counterString.Length) + counterString;
            }
            
            //counterString = new string('0', 6 - counterString.Length) + counterString; // Oldie string creator
            return Content(counterString, "text/plain");  // counterString starts with 't' or 'r' , meaning test or real respectively.
        }

        [NonAction]
        private ActionResult HandleCheckin(FSqCheckin checkin)
        {
            string venueId = checkin.venue.id;
            int venueCheckins = checkin.venue.stats.checkinsCount;

            //cache checkin count
            _cache.Set(getCacheKey(venueId), venueCheckins, DateTimeOffset.MaxValue);

            using (base.db = new CounterDbContext())
            {
                Venue venue = db.Venues.Find(venueId);
                if (venue == null)
                {
                    //create a new venue record
                    venue = new Venue()
                    {
                        VenueId = venueId,
                        Name = checkin.venue.name,
                        CheckinCount = venueCheckins,
                        lastUpdated = DateTime.Now,
                        PushCheckins = new List<PushCheckin>(1) { new PushCheckin(checkin) }
                    };
                    db.Venues.Add(venue);
                    log.Info(String.Format("Created new venue {0} with id {1}", venue.Name, venue.VenueId));
                }
                else
                {
                    //save checkin to database
                    venue.PushCheckins.Add(new PushCheckin(checkin));

                    //check if our our current count makes sense
                    if (venue.CheckinCount >= venueCheckins)
                    {
                        log.Warn(String.Format("Received checkin update of {0} for '{1}', which is more than the stored value of '{2}'", venueCheckins, venue.Name, venue.CheckinCount));
                    }

                    //update checkin count
                    venue.CheckinCount = venueCheckins;
                    venue.lastUpdated = DateTime.Now;
                    log.Info(String.Format("Updated checkin count for {0}: {1}", venue.Name, venue.CheckinCount));
                }

                db.SaveChanges();
            }

            //TODO: use long-polling to send update to arduino board
            //SendCounterUpdate(venueId, venueCheckins);

            return new HttpStatusCodeResult(HttpStatusCode.OK);
        }

        [Authorize]
        public PartialViewResult Test(string id)
        {
            ViewBag.started = testingValues.ContainsKey(id);
            ViewBag.value = ViewBag.started ? testingValues[id].ToString() : "000000";  //TODO: pad with zeros
            return PartialView((object)id);
        }

        [HttpPost]
        [Authorize]
        public HttpStatusCodeResult Test(string id, int value, bool start)
        {
            //make sure user manages this venue
            using (base.db = new CounterDbContext())
            {
                if (!GetLoggedInUserProfile().ManagedVenues.Select(v => v.VenueId).Contains(id))
                {
                    log.WarnFormat("{0} tried to access test console for venue {1}, but is not a manager.", User.Identity.Name, id);
                    return new HttpStatusCodeResult(HttpStatusCode.Forbidden, "The current logged in user is not a manager for venue" + id);
                }
            }

            if (start)
            {
                if (value > 999999 || value < 0)
                {
                    throw new ArgumentException("Value must be within the range [0, 999999]", "value");
                }

                testingValues[id] = value;
                //TODO: add an expiry time for this value in case the user forgets to stop testing
            }
            else
            {
                testingValues.Remove(id);
            }

            return new HttpStatusCodeResult(HttpStatusCode.OK);
        }

        [NonAction]
        private ActionResult HandleLike(string user, string secret, string p)
        {
            return new HttpStatusCodeResult(HttpStatusCode.NotImplemented);
        }

        [NonAction]
        private ActionResult HandleTip(string user, string secret, string p)
        {
            return new HttpStatusCodeResult(HttpStatusCode.NotImplemented);
        }

        [NonAction]
        private ActionResult HandlePhoto(string user, string secret, string p)
        {
            return new HttpStatusCodeResult(HttpStatusCode.NotImplemented);
        }

        #region Helpers

        private string getCacheKey(string venueId)
        {
            return "checkins_" + venueId;
        }

        private int getVenueCheckins(string venueId)
        {
            if (testingValues.ContainsKey(venueId))
            {
                //return the temporary testing value
                return testingValues[venueId];
            }
            else if (_cache.Contains(getCacheKey(venueId)))
            {
                //return immediately if the value is in the cache
                return (int)_cache[getCacheKey(venueId)];
            }
            else
            {
                //get the venue from the database
                Venue venue;
                using (base.db = new CounterDbContext())
                {
                    venue = db.Venues.Find(venueId);
                }

                //make sure the venue exists
                if (venue == null)
                {
                    log.Error("GetCheckinCount request made with invalid venue id: " + venueId);
                    throw new KeyNotFoundException("No venue exists with ID " + venueId);
                }

                //cache the checkin count
                _cache.Set(getCacheKey(venue.VenueId), venue.CheckinCount, DateTimeOffset.Now.AddMinutes(cacheExpiryMinutes));
                return venue.CheckinCount;
            }
        }

        #endregion
    }
}