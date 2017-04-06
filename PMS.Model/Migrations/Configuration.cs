using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations;
using System.Linq;
using PMS.Model.Models;

namespace PMS.Model.Migrations
{
    internal sealed class Configuration : DbMigrationsConfiguration<ApplicationContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
            
        }

        protected override void Seed(ApplicationContext applicationContext)
        {
            using (var transaction = applicationContext.Database.BeginTransaction())
            {
                List<string> rolesName = new List<string> { Resources.Executor, Resources.Manager, Resources.MainProjectEngineer, Resources.Director, Resources.Admin };
                for (int i = 0; i < rolesName.Count; i++)
                {
                    if (applicationContext.Roles.AsEnumerable().All(k => k.Name != rolesName[i]))
                        applicationContext.Roles.Add(new Role(rolesName[i], (RoleType) i));

                }
                applicationContext.SaveChanges();
                try
                {
                    transaction.Commit();
                }
                catch (Exception e)
                {
                    transaction.Rollback();
                }
                //var userManager = HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>();
                //var roleManager = HttpContext.Current.GetOwinContext().Get<ApplicationRoleManager>();
                //const string name = "admin@admin.ru";
                //const string password = "15935712gfdtk";
                //const string roleName = "��������";

                ////Create Role Admin if it does not exist
                //var role = roleManager.FindByName(roleName);
                //if (role == null)
                //{
                //    role = new IdentityRole(roleName);
                //    var roleresult = roleManager.Create(role);
                //}
            }

            //  This method will be called after migrating to the latest version.

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method 
            //  to avoid creating duplicate seed data. E.g.
            //
            //    ApplicationContext.People.AddOrUpdate(
            //      p => p.FullName,
            //      new Person { FullName = "Andrew Peters" },
            //      new Person { FullName = "Brice Lambson" },
            //      new Person { FullName = "Rowan Miller" }
            //    );
            //
        }
    }
}
