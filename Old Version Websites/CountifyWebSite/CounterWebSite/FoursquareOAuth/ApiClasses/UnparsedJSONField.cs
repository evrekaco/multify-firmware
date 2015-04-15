using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;

namespace FoursquareOAuth
{
    //Field containing JSON data that has not be deserialized yet
    public class UnparsedJSONField
    {
        [JsonExtensionData]
        public IDictionary<string, JToken> fieldData;
    }
}
