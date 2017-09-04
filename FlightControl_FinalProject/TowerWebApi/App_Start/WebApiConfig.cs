using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;

namespace TowerWebApi
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            // Web API configuration and services

            // Web API routes
            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute(
                name: "EmergencyApi",
                routeTemplate: "api/{controller}/emergency/{id}",
                defaults: new { Action = "emergency" }
            );

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );
        }
    }
}
