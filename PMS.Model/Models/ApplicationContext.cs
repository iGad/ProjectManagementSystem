using System;
using System.Data.Entity;
using System.Linq;
using Microsoft.AspNet.Identity.EntityFramework;
using PMS.Model.Models.Identity;

namespace PMS.Model.Models
{
    public class ApplicationContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationContext(): base("DefaultConnection", throwIfV1Schema: false)//"DefaultConnection"
        {
            if (!Database.Exists())
            {
                Database.SetInitializer(new ApplicationDbInitializer());
            }
           
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
