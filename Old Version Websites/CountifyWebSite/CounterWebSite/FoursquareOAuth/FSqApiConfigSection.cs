using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;

namespace FoursquareOAuth
{
    public class FSqApiConfigSection : ConfigurationSection
    {
        // Create a "clientId" attribute
        [ConfigurationProperty("clientId", IsRequired = true)]
        public String ClientId
        {
            get
            {
                return (String)this["clientId"];
            }
            set
            {
                this["clientId"] = value;
            }
        }

        // Create a "clientSecret" attribute
        [ConfigurationProperty("clientSecret", IsRequired = true)]
        public String ClientSecret
        {
            get
            {
                return (String)this["clientSecret"];
            }
            set
            {
                this["clientSecret"] = value;
            }
        }

        // Create a "pushSecret" attribute
        [ConfigurationProperty("pushSecret", IsRequired = true)]
        public String PushSecret
        {
            get
            {
                return (String)this["pushSecret"];
            }
            set
            {
                this["pushSecret"] = value;
            }
        }

        // Create a "apiVersion" attribute
        [ConfigurationProperty("apiVersion", DefaultValue = "20140414", IsRequired = false)]
        public String ApiVersion
        {
            get
            {
                return (String)this["apiVersion"];
            }
            set
            {
                this["apiVersion"] = value;
            }
        }

        // Create a "apiEndpoint" attribute
        [ConfigurationProperty("apiEndpoint", DefaultValue = "https://api.foursquare.com/v2/", IsRequired = false)]
        public String ApiEndpoint
        {
            get
            {
                return (String)this["apiEndpoint"];
            }
            set
            {
                this["apiEndpoint"] = value;
            }
        }

        // Create a "profileUrlPrefix" attribute
        [ConfigurationProperty("profileUrlPrefix", DefaultValue = " https://foursquare.com/user/", IsRequired = false)]
        public String ProfileUrlPrefix
        {
            get
            {
                return (String)this["profileUrlPrefix"];
            }
            set
            {
                this["profileUrlPrefix"] = value;
            }
        }
    }
}
