using Hubs;
using Microsoft.AspNet.SignalR;
using Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace TowerWebApi.Controllers
{
    public class LegController : ApiController
    {
        [HttpGet]
        public IEnumerable<BaseLeg> Get()
        {
            //BaseLeg[] legs =
            //    {
            //        new BaseLeg {Id= 1, LegType= Models.Enums.EState.AirWaiting1},
            //        new BaseLeg {Id= 2, LegType= Models.Enums.EState.AirWaiting2},
            //        new BaseLeg {Id= 3, LegType= Models.Enums.EState.AirWaiting3},
            //        new BaseLeg {Id= 4, LegType= Models.Enums.EState.LandingRoad},
            //        new BaseLeg {Id= 5, LegType= Models.Enums.EState.AccessAfterLanding},
            //        new BaseLeg {Id= 6, LegType= Models.Enums.EState.Boarding},
            //        new BaseLeg {Id= 7, LegType= Models.Enums.EState.Boarding},
            //        new BaseLeg {Id= 8, LegType= Models.Enums.EState.AccessToDeparting}
            //    };
            //return legs;
            return WebApiApplication.DataService.Legs;
            //return WebApiApplication.Locator.Tower.Legs;
        }
        [HttpGet]
        public BaseLeg Get(int id)
        {
            //return null;
            return Get().FirstOrDefault(leg => leg != null && leg.Id == id);
        }

        [HttpGet]
        [ActionName("emergency")]
        public void Emergency(int id)
        {
            ////get to star emergency and delete to stop
            var warnigLeg = Get(id);
            //WebApiApplication.Locator.WarnigService.SetWarning(null, warnigLeg);
            GlobalHost.ConnectionManager.GetHubContext<ClientHub>().Clients.All.onEmergencyStart(null, warnigLeg);
        }

        [HttpDelete]
        [ActionName("emergency")]
        public void DisEmergency(int id)
        {
            var warnigLeg = Get(id);
            //WebApiApplication.Locator.WarnigService.StopWarning(warnigLeg);
            GlobalHost.ConnectionManager.GetHubContext<ClientHub>().Clients.All.onEmergencyStop(warnigLeg);
        }
    }
}
