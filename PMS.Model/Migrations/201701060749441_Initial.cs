using System.Data.Entity.Migrations;

namespace PMS.Model.Migrations
{
    public partial class Initial : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Comments",
                c => new
                    {
                        Id = c.Int(false, true),
                        WorkItemId = c.Int(false),
                        Text = c.String(),
                        UserId = c.String(maxLength: 128),
                        CreatedDate = c.DateTime(false),
                        UpdatedDate = c.DateTime(false)
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.UserId)
                .ForeignKey("dbo.WorkItems", t => t.WorkItemId, true)
                .Index(t => t.WorkItemId)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.AspNetUsers",
                c => new
                    {
                        Id = c.String(false, 128),
                        Name = c.String(false),
                        Surname = c.String(),
                        Fathername = c.String(),
                        Birthday = c.DateTime(),
                        IsDeleted = c.Boolean(false),
                        Email = c.String(maxLength: 256),
                        EmailConfirmed = c.Boolean(false),
                        PasswordHash = c.String(),
                        SecurityStamp = c.String(),
                        PhoneNumber = c.String(),
                        PhoneNumberConfirmed = c.Boolean(false),
                        TwoFactorEnabled = c.Boolean(false),
                        LockoutEndDateUtc = c.DateTime(),
                        LockoutEnabled = c.Boolean(false),
                        AccessFailedCount = c.Int(false),
                        UserName = c.String(false, 256)
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.UserName, unique: true, name: "UserNameIndex");
            
            CreateTable(
                "dbo.AspNetUserClaims",
                c => new
                    {
                        Id = c.Int(false, true),
                        UserId = c.String(false, 128),
                        ClaimType = c.String(),
                        ClaimValue = c.String()
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, true)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.AspNetUserLogins",
                c => new
                    {
                        LoginProvider = c.String(false, 128),
                        ProviderKey = c.String(false, 128),
                        UserId = c.String(false, 128)
                    })
                .PrimaryKey(t => new { t.LoginProvider, t.ProviderKey, t.UserId })
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, true)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.AspNetUserRoles",
                c => new
                    {
                        UserId = c.String(false, 128),
                        RoleId = c.String(false, 128)
                    })
                .PrimaryKey(t => new { t.UserId, t.RoleId })
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, true)
                .ForeignKey("dbo.AspNetRoles", t => t.RoleId, true)
                .Index(t => t.UserId)
                .Index(t => t.RoleId);
            
            CreateTable(
                "dbo.WorkItems",
                c => new
                    {
                        Id = c.Int(false, true),
                        ParentId = c.Int(),
                        Type = c.Int(false),
                        Description = c.String(),
                        CreatorId = c.String(false, 128),
                        ExecutorId = c.String(maxLength: 128),
                        State = c.Int(false),
                        Status = c.Int(false),
                        DeadLine = c.DateTime(false),
                        Name = c.String(false),
                        CreatedDate = c.DateTime(false),
                        UpdatedDate = c.DateTime(false)
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.CreatorId, true)
                .ForeignKey("dbo.AspNetUsers", t => t.ExecutorId)
                .ForeignKey("dbo.WorkItems", t => t.ParentId)
                .Index(t => t.ParentId)
                .Index(t => t.CreatorId)
                .Index(t => t.ExecutorId);
            
            CreateTable(
                "dbo.AspNetRoles",
                c => new
                    {
                        Id = c.String(false, 128),
                        Name = c.String(false, 256)
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.Name, unique: true, name: "RoleNameIndex");
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.AspNetUserRoles", "RoleId", "dbo.AspNetRoles");
            DropForeignKey("dbo.WorkItems", "ParentId", "dbo.WorkItems");
            DropForeignKey("dbo.WorkItems", "ExecutorId", "dbo.AspNetUsers");
            DropForeignKey("dbo.WorkItems", "CreatorId", "dbo.AspNetUsers");
            DropForeignKey("dbo.Comments", "WorkItemId", "dbo.WorkItems");
            DropForeignKey("dbo.Comments", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserRoles", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserLogins", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserClaims", "UserId", "dbo.AspNetUsers");
            DropIndex("dbo.AspNetRoles", "RoleNameIndex");
            DropIndex("dbo.WorkItems", new[] { "ExecutorId" });
            DropIndex("dbo.WorkItems", new[] { "CreatorId" });
            DropIndex("dbo.WorkItems", new[] { "ParentId" });
            DropIndex("dbo.AspNetUserRoles", new[] { "RoleId" });
            DropIndex("dbo.AspNetUserRoles", new[] { "UserId" });
            DropIndex("dbo.AspNetUserLogins", new[] { "UserId" });
            DropIndex("dbo.AspNetUserClaims", new[] { "UserId" });
            DropIndex("dbo.AspNetUsers", "UserNameIndex");
            DropIndex("dbo.Comments", new[] { "UserId" });
            DropIndex("dbo.Comments", new[] { "WorkItemId" });
            DropTable("dbo.AspNetRoles");
            DropTable("dbo.WorkItems");
            DropTable("dbo.AspNetUserRoles");
            DropTable("dbo.AspNetUserLogins");
            DropTable("dbo.AspNetUserClaims");
            DropTable("dbo.AspNetUsers");
            DropTable("dbo.Comments");
        }
    }
}
