using System;
using System.ComponentModel.DataAnnotations;

namespace ProjectWasel21.Models
{
    public class ExternalData
    {

        [Key]
        public int DataId { get; set; } // PK
        public string Source { get; set; }
        public string ExternalKey { get; set; }
        public string JsonData { get; set; }
        public DateTime FetchedAt { get; set; }
    }
}