using DatabaseContext;
using Resources;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CounterWebSite.Models
{
    public class VenueDetails
    {
        [HiddenInput(DisplayValue = false)]
        public string VenueId { get; set; }

        public string Name { get; set; }

        [Display(Name = "# of Checkins")]
        public int Checkins { get; set; }

        [Display(Name = "Last Update")]
        [DataType(DataType.DateTime)]
        public DateTime lastUpdated { get; set; }

        [Display(Name = "Recorded Checkins")]
        public List<VenueCheckin> RecordedCheckins { get; set; }

        public List<UserDetails> Managers { get; set; }

        public VenueDetails() { }

        public VenueDetails(Venue venue)
        {
            this.VenueId = venue.VenueId;
            this.Name = venue.Name;
            this.Checkins = venue.CheckinCount;
            this.lastUpdated = venue.lastUpdated;
            
            this.Managers = venue.Managers
                .Select(m => new UserDetails(m))
                .ToList();
            
            this.RecordedCheckins = venue.PushCheckins
                .OrderBy(c => c.CheckinTime)
                .Take(20)
                .Select(c => new VenueCheckin(c))
                .ToList();
        }
    }

    public class VenueCheckin
    {
        [Display(Name="VenueCheckin_UserName", ResourceType=typeof(VenueStrings))]
        public string UserName { get; set; }

        [Display(Name = "VenueCheckin_CheckinTime", ResourceType=typeof(VenueStrings))]
        public DateTime CheckinTime { get; set; }

        public VenueCheckin() { }

        public VenueCheckin(PushCheckin checkin)
        {
            this.UserName = checkin.UserName;
            this.CheckinTime = checkin.CheckinTime;
        }
    }
}