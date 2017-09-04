using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;
using Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hubs
{
    [HubName("TowerHub")]
    public class TowerHub : Hub
    {
        public TowerHub()
        {

        }
        public void SendFlight(Fligth  newFlight)
        {
            Clients.Others.flightCome(newFlight);
        }
    }
}
