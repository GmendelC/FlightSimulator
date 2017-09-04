using FactoryServer.Models;
using Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace FactoryServer.Controllers
{
    public class FactoryController : ApiController
    {
            public void Post([FromBody] Plane plane)
            {
                 FlightFactory.Instance.ReturnPlane(plane);
            }
    }
}
