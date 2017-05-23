using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using PMS.Model.Models;

namespace PMS.Model.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly ApplicationContext context;
        public UserRepository(ApplicationContext context)
        {
            this.context = context;
        }

        public IEnumerable<ApplicationUser> GetUsers(Func<ApplicationUser, bool> whereExpression)
        {
            return this.context.Users.Where(whereExpression);
        } 

        public ApplicationUser GetById(string id)
        {
            return this.context.Users.SingleOrDefault(x=>x.Id == id);
        }

        public ApplicationUser GetByUserName(string userName)
        {
            return this.context.Users.SingleOrDefault(x => !x.IsDeleted && x.UserName == userName);
        }

        public IEnumerable<ApplicationUser> GetUsersByRole(string roleName)
        {
            var role = this.context.Roles.SingleOrDefault(x => x.Name == roleName);
            if (role != null)
            {
                return this.context.Users.Include(x => x.Roles).Where(x => x.Roles.Any(r => r.RoleId == role.Id));
            }
            return new ApplicationUser[0];
        }

        public IEnumerable<ApplicationUser> GetUsersByRole(RoleType roleCode)
        {
            var role = this.context.Roles.SingleOrDefault(x => x.RoleCode == roleCode);
            if (role != null)
            {
                return this.context.Users.Include(x => x.Roles).Where(x => x.Roles.Any(r => r.RoleId == role.Id));
            }
            return new ApplicationUser[0];
        }

        public IEnumerable<ApplicationUser> GetUsersByRoles(IEnumerable<string> rolesName)
        {
            var usersIds =
                this.context.Roles.Include(x => x.Users)
                    .Where(x => rolesName.Contains(x.Name))
                    .SelectMany(x => x.Users.Select(u => u.UserId))
                    .Distinct()
                    .ToArray();
            if (usersIds.Any())
            {
                return this.context.Users.Where(x => usersIds.Contains(x.Id));
            }
            return new ApplicationUser[0];
        }

        public IEnumerable<Role> GetRoles()
        {
            return this.context.Roles;
        }

        public Role GetRoleById(string id)
        {
            return this.context.Roles.SingleOrDefault(x => x.Id == id);
        }
        

        public int SaveChanges()
        {
            return this.context.SaveChanges();
        }
    }
}
