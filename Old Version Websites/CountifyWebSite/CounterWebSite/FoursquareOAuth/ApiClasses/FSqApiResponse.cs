using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FoursquareOAuth.ApiClasses
{
    public class FSqApiResponse
    {
        public FSqMeta Meta { get; set; }

        [JsonIgnore]
        public List<UnparsedJSONField> Notifications { get; set; }

        public JObject Response { get; set; }
    }
}
