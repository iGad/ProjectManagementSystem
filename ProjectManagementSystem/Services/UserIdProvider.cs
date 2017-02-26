using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Common.Services;
using Microsoft.AspNet.SignalR;

namespace ProjectManagementSystem.Services
{
    public class UserIdProvider : IUserIdProvider, ICurrentUsernameProvider
    {
        public string GetUserId(IRequest request)
        {
            return HttpContext.Current.User.Identity.Name;
        }

        public string GetCurrentUsername()
        {
            return HttpContext.Current.User.Identity.Name;
        }
    }
}