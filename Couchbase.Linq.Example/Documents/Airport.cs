using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using Couchbase.Linq.Filters;
using Newtonsoft.Json;

namespace Couchbase.Linq.Example.Documents
{
    [DocumentTypeFilter(TypeString)]
    public class Airport
    {
        public const string TypeString = "airport";

        // Read only property to return the calculated primary key
        // This is used when saving the document
        [JsonIgnore]
        [Key]
        public string Key => TypeString + "_" + Id;

        [JsonProperty("airportname")]
        public virtual string AirportName { get; set; }
        public virtual string City { get; set; }
        public virtual string Country { get; set; }
        [JsonProperty("faa")]
        public virtual string FAA { get; set; }
        public virtual Coordinate Geo { get; set; }
        [JsonProperty("icao")]
        public virtual string ICAO { get; set; }
        public virtual int Id { get; set; }
        public virtual string Timezone { get; set; }

        public string Type => TypeString;
    }
}