using DotNetOpenAuth.AspNet.Clients;
using DotNetOpenAuth.Messaging;
using FoursquareOAuth.ApiClasses;
using Newtonsoft.Json;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Net;
using Newtonsoft.Json.Linq;

namespace FoursquareOAuth
{
    public class FoursquareOAuthClient : OAuth2Client
    {
        private const string AuthorizationEndpoint = "https://foursquare.com/oauth2/authenticate";
        private const string TokenEndpoint = "https://foursquare.com/oauth2/access_token";
        
        private FSqApiInterface _FSqApi;
        private IAccessTokenManager _TokenManager;

        public FoursquareOAuthClient(IAccessTokenManager TokenManager) : base("Foursquare")
        {
            this._FSqApi = new FSqApiInterface(TokenManager);
            this._TokenManager = TokenManager;
        }

        protected override Uri GetServiceLoginUrl(Uri returnUrl)
        {
            UriBuilder uriBuilder = new UriBuilder(AuthorizationEndpoint);
            uriBuilder.AppendQueryArgument("client_id", _FSqApi.ClientId);
            uriBuilder.AppendQueryArgument("response_type", "code");
            uriBuilder.AppendQueryArgument("redirect_uri", returnUrl.AbsoluteUri);
            return uriBuilder.Uri;
        }

        protected override IDictionary<string, string> GetUserData(string accessToken)
        {
            //get user data from the FS api
            FSqUser fsUser = _FSqApi.GetUserData(accessToken);

            //save data in a dictionary
            Dictionary<string, string> userData = new Dictionary<string, string>(5);
            userData["id"] = fsUser.id;
            userData["username"] = string.Format("{0} {1}", fsUser.firstName, fsUser.lastName).Trim();
            userData["oauth_token"] = accessToken;
            userData["type"] = fsUser.type;
            //userData["photoUrl"] = fsUser.photo.GetUrl(Photo.PhotoSize.ThreeHundred);
            userData["checkins"] = fsUser.checkins.count.ToString();
            userData["phone"] = fsUser.contact.phone;
            userData["email"] = fsUser.contact.email;
            //userData["bio"] = fsUser.bio;

            //save access token
            _TokenManager.SaveAccessToken(fsUser.id, accessToken);

            return userData;
        }

        protected override string QueryAccessToken(Uri returnUrl, string authorizationCode)
        {
            //build the request uri
            UriBuilder uriBuilder = new UriBuilder(TokenEndpoint);
            uriBuilder.AppendQueryArgument("client_id", _FSqApi.ClientId);
            uriBuilder.AppendQueryArgument("client_secret", _FSqApi.ClientSecret);
            uriBuilder.AppendQueryArgument("grant_type", "authorization_code");
            uriBuilder.AppendQueryArgument("redirect_uri", returnUrl.AbsoluteUri);
            uriBuilder.AppendQueryArgument("code", authorizationCode);

            //send the request and return the result
            string result;
            using (WebClient webClient = new WebClient())
            {
                string text = webClient.DownloadString(uriBuilder.Uri);
                if (string.IsNullOrEmpty(text))
                {
                    result = null;
                }
                else
                {
                    result = JsonConvert.DeserializeObject<Dictionary<string, string>>(text)["access_token"];
                }
            }
            return result;
        }
    }
}