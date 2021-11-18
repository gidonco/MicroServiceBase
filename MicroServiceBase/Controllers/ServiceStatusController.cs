using MicroServiceBase.Template.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace MicroServiceBase.Template.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ServiceStatusController : ControllerBase
    {
        private readonly GlobalStatus status;
        private ILogger<ServiceStatusController> logger;

        public ServiceStatusController(GlobalStatus status, ILogger<ServiceStatusController> logger)
        {
            this.status = status;
            this.logger = logger;
        }


        // GET: api/<ServiceStatusController>
        [HttpGet]
        public ServiceStatus Get()
        {
            status.ApiCalled(System.Reflection.MethodBase.GetCurrentMethod().Name); //  do this in every rest call

            var svcStatus = status.GetStatus();

            logger.LogInformation("returning status, days uptime : {DaysUp:0.000}", svcStatus.DaysUpTime);
            return svcStatus;
        }

        /*
        // GET api/<ServiceStatusController>/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/<ServiceStatusController>
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT api/<ServiceStatusController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<ServiceStatusController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
        */
    }
}
