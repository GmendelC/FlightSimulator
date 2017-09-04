using AirPortLogic.Models;
using DAL;
using DAL.Infra;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Routing;

namespace TowerWebApi
{
    public class WebApiApplication : System.Web.HttpApplication
    {
        static public ITowerDataService DataService = new TowerDataService();

        //static private ModelLocator _locator;

        //static public ModelLocator Locator {
        //    get { return _locator ?? (_locator = new ModelLocator()); }
        //}

        // static private BackgroundTaskManager ManagerBackgroundTask;
        protected void Application_Start()
        {
            //Locator.Tower.AddFlight(null);
            GlobalConfiguration.Configure(WebApiConfig.Register);
        }
    }
}
