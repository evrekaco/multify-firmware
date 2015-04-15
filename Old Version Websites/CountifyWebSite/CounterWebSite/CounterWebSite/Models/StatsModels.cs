using DatabaseContext;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CounterWebSite.Models
{
    public class StatsModel
    {
        public string venueId { get; set; }

        public string venueName { get; set; }

        public IEnumerable<SelectListItem> managedVenues { get; set; }

        public List<VenueCheckin> Checkins { get; set; }

        public int TotalCheckins { get; set; }

        public string TotalCheckinsString { get; set; }

        public int UntrackedCheckins { get; set; }

        public string ChartDataString { get; set; }

        public StatsModel() { }

        public StatsModel(UserProfile User, string VenueId)
        {
            //get a list of all venues managed by this user
            this.managedVenues = User.ManagedVenues.Select(v => new SelectListItem()
                {
                    Text = v.Name,
                    Value = v.VenueId,
                    Selected = v.VenueId == venueId
                });

            //get the selected venue
            Venue Venue = User.ManagedVenues.First(v => v.VenueId == VenueId);
            this.venueId = Venue.VenueId;
            this.venueName = Venue.Name;

            this.Checkins = Venue.PushCheckins
                .OrderByDescending(c => c.CheckinTime)
                //.Take(20) //TODO: support this
                .ToList()
                .Select(c => new VenueCheckin(c))
                .ToList();

            this.TotalCheckins = Venue.CheckinCount;
            this.UntrackedCheckins = Venue.CheckinCount - Venue.PushCheckins.Count;

            //make a string padded with zeroes
            this.TotalCheckinsString = Venue.CheckinCount.ToString();
            this.TotalCheckinsString = new string('0', 6 - this.TotalCheckinsString.Length) + this.TotalCheckinsString;

            ChartDataString = "[[";

            var checkinData = Venue.PushCheckins
                .Select(c => new { c.UserName, c.CheckinTime })
                .OrderBy(c => c.CheckinTime)
                .ToList();

            for(int i = 0 ; i < checkinData.Count; i++)
            {
                ChartDataString += String.Format("[new Date({0}),{1}]",
                    checkinData[i].CheckinTime.Subtract(new DateTime(1970, 1,1)).TotalMilliseconds,
                    Venue.CheckinCount - Venue.PushCheckins.Count + i);

                if(i != checkinData.Count - 1)
                {
                    ChartDataString += ",";
                }
            }

            ChartDataString += "]]";
        }
    }
}