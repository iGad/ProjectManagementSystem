using System;
using System.Collections.Generic;
using Microsoft.AspNet.Identity.EntityFramework;
using PMS.Model.Models;

namespace PMS.Model.Repositories
{
    public interface IUserRepository : IRepository
    {
        IEnumerable<ApplicationUser> GetUsers(Func<ApplicationUser, bool> whereExpression);
        ApplicationUser GetById(string id);
        ApplicationUser GetByUserName(string userName);
        IEnumerable<ApplicationUser> GetUsersByRole(string roleName);
        IEnumerable<ApplicationUser> GetUsersByRole(RoleType roleCode);
        IEnumerable<Role> GetRoles();
        Role GetRoleById(string id);
        IEnumerable<ApplicationUser> GetUsersByRoles(IEnumerable<string> rolesName);
    }
}