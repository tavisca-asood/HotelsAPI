using HotelsAPI.Models;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace HotelsAPI.Controllers
{
    public class HotelController : ApiController
    {
        string AutoCompleteAPI = @"http://autocomplete.geocoder.api.here.com/6.2/suggest.json?app_id=izvoQkf6OFl1c0bKCN9N&app_code=gDe4K-dVDGW4m1oFkr95HQ&query={0}";
        string GeolocationAPI = @"https://geocoder.cit.api.here.com/6.2/geocode.json?searchtext={0}&app_id=izvoQkf6OFl1c0bKCN9N&app_code=gDe4K-dVDGW4m1oFkr95HQ&limit=1";
        string HotelsAPI = @"https://api.foursquare.com/v2/venues/search?ll={0},{1}&client_id=QIIBSDIFGOD4QOGMZ1BNSCDADTGCHMNNT2VEJBMGPWDTVNM5&client_secret=HXUAVNLAD5WTNGM0U5EIO5LOG4ZCGGMHNHPIWWWS1AS3ZDPX&v=20180824&query=hotel&limit=15&radius=50000";
        List<Hotel> hotels = new List<Hotel>();



        // GET: api/Hotel
        public List<Hotel> Get()
        {
            return hotels;
        }

        // GET: api/Hotel/5
        public ApiResponse Get(string id)
        {
            string pageData = "";
            dynamic result;
            pageData = getPage(String.Format(AutoCompleteAPI, id));
            result = JsonConvert.DeserializeObject(pageData);
            string citySuggestion = "";
            try
            {
                citySuggestion = result.suggestions[0].label;
            }
            catch(Exception exception)
            {
                return new ApiResponse()
                {
                    hotelsResponse = hotels,
                    status = new Status()
                    {
                        ApiStatus = ApiStatus.Failiure,
                        StatusCode = 404,
                        StatusMessage = "No suggestion found!"
                    }
                };
            }
            pageData = getPage(String.Format(GeolocationAPI, citySuggestion));
            result = JsonConvert.DeserializeObject(pageData);
            string latitude = result.Response.View[0].Result[0].Location.DisplayPosition.Latitude;
            string longitude = result.Response.View[0].Result[0].Location.DisplayPosition.Longitude;
            pageData=getPage(String.Format(HotelsAPI, latitude, longitude));
            result = JsonConvert.DeserializeObject(pageData);
            string address = "";
            foreach (var venue in result.response.venues)
            {
                var formattedAddress = venue.location.formattedAddress;
                foreach (var temporaryAddress in formattedAddress)
                {
                    address += temporaryAddress;
                    address += ", ";
                }
                address=address.Replace("Ä\u0081", "a");
                hotels.Add(
                    new Hotel()
                    {
                        Name = venue.name,
                        Address = address,
                        latitude = venue.location.lat,
                        longitude = venue.location.lng
                    });
            }
            return new ApiResponse()
            {
                hotelsResponse = hotels,
                status = new Status()
                {
                    ApiStatus = ApiStatus.Success,
                    StatusCode = 200,
                    StatusMessage = "Success!"
                }
            };
        }

        // POST: api/Hotel
        public void Post([FromBody]string value)
        {
        }

        // PUT: api/Hotel/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE: api/Hotel/5
        public void Delete(int id)
        {
        }

        public string getPage(string url)
        {
            string pageData = "";
            using (WebClient client = new WebClient())
            {
                pageData = client.DownloadString(url);
            }
            return pageData;
        }

    }
}
