using MicroServiceBase.Template.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MicroServiceBase.Template
{
    public class GlobalStatus
    {
        private ILogger<GlobalStatus> logger;

        private DateTime startupTime;
        private int countCalls;
        private IWebHostEnvironment environment;

        private object lockThis = new object();

        public GlobalStatus(ILogger<GlobalStatus> logger, IWebHostEnvironment environment)
        {
            this.logger = logger;
            this.startupTime = DateTime.Now;
            this.environment = environment;
        }

        public ServiceStatus GetStatus()
        {
            return new ServiceStatus
            {
                DaysUpTime = (DateTime.Now - startupTime).TotalDays,
                Calls = countCalls,
                EnvironmentName = environment.EnvironmentName,
                ContentRootPath = environment.ContentRootPath,
                ApplicationName = environment.ApplicationName
            };
        }

        public bool IsDevelopment => GetStatus().EnvironmentName == "Development";
        public void ApiCalled(string methodName)
        {
            logger.LogDebug("method {methodName} call", methodName);
            lock (lockThis)
            {
                countCalls++;
            }
        }
    }
}
