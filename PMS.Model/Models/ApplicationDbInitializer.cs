using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Diagnostics;
using System.Linq;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;

namespace PMS.Model.Models.Identity
{
    public class ApplicationDbInitializer : CreateDatabaseIfNotExists<ApplicationContext>
    {
        protected override void Seed(ApplicationContext applicationContext)
        {
            AddRoles(applicationContext);
            InitializeIdentityForEF(applicationContext);
            base.Seed(applicationContext);
        }

        private void AddRoles(ApplicationContext applicationContext)
        {
            
        }

        //Create User=Admin@Admin.com with password=Admin@123456 in the Admin role        
        public static void InitializeIdentityForEF(ApplicationContext applicationContext)
        {
            using (var transaction = applicationContext.Database.BeginTransaction())
            {
                var owinContext = new OwinContext();
                owinContext.Set(applicationContext);
                var userManager = ApplicationUserManager.Create(new IdentityFactoryOptions<ApplicationUserManager>(), owinContext);
                var roleManager = ApplicationRoleManager.Create(new IdentityFactoryOptions<ApplicationRoleManager>(), owinContext);
                List<string> rolesName = new List<string>() { "Руководитель направления", "Главный инженер проекта", "Исполнитель", "Директор", "Администратор" };
                for (int i = 0; i < rolesName.Count; i++)
                {
                    var role = roleManager.FindByName(rolesName[i]);
                    if (role == null)
                    {
                        role = new IdentityRole(rolesName[i]);
                        roleManager.Create(role);
                    }

                }
                applicationContext.SaveChanges();
                
                const string name = "admin@admin.com";
                const string password = "admin@123456";
                var admin = userManager.FindByName(name);
                if (admin == null)
                {
                    admin = new ApplicationUser {UserName = name, Email = name, EmailConfirmed = true, Name="Admin"};
                    userManager.Create(admin, password);
                    userManager.SetLockoutEnabled(admin.Id, false);
                    userManager.AddToRole(admin.Id, rolesName.Last());
                }
                try
                {
                    transaction.Commit();
                }
                catch
                {
                    transaction.Rollback();
                }
                
                //var userManager = HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>();
                //var roleManager = HttpContext.Current.GetOwinContext().Get<ApplicationRoleManager>();
                //const string name = "admin@admin.ru";
                //const string password = "15935712gfdtk";
                //const string roleName = "Директор";

                ////Create Role Admin if it does not exist
                //var role = roleManager.FindByName(roleName);
                //if (role == null)
                //{
                //    role = new IdentityRole(roleName);
                //    var roleresult = roleManager.Create(role);
                //}
            }
           
           
            //const string roleName = "Директор";
            //List<string> rolesName = new List<string>() { "Руководитель направления", "Главный инженер проекта", "Исполнитель", "Администратор", "Директор" };
            //IdentityRole role = null;
            //for (int i = 0; i < rolesName.Count; i++)
            //{
            //    role = roleManager.FindByName(rolesName[i]);
            //    if (role == null)
            //    {
            //        role = new IdentityRole(rolesName[i]);
            //        var roleresult = roleManager.Create(role);
            //    }
            //}
            //Create Role Admin if it does not exist
            //var role = roleManager.FindByName(roleName);
            //if (role == null) {
            //    role = new IdentityRole(roleName);
            //    var roleresult = roleManager.Create(role);
            //}



            //var user = userManager.FindByName(name);
            //if (user == null) {
            //    user = new ApplicationUser { UserName = name, Email = name, Name = "Админ", Surname = "Админ", Birthday = DateTime.Parse("1990-01-01")};
            //    var result = userManager.Create(user, password);
            //    result = userManager.SetLockoutEnabled(user.Id, false);
            //}

            //// Add user admin to Role Admin if not already added
            //var rolesForUser = userManager.GetRoles(user.Id);
            //if (!rolesForUser.Contains(role.Name)) {
            //    var result = userManager.AddToRole(user.Id, role.Name);
            //}
        }
    }
}