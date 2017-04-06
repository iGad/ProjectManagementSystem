using System;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using Microsoft.AspNet.SignalR;
using ProjectManagementSystem.Export;
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
            //this.Error+= OnError;
            //this.AuthorizeRequest += OnAuthorizeRequest;
        }

        private void OnAuthorizeRequest(object sender, EventArgs eventArgs)
        {
            var i = 0;
        }

        private void OnError(object sender, EventArgs eventArgs)
        {
            
        }
    }
}
