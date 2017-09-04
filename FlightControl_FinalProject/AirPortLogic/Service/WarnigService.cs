using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AirPortLogic.Infra;
using Models.Entities;
using Models.Infra;
using Microsoft.Practices.Unity;
using AirPortLogic.Models;
using DAL.Infra;
using System.Threading;

namespace AirPortLogic.Service
{
    public class WarnigService : IWarnigService
    {
        [Dependency("Instance")]
        public ITowerControl Tower { get; set;}
        public ITowerDataService DataService { get; set; }

        Random _rnd = new Random();

        public WarnigService(ITowerDataService dataService)
        {
            DataService = dataService;
        }


        public void SetWarning(Plane plane = null, BaseLeg leg = null)
        {
            // create an emergency in pane or in leg or random in airport
            if (plane != null)
            {
                var tPlane = Tower.Planes.First(p => p.Id == plane.Id);
                if (tPlane != null)
                {
                    tPlane.SetEmergency();
                } 
            }
             else if (leg != null)
            {
                var tLeg = Tower.Legs.First(l => l.Id == leg.Id);
                if (tLeg != null)
                {
                    SetLegEmergency(tLeg);
                } 
            }
            else
            {// by default emergency will be choosen randomly 
                if (_rnd.Next()%2 == 0) // mod 2 emergency in plane
                {
                    var planes = Tower.Planes.ToArray();
                    int count = planes.Count();
                    Tower.Planes.ElementAt(_rnd.Next(count)).SetEmergency();
                }
                else
                {
                    int count = Tower.Legs.Count();
                    var legTower = Tower.Legs.ElementAt(_rnd.Next(count));
                    SetLegEmergency(legTower);
                }
            }
        }

        private void SetLegEmergency(BaseLeg leg)
        {
            // simulate leg emergency close leg
            leg.InEmergency = true;

            DataService.AddOrUpadateLeg(leg);
            
            //  call real help
            //Thread.Sleep(10000);
            //leg.EmergencyStop();
            //DataService.AddOrUpadateLeg(leg);
            //_httpSocket.UpadateLeg(leg);
        }

        public void StopWarning(BaseLeg leg)
        {
            if (leg != null)
            {
                var tLeg = Tower.Legs.First(l => l.Id == leg.Id);
                if (tLeg != null)
                {
                    tLeg.EmergencyStop();
                    DataService.AddOrUpadateLeg(tLeg);
                } 
            }
        }
    }
}
