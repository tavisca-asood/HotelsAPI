using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HotelsAPI.Models
{
    public enum ApiStatus
    {
        Success,
        Failiure,
        NotFound
    }
    public class Status
    {
        public string StatusMessage { get; set; }
        public int StatusCode { get; set; }
        public ApiStatus ApiStatus { get; set; }
    }
}