using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Couchbase.Linq.Example.Documents;
using Couchbase.Linq.Example.Models;
using Couchbase.Linq.Extensions;

namespace Couchbase.Linq.Example.Controllers
{
    public class AdvancedFeaturesController : Controller
    {
        public ActionResult IsMissing()
        {
            var db = new BucketContext(ClusterHelper.GetBucket("travel-sample"));

            // N1QlFunctions.IsMissing returns true if the attribute is not found on the document
            // This is considered different than being null
            // It's the equivalent of "undefined" in Javascript

            var query = from p in db.Query<Airline>()
                        where N1QlFunctions.IsMissing(p.IACO)
                        select p;

            return View(query);
        }

        public ActionResult UseKeys()
        {
            var db = new BucketContext(ClusterHelper.GetBucket("travel-sample"));

            // UseKeys looks up documents using their primary key
            // UseKeys is required for subqueries

            var query = from p in db.Query<Airline>()
                            .UseKeys(new[] {"airline_137", "airline_10765", "airline_1316" })
                        select p;

            return View(query);
        }

        public ActionResult UseIndex()
        {
            var db = new BucketContext(ClusterHelper.GetBucket("travel-sample"));

            // UseIndex provides index hints to the query engine
            // This can improve speed if the query engine isn't making the best index selection

            var query = from p in db.Query<Route>().UseIndex("def_sourceairport")
                        where p.SourceAirport == "ATL"
                        orderby p.DestinationAirport
                        select p;

            return View(query);
        }

        public async Task<ActionResult> Async()
        {
            var db = new BucketContext(ClusterHelper.GetBucket("travel-sample"));

            var query = from p in db.Query<Airline>()
                        orderby p.Name
                        select p;

            // ExecuteAsync runs the query asyncronously

            return View(await query.ExecuteAsync());
        }

        public async Task<ActionResult> AsyncAggregate()
        {
            var db = new BucketContext(ClusterHelper.GetBucket("travel-sample"));

            var query = from p in db.Query<Route>()
                        select p;

            // ExecuteAsync can also be used for First(), Single() and aggregation functions
            // By passing the final step as a lambda

            return View(await query.ExecuteAsync(p => p.Average(q => q.Distance)));
        }
    }
}