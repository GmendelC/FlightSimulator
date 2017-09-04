using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace TowerWebApi.Controllers
{
    public class loggerController : ApiController
    {
        public IEnumerable<string> Get()
        {
            string[] loggs = { "dhkfhs", "khhkshfka"};

            return loggs;
        }
    }
}
