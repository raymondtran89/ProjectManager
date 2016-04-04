using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using ProjectManager.WebUI.Infrastructure;

namespace ProjectManager.WebUI
{
    public class MvcApplication : HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            EfConfig.Initialize();
        }
    }
}