using Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AirPortLogic.Infra
{
    public interface IClientHtttpSocketService : ILogService, IDisposable
    {
        // abstract to client singnalr web socket
        // have all thing to conet to web client, front end, to airport

        Task StartConnection();
        void StopConnection();

        // in hub
        // add flight to airport
        //void AddFlight(Fligth newFlight);

        // changes of legs or planes
        void UpadateLeg(BaseLeg leg);
        void UpadatePlane(Plane plane);
        // void log in IlogService

        // in hub
        // event to client
        //event Action<string> onLog;
        //event Action<Plane> onPlaneChange;
        //event Action<BaseLeg> onLegChnge;

        // in hub
        // set emergency from client
        //void SetEmergency(Plane plane = null, BaseLeg leg = null);
        //void StopEmergency(BaseLeg leg = null);

        // event to airpot in set emergency
        event Action<Plane, BaseLeg> onSetEmergency;
        event Action<BaseLeg> onStopEmergency;
    }
}
