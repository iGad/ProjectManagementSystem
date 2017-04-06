using System;
using System.Data.Entity;
using System.Linq;
using Common.Models;
using Microsoft.AspNet.Identity.EntityFramework;
using PMS.Model.Migrations;
using PMS.Model.Models.Identity;

namespace PMS.Model.Models
{
    public class ApplicationContext : IdentityDbContext<ApplicationUser, Role, string, IdentityUserLogin, UserRole, IdentityUserClaim> 
    {
        public ApplicationContext(): base("DefaultConnection")//"DefaultConnection"
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

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<WorkItem>().HasMany(x => x.Children).WithOptional().HasForeignKey(x => x.ParentId);
            modelBuilder.Entity<WorkEventUserRelation>().HasKey(x => new {x.EventId, x.UserId});
        }

        public DbSet<WorkItem> WorkItems { get; set; } 
        public DbSet<Comment> Comments { get; set; }
        public DbSet<WorkEvent> Events { get; set; }
        public DbSet<WorkEventUserRelation> EventsUsers { get; set; }

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
