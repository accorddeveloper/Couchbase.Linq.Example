using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Couchbase.Linq.Example.Documents;

namespace Couchbase.Linq.Example.Models
{
    public class RouteModel
    {
        public string AirlineName { get; set; }
        public int Stops { get; set; }
        public List<Schedule> Schedule { get; set; }
    }
}