using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using Couchbase.Linq.Filters;
using Newtonsoft.Json;

namespace Couchbase.Linq.Example.Documents
{
    // JSON decorators not shown to simplify display
    [DocumentTypeFilter(TypeString)]
    public class Route
    {
        public const string TypeString = "route";

        // Read only property to return the calculated primary key
        // This is used when saving the document
        [Key]
        public string Key => TypeString + "_" + Id;

        public virtual string Airline { get; set; }
        [JsonProperty("airlineid")]
        public virtual string AirlineId { get; set; }
        [JsonProperty("destinationairport")]
        public virtual string DestinationAirport { get; set; }
        public virtual double Distance { get; set; }
        public virtual string Equipment { get; set; }
        public virtual int Id { get; set; }
        public virtual IList<Schedule> Schedule { get; set; }
        [JsonProperty("sourceairport")]
        public virtual string SourceAirport { get; set; }
        public virtual int Stops { get; set; }
        public string Type => TypeString;
    }
}