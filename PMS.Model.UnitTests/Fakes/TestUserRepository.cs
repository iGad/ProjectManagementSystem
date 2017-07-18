using System;
using System.Collections.Generic;
using System.Linq;
using Common.Services;
using Microsoft.AspNet.Identity.EntityFramework;
using PMS.Model.Models;
using PMS.Model.Repositories;
using PMS.Model.Services;
using PMS.Model.UnitTests.Common;

namespace PMS.Model.UnitTests.Fakes
{
    public class TestUserRepository : IUserRepository, ICurrentUsernameProvider, IUsersService
    {
        private ApplicationUser _currentUser;
        public readonly List<Role> Roles = new List<Role>();
        public readonly List<ApplicationUser> Users = new List<ApplicationUser>();
        public readonly List<IdentityUserRole> UsersRoles = new List<IdentityUserRole>();

        public TestUserRepository()
        {
            List<string> rolesName = new List<string> { Resources.Manager, Resources.MainProjectEngineer, Resources.Executor, Resources.Director, Resources.Admin };
            List<RoleType> types = new List<RoleType> {RoleType.Manager, RoleType.MainProjectEngeneer, RoleType.Executor, RoleType.Director, RoleType.Admin};
            for (int i = 0; i < rolesName.Count; i++)
            {
                Roles.Add(new Role(rolesName[i], types[i]) {Id = new TGuid(20 + i + 1).ToGuid().ToString()});
            }
            
            
            const string name = "admin@admin.com";
            var admin = new ApplicationUser { UserName = name, Email = name, EmailConfirmed = true, Name = "Admin", Id = new TGuid(10).ToGuid().ToString() };
            var mainEngeneer = new ApplicationUser { UserName = "mainEngeneer", Email = "mainEngeneer", EmailConfirmed = true, Name = "Main engeneer", Id = new TGuid(1).ToGuid().ToString() };
            var director = new ApplicationUser { UserName = "director", Email = "director", EmailConfirmed = true, Name = "Director", Id = new TGuid(0).ToGuid().ToString() };
            var manager1 = new ApplicationUser { UserName = "manager1", Email = "manager1", EmailConfirmed = true, Name = "manager1", Id = new TGuid(2).ToGuid().ToString() };
            var manager2 = new ApplicationUser { UserName = "manager2", Email = "manager2", EmailConfirmed = true, Name = "manager2", Id = new TGuid(3).ToGuid().ToString() };
            var executor1 = new ApplicationUser { UserName = "executor1", Email = "executor1", EmailConfirmed = true, Name = "executor1", Id = new TGuid(4).ToGuid().ToString() };
            var executor2 = new ApplicationUser { UserName = "executor2", Email = "executor2", EmailConfirmed = true, Name = "executor2", Id = new TGuid(5).ToGuid().ToString() };
            var executor3 = new ApplicationUser { UserName = "executor3", Email = "executor3", EmailConfirmed = true, Name = "executor3", Id = new TGuid(6).ToGuid().ToString() };
            Users.AddRange(new [] {admin, mainEngeneer, director, manager1, manager2, executor3, executor1, executor2});
            UsersRoles.Add(new IdentityUserRole { UserId = mainEngeneer.Id, RoleId = Roles[1].Id });
            UsersRoles.Add(new IdentityUserRole { UserId = director.Id, RoleId = Roles[0].Id });
            UsersRoles.Add(new IdentityUserRole { UserId = director.Id, RoleId = Roles[1].Id });
            UsersRoles.Add(new IdentityUserRole { UserId = director.Id, RoleId = Roles[2].Id });
            UsersRoles.Add(new IdentityUserRole { UserId = director.Id, RoleId = Roles[3].Id });
            UsersRoles.Add(new IdentityUserRole { UserId = manager1.Id, RoleId = Roles[0].Id });
            UsersRoles.Add(new IdentityUserRole { UserId = manager2.Id, RoleId = Roles[0].Id });
            UsersRoles.Add(new IdentityUserRole { UserId = manager2.Id, RoleId = Roles[2].Id });
            UsersRoles.Add(new IdentityUserRole { UserId = executor3.Id, RoleId = Roles[2].Id });
            UsersRoles.Add(new IdentityUserRole { UserId = executor1.Id, RoleId = Roles[2].Id });
            UsersRoles.Add(new IdentityUserRole { UserId = executor2.Id, RoleId = Roles[2].Id });
            _currentUser = admin;
        }

