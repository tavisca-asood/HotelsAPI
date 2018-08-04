using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HotelsAPI.Models
{
    public class ApiResponse
    {
        public List<Hotel> hotelsResponse;
        public Status status { get; set; }
    }
}