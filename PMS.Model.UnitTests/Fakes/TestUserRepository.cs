using System;
using System.Collections.Generic;
using System.Linq;
using Common.Services;
using Microsoft.AspNet.Identity.EntityFramework;
using PMS.Model.Models;
using PMS.Model.Repositories;
using PMS.Model.UnitTests.Common;

namespace PMS.Model.UnitTests.Fakes
{
    public class TestUserRepository : IUserRepository, ICurrentUsernameProvider
    {
        private ApplicationUser currentUser;
        public readonly List<IdentityRole> Roles = new List<IdentityRole>();
        public readonly List<ApplicationUser> Users = new List<ApplicationUser>();
        public readonly List<IdentityUserRole> UsersRoles = new List<IdentityUserRole>();

        public TestUserRepository()
        {
            List<string> rolesName = new List<string> { Resources.Manager, Resources.MainProjectEngineer, Resources.Executor, Resources.Director, Resources.Admin };
            for (int i = 0; i < rolesName.Count; i++)
            {
                this.Roles.Add(new IdentityRole(rolesName[i]) {Id = new TGuid(20 + i + 1).ToGuid().ToString()});
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
            this.Users.AddRange(new [] {admin, mainEngeneer, director, manager1, manager2, executor3, executor1, executor2});
            this.UsersRoles.Add(new IdentityUserRole { UserId = mainEngeneer.Id, RoleId = this.Roles[1].Id });
            this.UsersRoles.Add(new IdentityUserRole { UserId = director.Id, RoleId = this.Roles[0].Id });
            this.UsersRoles.Add(new IdentityUserRole { UserId = director.Id, RoleId = this.Roles[1].Id });
            this.UsersRoles.Add(new IdentityUserRole { UserId = director.Id, RoleId = this.Roles[2].Id });
            this.UsersRoles.Add(new IdentityUserRole { UserId = director.Id, RoleId = this.Roles[3].Id });
            this.UsersRoles.Add(new IdentityUserRole { UserId = manager1.Id, RoleId = this.Roles[0].Id });
            this.UsersRoles.Add(new IdentityUserRole { UserId = manager2.Id, RoleId = this.Roles[0].Id });
            this.UsersRoles.Add(new IdentityUserRole { UserId = manager2.Id, RoleId = this.Roles[2].Id });
            this.UsersRoles.Add(new IdentityUserRole { UserId = executor3.Id, RoleId = this.Roles[2].Id });
            this.UsersRoles.Add(new IdentityUserRole { UserId = executor1.Id, RoleId = this.Roles[2].Id });
            this.UsersRoles.Add(new IdentityUserRole { UserId = executor2.Id, RoleId = this.Roles[2].Id });
            this.currentUser = admin;
        }

        public int SaveChangesCall { get; private set; }
        public int SaveChanges()
        {
            SaveChangesCall++;
            return 0;
        }

        public IEnumerable<ApplicationUser> GetUsers(Func<ApplicationUser, bool> whereExpression)
        {
            return this.Users.Where(whereExpression);
        }

        public ApplicationUser GetById(string id)
        {
            return this.Users.SingleOrDefault(x => x.Id == id);
        }

        public ApplicationUser GetByUserName(string userName)
        {
            return this.Users.SingleOrDefault(x => x.UserName == userName);
        }

        public IEnumerable<ApplicationUser> GetUsersByRole(string roleName)
        {
            var roleId = this.Roles.Single(x => x.Name == roleName).Id;
            var usersRoles = this.UsersRoles.Where(x => x.RoleId == roleId).Select(x=>x.UserId);
            return GetUsers(x => usersRoles.Contains(x.Id));
        }

        public IEnumerable<IdentityRole> GetRoles()
        {
            return this.Roles;
        }

        public IdentityRole GetRoleById(string id)
        {
            return this.Roles.SingleOrDefault(x => x.Id == id);
        }

        public IEnumerable<ApplicationUser> GetUsersByRoles(IEnumerable<string> rolesName)
        {
            var rolesIds = this.Roles.Where(x => rolesName.Contains(x.Name)).Select(x=>x.Id);
            var usersIds = this.UsersRoles.Where(x => rolesIds.Contains(x.RoleId)).Select(x => x.UserId).Distinct();
            return GetUsers(x => usersIds.Contains(x.Id));
        }

        public void SetCurrentUser(string userId)
        {
            this.currentUser = this.Users.Single(x => x.Id == userId);
        }

        public string GetCurrentUsername()
        {
            return this.currentUser.UserName;
        }
    }
}
