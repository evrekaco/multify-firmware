using DatabaseContext;
using FoursquareOAuth;
using log4net;
using Microsoft.Web.WebPages.OAuth;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebMatrix.WebData;

namespace CounterWebSite
{
    public class AccessTokenManager : IAccessTokenManager
    {
        private static readonly ILog _log = LogManager.GetLogger(typeof(AccessTokenManager));
        private const string ProviderName = "Foursquare";

        /// <summary>
        /// Create a new access token record in the database with the provided parameters
        /// </summary>
        /// <param name="FoursquareId"></param>
        /// <param name="accessToken"></param>
        public void SaveAccessToken(string FoursquareId, string accessToken)
        {
            using(CounterDbContext _db = new CounterDbContext())
            {
                AccessToken DbToken = _db.AccessTokens.Find(FoursquareId);
                if (DbToken == null)
                {
                    //create a new access token entry
                    DbToken = new AccessToken()
                    {
                        FoursquareId = FoursquareId,
                        Token = accessToken
                    };

                    _db.AccessTokens.Add(DbToken);
                }
                else
                {
                    //update existing access token entry
                    DbToken.Token = accessToken;
                }

                _db.SaveChanges();
                _log.InfoFormat("Saved access token successfully for user '{0}'", FoursquareId);
            }
        }

        /// <summary>
        /// Retrieve the access token for the specified fourquare user
        /// </summary>
        /// <param name="FoursquareId"></param>
        /// <returns>The access token as a string, or null if none exists</returns>
        public string RetrieveAccessToken(string FoursquareId)
        {
            using (CounterDbContext _db = new CounterDbContext())
            {
                AccessToken token = _db.AccessTokens.Find(FoursquareId);
                if (token == null)
                {
                    return null;
                }

                return string.IsNullOrEmpty(token.Token) ? null : token.Token;
            }
        }

        /// <summary>
        /// Deletes the access token and the OAuth account for the corresponding user
        /// </summary>
        /// <param name="FoursquareId"></param>
        public void DeleteAccessToken(string FoursquareId)
        {
            throw new NotImplementedException();

            //delete the account for this user so they will have to connect again
            OAuthWebSecurity.DeleteAccount(ProviderName, FoursquareId);

            using (CounterDbContext _db = new CounterDbContext())
            {
                //remove the access token
                _db.AccessTokens.Find(FoursquareId).Token = null;
                _db.SaveChanges();
            }

            _log.InfoFormat("Deleted OAuth account and access token successfully for user '{0}'", FoursquareId);
        }
    }
}