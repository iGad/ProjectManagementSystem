using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using PMS.Model.Models;

namespace PMS.Model.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly ApplicationContext _context;
        public UserRepository(ApplicationContext context)
        {
            _context = context;
        }

        public IEnumerable<ApplicationUser> GetUsers(Expression<Func<ApplicationUser, bool>> whereExpression)
        {
            return _context.Users.Where(whereExpression);
        }

        public async Task<List<ApplicationUser>> GetUsersAsync(Expression<Func<ApplicationUser, bool>> whereExpression)
        {
            return await _context.Users.Where(whereExpression).ToListAsync();
        }


        public ApplicationUser GetById(string id)
        {
            return _context.Users.SingleOrDefault(x=>x.Id == id);
        }

        public async Task<ApplicationUser> GetByIdAsync(string id)
        {
            return await _context.Users.SingleOrDefaultAsync(x => x.Id == id);
        }

        public ApplicationUser GetByUserName(string userName)
        {
            return _context.Users.SingleOrDefault(x => !x.IsDeleted && x.UserName == userName);
        }

        public async Task<ApplicationUser> GetByUserNameAsync(string userName)
        {
            return  await _context.Users.SingleOrDefaultAsync(x => !x.IsDeleted && x.UserName == userName);
        }

        public IEnumerable<ApplicationUser> GetUsersByRole(string roleName)
        {
            var role = _context.Roles.SingleOrDefault(x => x.Name == roleName);
            if (role != null)
            {
                return _context.Users.Include(x => x.Roles).Where(x => x.Roles.Any(r => r.RoleId == role.Id));
            }
            return new ApplicationUser[0];
        }

        public IEnumerable<ApplicationUser> GetUsersByRole(RoleType roleCode)
        {
            var role = _context.Roles.SingleOrDefault(x => x.RoleCode == roleCode);
            if (role != null)
            {
                return _context.Users.Include(x => x.Roles).Where(x => x.Roles.Any(r => r.RoleId == role.Id));
            }
            return new ApplicationUser[0];
        }

        public IEnumerable<ApplicationUser> GetUsersByRoles(IEnumerable<string> rolesName)
        {
            var usersIds =
                _context.Roles.Include(x => x.Users)
                    .Where(x => rolesName.Contains(x.Name))
                    .SelectMany(x => x.Users.Select(u => u.UserId))
                    .Distinct()
                    .ToArray();
            if (usersIds.Any())
            {
                return _context.Users.Where(x => usersIds.Contains(x.Id));
            }
            return new ApplicationUser[0];
        }

        public IEnumerable<Role> GetRoles()
        {
            return _context.Roles;
        }

        public Role GetRoleById(string id)
        {
            return _context.Roles.SingleOrDefault(x => x.Id == id);
        }
        

        public int SaveChanges()
        {
            return _context.SaveChanges();
        }

        public async Task<int> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync();
        }
    }
}
