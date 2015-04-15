using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FoursquareOAuth.ApiClasses
{
    public class FSqPushNotification
    {
        public string id { get; set; }
        public int createdAt { get; set; }
        public string type { get; set; }
        public int timeZoneOffset { get; set; }
        public FSqUser user { get; set; }
        public FSqVenue venue { get; set; }
    }
}
