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
    [HubName("ClientHub")]
    public class ClientHub : Hub
    {
        public ClientHub()
        {

        }
        public void UpadateLeg(BaseLeg leg)
        {
            // in chage emergency status, plane enter or leave
            Clients.Others.upadateLeg(leg);
        }

        public void UpadatePlane(Plane plane)
        {
            // in chage emergency status, plane enter or leave
            Clients.Others.upadatePlane(plane);
        }

        public void SendLog(string message)
        {
            Clients.Others.onLog(message);
        }

        public void SetEmergency(Plane plane = null, BaseLeg leg = null)
        {
            Clients.Others.onEmergencyStart(plane, leg);
        }

        public void StopEmergency(BaseLeg leg = null)
        {
            Clients.Others.onEmergencyStop(leg);
        }
    }
}
