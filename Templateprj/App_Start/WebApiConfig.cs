using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Formatting;
using System.Web.Http;

namespace Templateprj
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            // Web API configuration and services

            // Web API routes
            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute(
                      name: "DefaultApi",
                        routeTemplate: "api/{controller}/{action}/{type}/apikey={apikey}",
      defaults: new { id = RouteParameter.Optional }
              );
            config.Routes.MapHttpRoute(
  name: "DefaultiaPi12",
  routeTemplate: "api/{controller}/{action}/{id}",
  defaults: new { id = RouteParameter.Optional });

            config.Routes.MapHttpRoute(
                name: "DefaultAi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );
        }
    }
}
