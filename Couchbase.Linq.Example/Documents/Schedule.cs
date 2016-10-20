using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Newtonsoft.Json;

namespace Couchbase.Linq.Example.Documents
{
    public class Schedule
    {
        public virtual int Day { get; set; }
        public virtual string Flight { get; set; }
        [JsonProperty("utc")]
        public virtual TimeSpan UTC { get; set; }
    }
}