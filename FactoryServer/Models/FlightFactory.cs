using Models.Entities;
using FactoryServer.Service;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Timers;
using System.Web;

namespace FactoryServer.Models
{
    public class FlightFactory : IDisposable
    {
        static  private FlightFactory _instance;

        static public FlightFactory Instance
        {
            get { return _instance ?? (_instance = new FlightFactory()); }
        }

        public event Action<Fligth> SendFlightEvent;

        private PlaneFactory _factory;

        private Random _rnd;

        private Timer _timer;

        private HttpSenderService _sender;
        private const double INTERVAL_MILISENCOUDS = 1000;

        private FlightFactory()
        {
            _factory = new PlaneFactory();
            _rnd = new Random();
            _timer = new Timer();
            _sender = new HttpSenderService();

             InitTimer();
        }

        private void InitTimer()
        {
            _timer.Interval = INTERVAL_MILISENCOUDS;
            _timer.Elapsed += _timer_Elapsed;
            _timer.Start();
        }

        private async void _timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            if (_rnd.Next()%6 ==0 )
            {
                bool destinationOut = _rnd.Next()%2==0;
                var plane = _factory.GetFreePlane(destinationOut);
                if (plane != null)
                {
                    var newFlight = new Fligth
                    {
                        FlightPlane = plane,
                        Id = Guid.NewGuid(),
                        TotalTime = TimeSpan.FromMilliseconds(0)
                    };

                    
                    //if (!( _sender.SendFlight(newFlight).Result))
                        if(SendFlightEvent != null)
                            SendFlightEvent?.Invoke(newFlight);
                        else
                            _factory.ReturnPlane(plane); 
                }

            }
        }

        public void ReturnPlane(Plane plane)
        {
            _factory.ReturnPlane(plane);
        }

        public void Dispose()
        {
            _timer.Dispose();
        }
    }

}