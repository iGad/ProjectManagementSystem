using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ProjectManagementSystem.Models;

namespace ProjectManagementSystem.Infrastructure
{
    public class SecureModule : IHttpModule 
    {
        public void Init(HttpApplication context)
        {
            context.AuthenticateRequest += Context_AuthenticateRequest;
        }

        /// <summary>
        /// Обрабатывает событие аутентификации приложения
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Context_AuthenticateRequest(object sender, EventArgs e)
        {
            var httpContext = ((HttpApplication)sender).Context;
            var user = httpContext.User;

            if (user?.Identity == null)
                return;

            if (!user.Identity.IsAuthenticated)
                return;

            httpContext.User = new UserPrincipal(user.Identity.Name);
        }

        public void Dispose()
        {
            if (HttpContext.Current == null)
            {
                return;
            }

            if (HttpContext.Current.ApplicationInstance == null)
            {
                return;
            }

            HttpContext.Current.ApplicationInstance.AuthorizeRequest -= Context_AuthenticateRequest;
        }
    }
}