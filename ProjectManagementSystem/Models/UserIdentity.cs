using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Security.Principal;
using System.Web.Security;
using PMS.Model.Models;

namespace ProjectManagementSystem.Models
{
    public class UserIdentity : IIdentity
    {
        private readonly ApplicationUser _user;
        public UserIdentity(string username)
        {
            _user = GetUser(username);
        }

        private ApplicationUser GetUser(string username)
        {
            using (var context = new ApplicationContext())
            {
                return context.Users.Include(x => x.Roles).SingleOrDefault(x => x.UserName == username);
            }
        }

        public string Name => _user?.UserName ?? "";
        public string AuthenticationType =>  FormsAuthentication.IsEnabled ? "Forms" : "Application cookie";
        public bool IsAuthenticated => _user != null;

        public override string ToString()
        {
            return Name;
        }
    }
}