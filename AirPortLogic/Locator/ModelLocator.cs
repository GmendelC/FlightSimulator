using Microsoft.Practices.Unity;
using Microsoft.Practices.ServiceLocation;
using Models.Infra;
using AirPortLogic.Infra;
using AirPortLogic.Service;
using DAL.Infra;
using DAL;

namespace AirPortLogic.Models
{
    /// <summary>
    /// This class contains static references to all the view models in the
    /// application and provides an entry point for the bindings.
    /// </summary>
    public class ModelLocator
    {
        private UnityContainer _IOCConteiner;
        public ModelLocator()

        {
            _IOCConteiner = new UnityContainer();
            _IOCConteiner.RegisterType<ITowerControl, TowerControl>();
            _IOCConteiner.RegisterType<ILogService, DebugLogService>();
            _IOCConteiner.RegisterType<ILogService, ConsoleLoggerService>("console");
            _IOCConteiner.RegisterInstance<ILogService>("array", new ArrayLogService());
            _IOCConteiner.RegisterType<IPlanePiorityService, PlanePiorityService>();
            _IOCConteiner.RegisterType<ITowerHttpClientService, TowerHttpClientService>();
            _IOCConteiner.RegisterType<IWarnigService, WarnigService>();
            _IOCConteiner.RegisterType<ITowerDataService, TowerDataService>();
            
            // web socket was singleton
            var clientWebSocketService = new ClientHttpSocketService();
            _IOCConteiner.RegisterInstance<IClientHtttpSocketService>(clientWebSocketService);
            _IOCConteiner.RegisterInstance<ILogService>("webSoket", clientWebSocketService);
            
            // tower have singleton as instance name
            var tower = _IOCConteiner.Resolve<ITowerControl>();
            _IOCConteiner.RegisterInstance<ITowerControl>("Instance", tower);
        }

        public ITowerControl Tower
        {
            get { return _IOCConteiner.Resolve<ITowerControl>("Instance"); }
        }

        public IWarnigService WarnigService
        {
            get { return _IOCConteiner.Resolve<IWarnigService>(); }
        }

        public ITowerDataService TowerDataService
        {
            get { return _IOCConteiner.Resolve<ITowerDataService>(); }
        }
        public IClientHtttpSocketService WebSocketService
        {
            get { return _IOCConteiner.Resolve<IClientHtttpSocketService>(); }
        }
    }
}