using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace Leagues
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                "Admin_elmah",
                "elmah/{type}",
                new { action = "Index", controller = "Elmah", type = UrlParameter.Optional });

            routes.MapPageRoute("TuesdayTeams", "Reports", "~/Reports/TuesdayTeamsReport.aspx", false, null,
                new RouteValueDictionary(new { controller = new IncomingRequestConstraint() }));

            routes.MapPageRoute("WednesdayTeams", "Reports", "~/Reports/WednesdayTeamsReport.aspx", false, null,
                new RouteValueDictionary(new { controller = new IncomingRequestConstraint() }));

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
            );
        }
    }

    public class IncomingRequestConstraint : IRouteConstraint
    {
        public bool Match(HttpContextBase httpContext, Route route, string parameterName, RouteValueDictionary values, RouteDirection routeDirection)
        {
            return routeDirection == RouteDirection.IncomingRequest;
        }
    }
}
