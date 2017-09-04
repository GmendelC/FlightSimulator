using System;
using AirPortLogic.Models;
using PlaneFactory.Models;

namespace TowerRunner
{
    class Program
    {
        static void Main(string[] args)
        {
            var locator = new ModelLocator();
            var tower = locator.Tower;
            var warning = locator.WarnigService;
            var factory = FlightFactory.Instance;
            var webSocekt = locator.WebSocketService;

            factory.SendFlightEvent += tower.AddFlight;
            tower.OnPlaneReturn += factory.ReturnPlane;
            webSocekt.onSetEmergency += warning.SetWarning;
            webSocekt.onStopEmergency += warning.StopWarning;

            webSocekt.StartConnection();

            Console.WriteLine("Your air port was hosting to flights");

            Console.ReadLine();

            factory.SendFlightEvent -= tower.AddFlight;
            tower.OnPlaneReturn -= factory.ReturnPlane;
            webSocekt.onSetEmergency -= warning.SetWarning;
            webSocekt.onStopEmergency -= warning.StopWarning;

            webSocekt.Dispose();
            Console.WriteLine("Host stop");

            Console.ReadLine();
        }
    }
}
