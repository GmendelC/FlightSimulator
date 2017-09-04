using Hubs;
using Microsoft.AspNet.SignalR;
using Models.Entities;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace TowerWebApi.Controllers
{
    public class FlightController : ApiController
    {
        public IEnumerable<Plane> Get()
        {
            //Plane[] mockPlanes =
            //    {
            //        new Plane {Id=1,State=Models.Enums.EState.InAir },
            //        new Plane {Id=2,State=Models.Enums.EState.InAir },
            //        new Plane {Id=3,State=Models.Enums.EState.Terminal, DestinationOut=true }
            //    };
            //return mockPlanes;
            var allPlanes = WebApiApplication
                .DataService.PlanesOutSide.ToList();
            var legsPlanes = WebApiApplication.DataService
                .Legs.Select(l =>
                {
                    if (l.InPlanes != null)
                        return l.InPlanes.FirstOrDefault();
                    else
                        return null;
                })
                .Where(p => p !=null);
            allPlanes.AddRange(legsPlanes);
            return allPlanes;
            //return WebApiApplication.Locator.Tower.Planes;
        }

        public Plane Get(int id)
        {
            //return null;
            return Get().FirstOrDefault(
                plane => plane != null && plane.Id == id);
        }
        [HttpPost]
        public void Post([FromBody] Fligth newFligth)
        {
            //WebApiApplication.Locator.Tower.AddFlight(newFligth);
            GlobalHost.ConnectionManager.GetHubContext<ClientHub>().Clients.All.onFlightCome(newFligth);
        }

        [HttpGet]
        [ActionName("emergency")]
        public void Emergency(int id)
        {
            var warnigPlane = Get(id);
            //WebApiApplication.Locator.WarnigService.SetWarning(warnigPlane);
            GlobalHost.ConnectionManager.GetHubContext<ClientHub>().Clients.All.onEmergencyStart(warnigPlane,null);
        }
    }
}
