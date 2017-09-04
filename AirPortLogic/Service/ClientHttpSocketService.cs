using AirPortLogic.Infra;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Models.Entities;
using Microsoft.AspNet.SignalR.Client;

namespace AirPortLogic.Service
{
    public class ClientHttpSocketService : IClientHtttpSocketService, IDisposable
    {
        IHubProxy _hub;
        private bool _disable;
        private HubConnection _hubConnection;

        public ClientHttpSocketService()
        {
            _disable = true;
            InitHub();
        }

        private void InitHub()
        {
            try
            {
                // init the conction to web soket hub signalr
                
                //config conection and 
                var hubConnection = new HubConnection("http://localhost:6462");
                var hub = hubConnection.CreateHubProxy("ClientHub");

                hub.On<Plane, BaseLeg>(
                    "onEmergencyStart",
                    (plane, leg) => {
                        onSetEmergency?.Invoke(plane, leg); });
                hub.On<BaseLeg>(
                    "onEmergencyStop",
                    (leg) => {
                        onStopEmergency?.Invoke(leg); });
                _hub = hub;
                _hubConnection = hubConnection;
            }
            catch
            {
                _disable = true;
            }
        }

        public event Action<Plane, BaseLeg> onSetEmergency;
        public event Action<BaseLeg> onStopEmergency;

        public async void AddLog(string logMessage)
        {
            if(!_disable)
            try
            {
                var response = await _hub.Invoke<string>("SendLog", logMessage);
            }
            catch (Exception ex)
            {
            }
        }

        public async void UpadateLeg(BaseLeg leg)
        {
            if (!_disable)
                try
            {
                var response = await _hub.Invoke<BaseLeg>("UpadateLeg", leg);
            }
            catch (Exception ex)
            {
            }
        }

        public async void UpadatePlane(Plane plane)
        {
            if (!_disable)
                try
            {
                var response = await _hub.Invoke<Plane>("UpadatePlane", plane);
            }
            catch (Exception ex)
            {
            }
        }

        public async Task StartConnection()
        {
            try
            {
                await _hubConnection.Start();
                _disable = false;
            }
            catch { };
        }

        public void StopConnection()
        {
            try
            {
                _hubConnection.Stop();
                _disable = true;
            }
            catch { };
        }

        public void Dispose()
        {
            StopConnection();
            GC.SuppressFinalize(this);
        }

        ~ClientHttpSocketService()
        {
            StopConnection();
        }
    }
}
