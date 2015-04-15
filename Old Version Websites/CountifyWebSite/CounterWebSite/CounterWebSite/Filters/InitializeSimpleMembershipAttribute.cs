using System;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Threading;
using System.Web.Mvc;
using WebMatrix.WebData;
using CounterWebSite.Models;
using DatabaseContext;
using System.Web.Security;
using System.Linq;

namespace CounterWebSite.Filters
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = false, Inherited = true)]
    public sealed class InitializeSimpleMembershipAttribute : ActionFilterAttribute
    {
        private static SimpleMembershipInitializer _initializer;
        private static object _initializerLock = new object();
        private static bool _isInitialized = false;

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            // Ensure ASP.NET Simple Membership is initialized only once per app start
            //LazyInitializer.EnsureInitialized(ref _initializer, ref _isInitialized, ref _initializerLock);
            
            //no lazy loading
            //TODO: move this to Global.asax and get rid of this attribute
            lock(_initializerLock)
            {
                if(!_isInitialized)
                {
                    _initializer = new SimpleMembershipInitializer();
                    _isInitialized = true;
                }
            }
        }

        private class SimpleMembershipInitializer
        {
            public SimpleMembershipInitializer()
            {
                Database.SetInitializer<CounterDbContext>(null);

                try
                {
                    using (var context = new CounterDbContext())
                    {
                        if (!context.Database.Exists())
                        {
                            // Create the SimpleMembership database without Entity Framework migration schema
                            ((IObjectContextAdapter)context).ObjectContext.CreateDatabase();
                        }
                    }

                    WebSecurity.InitializeDatabaseConnection("DefaultConnection", "UserProfile", "UserId", "UserName", autoCreateTables: true);
                    
                    seedDatabase();
                }
                catch (Exception ex)
                {
                    throw new InvalidOperationException("The ASP.NET Simple Membership database could not be initialized. For more information, please see http://go.microsoft.com/fwlink/?LinkId=256588", ex);
                }
            }

            /// <summary>
            /// Create default roles and users
            /// </summary>
            private void seedDatabase()
            {
                //create administrator role
                if (!Roles.RoleExists("Administrators"))
                {
                    Roles.CreateRole("Administrators");
                }

                using (CounterDbContext db = new CounterDbContext())
                {
                    //create subscription types
                    SubscriptionManager sm = new SubscriptionManager(db);
                    sm.SeedSubscriptions();
                }
                
            }
        }
    }
}
