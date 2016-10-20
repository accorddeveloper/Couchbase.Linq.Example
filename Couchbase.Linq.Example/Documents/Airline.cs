using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using Couchbase.Linq.Filters;
using Newtonsoft.Json;

namespace Couchbase.Linq.Example.Documents
{
    // Filter used when querying these documents
    [DocumentTypeFilter(TypeString)]
    public class Airline
    {
        public const string TypeString = "airline";

        // Read only property to return the calculated primary key
        // This is used when saving the document
        [JsonIgnore]
        [Key]
        public string Key => TypeString + "_" + Id;

        public virtual string Callsign { get; set; }
        public virtual string Country { get; set; }
        [JsonProperty("iata")]
        public virtual string IATA { get; set; }
        [JsonProperty("iaco")]
        public virtual string IACO { get; set; }
        public virtual int Id { get; set; }
        public virtual string Name { get; set; }

        // Type is now readonly to maintain consistency
        public string Type => TypeString;
    }
}