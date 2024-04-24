using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace BankEncryptionAPI.Controllers
{
    public class HealthController : ApiController
    {
        [HttpGet]
        [Route("api/health")]
        public IHttpActionResult CheckHealth()
        {
           
            // If the health check is successful, return HTTP 200 OK
            return Ok("Bank API is healthy Test");

            // If there are issues, return an appropriate HTTP status code and a custom message
            // return InternalServerError(new Exception("Health check failed"));
        }
    }
}