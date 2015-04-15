using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FoursquareOAuth
{
    public interface IAccessTokenManager
    {
        void SaveAccessToken(string FoursquareId, string accessToken);  //used in OAuthClient when retrieving user data
        string RetrieveAccessToken(string FoursquareId);                //used in each API method to get the access token for the specified UserID
        void DeleteAccessToken(string FoursquareId);                    //used when an API request returns "invalid_auth"
    }
}
