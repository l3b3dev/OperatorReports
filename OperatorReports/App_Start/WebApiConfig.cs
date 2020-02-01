using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using System.Web.Mvc;
using DataAccessLogicComponent;
using DataAccessLogicComponent.Interfaces;
using OperatorReports.DI;
using Services;
using Services.Interfaces;
using Unity;
using Unity.Lifetime;

namespace OperatorReports
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            //hooking up our DI
            var container = new UnityContainer();
            container.RegisterType<IReportsRepository, ReportsRepository>(new HierarchicalLifetimeManager());
            container.RegisterType<IFilterParamsParser, FilterParamsParser>(new HierarchicalLifetimeManager());
            container.RegisterType<IDurationParser, DurationParser>(new HierarchicalLifetimeManager());
            config.DependencyResolver = new UnityResolver(container);

            // Web API routes
            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );
        }
    }
}
