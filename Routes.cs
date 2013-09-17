using System.Collections.Generic;
using System.Web.Mvc;
using System.Web.Routing;
using Orchard.Mvc.Routes;

namespace NogginBox.CustomFormsEdit
{
    public class Routes : IRouteProvider
	{
		public void GetRoutes(ICollection<RouteDescriptor> routes)
		{
			foreach (var routeDescriptor in GetRoutes())
				routes.Add(routeDescriptor);
        }

		public IEnumerable<RouteDescriptor> GetRoutes()
		{
			return new[]
			{
				new RouteDescriptor
				{
					Priority = 6,
					Route = new Route(
						"Content/Edit/{contentId}",
						new RouteValueDictionary
						{
							{"area", "NogginBox.CustomFormsEdit"},
							{"controller", "Home"},
							{"action", "Edit"}
						},
						new RouteValueDictionary {
							{"contentId", @"\d+" }
						},
						new RouteValueDictionary
						{
							{"area", "NogginBox.CustomFormsEdit"}
						},
						new MvcRouteHandler())
				}
			};
		}
	}
}