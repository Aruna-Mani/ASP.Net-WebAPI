using JayrideAPI.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;

namespace JayrideAPI.Controllers
{
    public class LocationController : ApiController
    {
        // GET: api/Candidate
        [HttpGet]
        public IHttpActionResult GetUserCountryByIp()
        {
            string ip = null;
            LocationModel ipInfo = new LocationModel();
            try
            {
                string info = new WebClient().DownloadString("http://ipinfo.io/" + ip);
                ipInfo = JsonConvert.DeserializeObject<LocationModel>(info);
                return Json(ipInfo);
            }
            catch (Exception)
            {
                ipInfo.Loc = "Unable to fetch the Location";
                return Json(ipInfo.Loc);
            }
        }
    }
}