        public int SaveChangesCall { get; private set; }
        public int SaveChanges()
        {
            SaveChangesCall++;
            return 0;
        }

        public IEnumerable<ApplicationUser> GetUsers(Func<ApplicationUser, bool> whereExpression)
        {
            return Users.Where(whereExpression);
        }

        public ApplicationUser GetById(string id)
        {
            return Users.SingleOrDefault(x => x.Id == id);
        }

        public ApplicationUser GetByUserName(string userName)
        {
            return Users.SingleOrDefault(x => x.UserName == userName);
        }

        public IEnumerable<ApplicationUser> GetUsersByRole(string roleName)
        {
            var roleId = Roles.Single(x => x.Name == roleName).Id;
            var usersRoles = UsersRoles.Where(x => x.RoleId == roleId).Select(x=>x.UserId);
            return GetUsers(x => usersRoles.Contains(x.Id));
        }

        public ApplicationUser Get(string id)
        {
            return GetById(id);
        }

        public List<ApplicationUser> GetAllowedUsersForWorkItemType(int typeId)
        {
            var type = (WorkItemType)typeId;
            var roles = new List<string> { Resources.Director, Resources.MainProjectEngineer };
            if (type == WorkItemType.Task)
            {
                roles.Add(Resources.Executor);
                roles.Add(Resources.Manager);
            }
            if (type == WorkItemType.Partition)
            {
                roles.Add(Resources.Manager);
            }
            return GetUsersByRoles(roles).Where(x => !x.IsDeleted).ToList();
        }

        public ApplicationUser GetByUsername(string username)
        {
            return GetByUserName(username);
        }

        public List<Role> GetRolesByIds(IEnumerable<string> roleIds)
        {
            return Roles.Where(x => roleIds.Contains(x.Id)).ToList();
        }

        List<ApplicationUser> IUsersService.GetUsersByRole(RoleType role)
        {
            return GetUsersByRole(role).ToList();
        }

        public ApplicationUser SafeGet(string id)
        {
            return GetById(id);
        }

        public IEnumerable<ApplicationUser> GetUsersByRole(RoleType roleCode)
        {
            var roleId = Roles.Single(x => x.RoleCode == roleCode).Id;
            var usersRoles = UsersRoles.Where(x => x.RoleId == roleId).Select(x => x.UserId);
            return GetUsers(x => usersRoles.Contains(x.Id));
        }

        public IEnumerable<Role> GetRoles()
        {
            return Roles;
        }

        public Role GetRoleById(string id)
        {
            return Roles.SingleOrDefault(x => x.Id == id);
        }

        public IEnumerable<ApplicationUser> GetUsersByRoles(IEnumerable<string> rolesName)
        {
            var rolesIds = Roles.Where(x => rolesName.Contains(x.Name)).Select(x=>x.Id);
            var usersIds = UsersRoles.Where(x => rolesIds.Contains(x.RoleId)).Select(x => x.UserId).Distinct();
            return GetUsers(x => usersIds.Contains(x.Id));
        }

        public void SetCurrentUser(string userId)
        {
            _currentUser = Users.Single(x => x.Id == userId);
        }

        public string GetCurrentUsername()
        {
            return _currentUser.UserName;
        }

        public ApplicationUser GetCurrentUser()
        {
            return _currentUser;
        }
    }
}
