using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace BankEncryptionAPI
{
    public class WebApiApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            GlobalConfiguration.Configure(WebApiConfig.Register);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            log4net.Util.LogLog.InternalDebugging = true;
            var baseDirectory = AppDomain.CurrentDomain.BaseDirectory;
            log4net.Config.XmlConfigurator.Configure(new FileInfo(baseDirectory + "log4net.config"));

        }
    }
}
