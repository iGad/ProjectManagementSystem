using System;
using System.Text;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using Microsoft.AspNet.SignalR;
using Newtonsoft.Json;
using ProjectManagementSystem.Export;
using ProjectManagementSystem.Infrastructure;
using ProjectManagementSystem.Services;

namespace ProjectManagementSystem
{
    public class MvcApplication : HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            GlobalConfiguration.Configure(WebApiConfig.Register);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            var dependencyResolver = new PmsDependencyResolver();
            DependencyResolver.SetResolver(dependencyResolver);
            //GlobalHost.DependencyResolver.Register(typeof(IUserIdProvider), ()=> new UserIdProvider());
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            ViewEngines.Engines.Clear();
            ViewEngines.Engines.Add(new PmsViewEngine());
            //this.Error+= OnError;
            //this.AuthorizeRequest += OnAuthorizeRequest;
        }

        private void OnAuthorizeRequest(object sender, EventArgs eventArgs)
        {
            var i = 0;
        }

        void Application_Error(object sender, EventArgs e)
        {
            Exception exc = Server.GetLastError();
            if (exc != null)
            {
                Response.StatusCode = 500;
                Response.Write(JsonConvert.SerializeObject(exc));
                //Response.StatusDescription = exc.Message;
                //Response.Clear();
                Server.ClearError();
            }
        }

        private void OnError(object sender, EventArgs eventArgs)
        {
            Exception exc = Server.GetLastError();
            if ((exc.InnerException != null && exc.InnerException.InnerException.Message == "Лицензионный ключ программы не найден!") || exc.Message == "Лицензионный ключ программы не найден!")
            {
                //Response.Clear();
                Server.ClearError();
            }
        }
    }
}
