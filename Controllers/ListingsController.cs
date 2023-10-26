using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using static JayrideAPI.Models.ListingsModel;

namespace JayrideAPI.Controllers
{
    public class ListingsController : ApiController
    {
        // GET: api/Listings
        private const string SEARCH_ENDPOINT = "https://jayridechallengeapi.azurewebsites.net/api/QuoteRequest";
        [Route("API/Listings/{numofpassengers}")]
        public IHttpActionResult GetListings(string numofpassengers)
        {
            //if we pass numofpassengers=2 as a query string in parameter 
            //Uri theRealURL = new Uri(HttpContext.Current.Request.Url.Scheme + "://" + HttpContext.Current.Request.Url.Authority + HttpContext.Current.Request.RawUrl);
            //string numofpassengers = HttpUtility.ParseQueryString(theRealURL.Query).Get("numofpassengers");

            HttpClient client = new HttpClient();
            int num_passengersInt = 0;
            if (string.IsNullOrEmpty(numofpassengers))
            {
                return Json(new { error = "Number of passengers is missing. Pass numofpassengers as parameter." });
            }
            try
            {
                num_passengersInt = Convert.ToInt32(numofpassengers);
            }
            catch (FormatException)
            {
                return Json(new { error = "Invalid number of passengers." });
            }
            HttpResponseMessage search_response = client.GetAsync(SEARCH_ENDPOINT).GetAwaiter().GetResult();
            if (search_response.StatusCode != HttpStatusCode.OK)
            {
                return Json(new { error = "Failed to fetch search data." });
            }
            string search_data = search_response.Content.ReadAsStringAsync().GetAwaiter().GetResult();

            Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(search_data);
            List<Listing> lst = myDeserializedClass.listings;

            List<FilterListing> filtered_listings = new List<FilterListing>();
            if (lst.Count > 0)
            {
                filtered_listings.Clear();
                foreach (Listing listing in lst)
                {
                    // Filter out listings that don't support the required number of passengers
                    if (listing.vehicleType.maxPassengers != num_passengersInt)
                    {
                        filtered_listings.Add(new FilterListing()
                        { name = listing.name, vehicleType = listing.vehicleType, pricePerPassenger = listing.pricePerPassenger });
                    }
                }
            }
            var Filteroutlst = filtered_listings.ToList();
            foreach (FilterListing fil in Filteroutlst)
            {
                // Calculate the total price for the remaining listings
                var calvalue = fil.pricePerPassenger * fil.vehicleType.maxPassengers;
                fil.totalPrice = Math.Round(calvalue, 2);
            }
            // Sort the filtered listings by total price
            var filteredListings = filtered_listings.OrderBy(x => x.totalPrice).ToList();
            return Json(filteredListings);
        }
        public class FilterListing
        {
            public string name { get; set; }
            public double pricePerPassenger { get; set; }
            public VehicleType vehicleType { get; set; }
            public double totalPrice { get; set; }
        }
    }
}
