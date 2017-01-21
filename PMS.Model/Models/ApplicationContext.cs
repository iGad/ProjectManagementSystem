using System;
using System.Data.Entity;
using System.Linq;
using Microsoft.AspNet.Identity.EntityFramework;
using PMS.Model.Migrations;
using PMS.Model.Models.Identity;

namespace PMS.Model.Models
{
    public class ApplicationContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationContext(): base("DefaultConnection", throwIfV1Schema: false)//"DefaultConnection"
        {
            RequireUniqueEmail = false;
            if (!Database.Exists())
            {
                Database.SetInitializer(new ApplicationDbInitializer());
            }
            else
            {
                Database.SetInitializer(new MigrateDatabaseToLatestVersion<ApplicationContext,Configuration>());
            }
           Database.Initialize(false);
        }

        public DbSet<WorkItem> WorkItems { get; set; } 
        public DbSet<Comment> Comments { get; set; }

        public static ApplicationContext Create()
        {
            return new ApplicationContext();
        }

        public override int SaveChanges()
        {
            ChangeTracker.DetectChanges();
            var changedEntities = ChangeTracker.Entries().ToArray();
            if (changedEntities.Any())
            {
                foreach (var changedEntity in changedEntities)
                {
                    var entity = changedEntity.Entity as Entity;
                    if(entity != null)
                        entity.UpdatedDate = DateTime.UtcNow;
                }
            }
            return base.SaveChanges();
        }
        
    }
}
