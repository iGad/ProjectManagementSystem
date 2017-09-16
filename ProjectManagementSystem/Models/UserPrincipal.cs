using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Security.Principal;
using System.Web;
using PMS.Model.Models;
using PMS.Model.Services;

namespace ProjectManagementSystem.Models
{
    public class UserPrincipal : IPrincipal
    {
        private readonly string _username;

        public UserPrincipal(string username)
        {
            _username = username;
        }

        public bool IsInRole(string role)
        {
            using (var context = new ApplicationContext())
            {
                return context.Users.Include(x => x.Roles)
                    .Where(x => x.UserName.Equals(_username, StringComparison.OrdinalIgnoreCase))
                    .SelectMany(x => x.Roles)
                    .Join(context.Roles, ur => ur.RoleId, r => r.Id, (ur, r) => r)
                    .Any(x => x.Name.Equals(role, StringComparison.OrdinalIgnoreCase));
            }
        }

        public bool IsInAnyRole(RoleType[] roleTypes)
        {
            using (var context = new ApplicationContext())
            {
                return context.Users.Include(x => x.Roles)
                    .Where(x => x.UserName.Equals(_username, StringComparison.OrdinalIgnoreCase))
                    .SelectMany(x => x.Roles)
                    .Join(context.Roles, ur => ur.RoleId, r => r.Id, (ur, r) => r)
                    .Any(x => roleTypes.Contains(x.RoleCode));
            }
        }

        public IIdentity Identity => new UserIdentity(_username);
    }
}