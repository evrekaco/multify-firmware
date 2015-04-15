using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FoursquareOAuth.ApiClasses
{
    class FSqVenueList
    {
        public int count { get; set; }
        public List<FSqVenue> items { get; set; }
    }
}
