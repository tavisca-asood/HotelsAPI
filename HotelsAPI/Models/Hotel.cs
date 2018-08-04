using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HotelsAPI.Models
{
    public class Hotel
    {
        public string Name { get; set; }
        public string Address { get; set; }
        public string latitude { get; set; }
        public string longitude { get; set; }

    }
}