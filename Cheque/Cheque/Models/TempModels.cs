using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace HouseFly.Models
{
    public class TempModels
    {
        [Key]
        public int TempId { get; set; }
        public string Domain { get; set; }
        public decimal Temp { get; set; }
        public decimal Pressure { get; set; }
        public decimal Humidity { get; set; }
        public string TimeStamp { get; set; }

    }
}