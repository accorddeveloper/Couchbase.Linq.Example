using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Couchbase.Linq.Example.Models
{
    public class UnnestedScheduleModel
    {
        public string Airline { get; set; }
        public int Day { get; set; }
        public TimeSpan UTC { get; set; }
    }
}