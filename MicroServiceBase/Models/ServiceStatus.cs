using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MicroServiceBase.Template.Models
{
    public class ServiceStatus
    {
        public double DaysUpTime { get; set; }
        public int Calls { get; set; }
        public string EnvironmentName { get; set; }
        public string ApplicationName { get; set; }
        public string ContentRootPath { get; set; }
    }
}
