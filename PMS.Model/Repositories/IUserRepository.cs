using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity.EntityFramework;
using PMS.Model.Models;

namespace PMS.Model.Repositories
{
    public interface IUserRepository : IRepository
    {
        IEnumerable<ApplicationUser> GetUsers(Func<ApplicationUser, bool> whereExpression);
        Task<List<ApplicationUser>> GetUsersAsync(Func<ApplicationUser, bool> whereExpression);
        ApplicationUser GetById(string id);
        Task<ApplicationUser> GetByIdAsync(string id);
        ApplicationUser GetByUserName(string userName);
        Task<ApplicationUser> GetByUserNameAsync(string userName);
        IEnumerable<ApplicationUser> GetUsersByRole(string roleName);
        IEnumerable<ApplicationUser> GetUsersByRole(RoleType roleCode);
        IEnumerable<Role> GetRoles();
        Role GetRoleById(string id);
        IEnumerable<ApplicationUser> GetUsersByRoles(IEnumerable<string> rolesName);
    }
}