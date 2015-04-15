using DotNetOpenAuth.Messaging;
using FoursquareOAuth.ApiClasses;
using Newtonsoft.Json;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Net;
using Newtonsoft.Json.Linq;
using log4net;
using System.Threading;
using System.Threading.Tasks;

namespace FoursquareOAuth
{
    public class FSqApiInterface
    {
        private static readonly ILog _log = LogManager.GetLogger(typeof(FSqApiInterface));
        private FSqApiConfigSection _config;
        private IAccessTokenManager _tokenManager;

        #region Properties
        public string ClientId
        {
            get { return _config.ClientId; }
        }

        public string ClientSecret
        {
            get { return _config.ClientSecret; }
        }

        public string PushSecret
        {
            get { return _config.PushSecret; }
        }

        public string ApiEndpoint
        {
            get { return _config.ApiEndpoint; }
        }

        public string ApiVersion
        {
            get { return _config.ApiVersion; }
        }

        public string ProfileUrlPrefix
        {
            get { return _config.ProfileUrlPrefix; }
        }
        #endregion

        /// <summary>
        /// Uses default configuration section name of "foursqareApi"
        /// </summary>
        public FSqApiInterface(IAccessTokenManager TokenManager)
            : this(TokenManager, "foursqareApi")
        {
        }

        public FSqApiInterface(IAccessTokenManager TokenManager, string configSectionName)
        {
            _tokenManager = TokenManager;
            _config = (FSqApiConfigSection)System.Configuration.ConfigurationManager.GetSection("foursqareApi");
        }

        public List<FSqVenue> GetManagedVenues(string accessToken)
        {
            UriBuilder uriBuilder = getUriBuilder("/venues/managed", accessToken);
            JObject response = sendRequest(uriBuilder.Uri);
            return response["venues"].ToObject<FSqVenueList>().items;
        }

        public FSqUser GetUserData(string accessToken)
        {
            JObject response = queryApi(accessToken, "/users/self");
            return response == null ? null : response["user"].ToObject<FSqUser>();
        }

        public Uri GetProfileUrl(string profileId)
        {
            return new Uri(ProfileUrlPrefix + profileId);
        }

        public int getVenueCheckins(string accessToken, string venueID)
        {
            throw new NotImplementedException();
        }

        public JObject queryApi(string accessToken, string apiPath)
        {
            UriBuilder uriBuilder = getUriBuilder(apiPath, accessToken);
            return sendRequest(uriBuilder.Uri);
        }

        #region Helpers

        /// <summary>
        /// Gets a uri builder with the specified Foursquare API path and access token
        /// Automatically appends the version parameter
        /// </summary>
        private UriBuilder getUriBuilder(string path, string accessToken)
        {
            UriBuilder uriBuilder = new UriBuilder(ApiEndpoint + path);
            uriBuilder.AppendQueryArgument("oauth_token", accessToken);
            uriBuilder.AppendQueryArgument("v", ApiVersion);
            return uriBuilder;
        }

        /// <summary>
        /// Send a request to the forusquare api using the provided uri
        /// </summary>
        /// <returns>The JSON object returned by the api</returns>
        private JObject sendRequest(Uri uri)
        {
            using (WebClient wc = new WebClient())
            {
                //TODO: add try-catch
                string textResponse = wc.DownloadString(uri);
                return DeserializeResponse(textResponse);
            }
        }

        /// <summary>
        /// Send a request asynchronously to the forusquare api using the provided uri
        /// </summary>
        private void sendRequestAsync(Uri uri, Action callback)
        {
            //send request on new thread and execute callback on completion
            Task.Factory.StartNew(() =>
            {
                sendRequest(uri);
                callback();
            });
        }

        private JObject DeserializeResponse(string textResponse)
        {
            FSqApiResponse response = JsonConvert.DeserializeObject<FSqApiResponse>(textResponse);

            if(response.Meta.code == 401 && response.Meta.errorType == "invalid_auth")
            {
                _log.Warn(String.Format("Foursqare access code '{0}' is no longer valid"));
                /* TODO: handle this:
                 * _tokenManager.DeleteAccessToken(); 
                 */
            }

            if (response.Meta.code != 200)
            {
                _log.Error(String.Format("Foursqare returned {0} response ({1}):{2}", response.Meta.code, response.Meta.errorType + ":" + response.Meta.errorDetail));
                return null;
            }
            else
            {
                return response.Response;
            }
        }

        #endregion
    }
}
