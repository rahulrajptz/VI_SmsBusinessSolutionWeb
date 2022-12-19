using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Http.Formatting;
using System.Net.Http.Headers;
using System.Web;
using System.Web.Configuration;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using UnityConfigs = Templateprj.Ioc.UnityConfig;

namespace Templateprj
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            GlobalConfiguration.Configure(WebApiConfig.Register);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            UnityConfigs.RegisterComponents();

        }

        protected void Session_Start(object sender, EventArgs e)
        {
            // Code that runs when a new session is started
            if (!Request.IsSecureConnection) return;
            Session.Timeout = 60000;
            var sessionStateSection = (SessionStateSection)ConfigurationManager.GetSection("system.web/sessionState");
            var cookieName = sessionStateSection.CookieName ?? "ASP.NET_SessionId";
            var responseCookie = Response.Cookies[cookieName];

            if (responseCookie != null) responseCookie.Secure = true;
        }

        protected void Session_End(object sender, EventArgs e)
        {
            if (Session["LoginID"] != null)
            {
                new DataAccess.AccountDbPrcs().Logout(3, Session["LoginID"].ToString(), Session["UserID"].ToString());
            }

            Session.Clear();
        }

    }
}
