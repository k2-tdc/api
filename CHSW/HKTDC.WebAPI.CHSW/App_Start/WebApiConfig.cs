using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;

namespace HKTDC.WebAPI.CHSW
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            // Web API configuration and services
            //config.EnableCors();
            // Web API routes
            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{action}/{id}",    // 4 
                defaults: new { id = RouteParameter.Optional }
            );

            /*config.Routes.MapHttpRoute(
                 name: "General",
                 routeTemplate: "api/{controller}/users/{uid}/{action}"    // 5
            );

            config.Routes.MapHttpRoute(
                 name: "Worklist-Computer-App",
                 routeTemplate: "api/{controller}/users/{uid}/work-list/computer-app/{para}",    // 7
                 defaults: new { action = "computer-app", para = RouteParameter.Optional }
            );

            config.Routes.MapHttpRoute(
                 name: "Worklist-Action",
                 routeTemplate: "api/{controller}/users/{uid}/work-list/computer-app/{actionid}",    // 7
                 defaults: new { action = "WorklistAction" }
            );

            config.Routes.MapHttpRoute(
                 name: "Get-Approval-Details",
                 routeTemplate: "api/{controller}/users/{uid}/work-list/computer-app/{sn}",    // 7
                 defaults: new { action = "GetApprovaletails" }
            );

            config.Routes.MapHttpRoute(
                 name: "Application-Computer-App",
                 routeTemplate: "api/{controller}/applications/computer-app/{applicationid}/{action}",    // 6
                 defaults: new { action = "Application-Computer-App", applicationid = RouteParameter.Optional }
            );

            config.Routes.MapHttpRoute(
                 name: "User-Application-Computer-App",
                 routeTemplate: "api/{controller}/users/{uid}/applications/computer-app/{action}/{id}",    //8
                 defaults: new { action = "User-Application-Computer-App", id = RouteParameter.Optional }
            );*/
        }
    }
}
