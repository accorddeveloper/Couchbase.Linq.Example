using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Couchbase.Linq.Example.Documents;
using Couchbase.Linq.Example.Models;
using Couchbase.Linq.Extensions;

namespace Couchbase.Linq.Example.Controllers
{
    public class QueryController : Controller
    {
        public ActionResult Basic()
        {
            var db = new BucketContext(ClusterHelper.GetBucket("travel-sample"));

            var query = from p in db.Query<Airline>()
                        orderby p.Name
                        select p;

            return View(query);
        }

        public ActionResult Advanced()
        {
            var db = new BucketContext(ClusterHelper.GetBucket("travel-sample"));

            // Linq2Couchbase supports most operations you would expect, including many not shown here

            var query = (from p in db.Query<Airline>()
                         where p.Callsign.StartsWith("A")
                         orderby p.Name
                         select new AirlineModel {Callsign = p.Callsign, Name = p.Name}).Take(10);

            return View(query);
        }

        public ActionResult Join()
        {
            var db = new BucketContext(ClusterHelper.GetBucket("travel-sample"));

            // To join between documents, at least on side of the equality must be N1QlFunctions.Key(x)
            // Prior to Couchbase Server 4.5, this must be the right side
            // After 4.5, this can be the left side assuming a proper index is available for the right side

            var query = from route in db.Query<Route>()
                        join airline in db.Query<Airline>() on route.AirlineId equals N1QlFunctions.Key(airline)
                        where route.SourceAirport == "ATL" && route.DestinationAirport == "ABE"
                        orderby airline.Name, route.Stops
                        select new RouteModel
                        {
                            AirlineName = airline.Name,
                            Stops = route.Stops,
                            Schedule = route.Schedule.ToList()
                        };

            return View(query);
        }

        public ActionResult Unnest()
        {
            var db = new BucketContext(ClusterHelper.GetBucket("travel-sample"));

            // Unnest is like an inner join, but run against an array within the main document

            var query = from route in db.Query<Route>()
                        from schedule in route.Schedule
                        where route.SourceAirport == "ATL" && route.DestinationAirport == "ABE"
                        orderby schedule.Day, schedule.UTC
                        select new UnnestedScheduleModel
                        {
                            Airline = route.Airline,
                            Day = schedule.Day,
                            UTC = schedule.UTC
                        };

            return View(query);
        }
    }
}