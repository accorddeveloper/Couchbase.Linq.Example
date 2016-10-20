using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace Couchbase.Linq.Example
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            // Initialize the Couchbase cluster from Web.config
            ClusterHelper.Initialize("couchbaseClients/couchbase");
        }

        protected void Application_End()
        {
            // Shutdown open connections to the Couchbase cluster
            ClusterHelper.Close();
        }
    }
}
