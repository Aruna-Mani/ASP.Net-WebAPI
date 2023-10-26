using JayrideAPI.Models;
using System.Net.Http;
using System.Web.Http;

namespace JayrideAPI.Controllers
{
    public class CandidateController : ApiController
    {
        // GET: api/Candidate
        public IHttpActionResult Get()
        {
            CandidateModel objCandidate = new CandidateModel();
            objCandidate.name = "test";
            objCandidate.phone = "test";
            return Json(objCandidate);
        }
    }
}
