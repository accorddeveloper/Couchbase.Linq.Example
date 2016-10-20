using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Couchbase.Linq.Example.Documents
{
    public class Coordinate
    {
        public virtual int Altitude { get; set; }
        public virtual double Latitude { get; set; }
        public virtual double Longitude { get; set; }
    }
}