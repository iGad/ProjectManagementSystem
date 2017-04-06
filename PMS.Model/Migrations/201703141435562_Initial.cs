namespace PMS.Model.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Initial : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Comments",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        WorkItemId = c.Int(nullable: false),
                        Text = c.String(),
                        UserId = c.String(maxLength: 128),
                        CreatedDate = c.DateTime(nullable: false),
                        UpdatedDate = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.UserId)
                .ForeignKey("dbo.WorkItems", t => t.WorkItemId, cascadeDelete: true)
                .Index(t => t.WorkItemId)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.AspNetUsers",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Name = c.String(nullable: false),
                        Surname = c.String(),
                        Fathername = c.String(),
                        Birthday = c.DateTime(),
                        IsDeleted = c.Boolean(nullable: false),
                        Email = c.String(maxLength: 256),
                        EmailConfirmed = c.Boolean(nullable: false),
                        PasswordHash = c.String(),
                        SecurityStamp = c.String(),
                        PhoneNumber = c.String(),
                        PhoneNumberConfirmed = c.Boolean(nullable: false),
                        TwoFactorEnabled = c.Boolean(nullable: false),
                        LockoutEndDateUtc = c.DateTime(),
                        LockoutEnabled = c.Boolean(nullable: false),
                        AccessFailedCount = c.Int(nullable: false),
                        UserName = c.String(nullable: false, maxLength: 256),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.UserName, unique: true, name: "UserNameIndex");
            
            CreateTable(
                "dbo.AspNetUserClaims",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UserId = c.String(nullable: false, maxLength: 128),
                        ClaimType = c.String(),
                        ClaimValue = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.AspNetUserLogins",
                c => new
                    {
                        LoginProvider = c.String(nullable: false, maxLength: 128),
                        ProviderKey = c.String(nullable: false, maxLength: 128),
                        UserId = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.LoginProvider, t.ProviderKey, t.UserId })
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.AspNetUserRoles",
                c => new
                    {
                        UserId = c.String(nullable: false, maxLength: 128),
                        RoleId = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.UserId, t.RoleId })
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .ForeignKey("dbo.AspNetRoles", t => t.RoleId, cascadeDelete: true)
                .Index(t => t.UserId)
                .Index(t => t.RoleId);
            
            CreateTable(
                "dbo.WorkItems",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ParentId = c.Int(),
                        Type = c.Int(nullable: false),
                        Description = c.String(),
                        CreatorId = c.String(nullable: false, maxLength: 128),
                        ExecutorId = c.String(maxLength: 128),
                        State = c.Int(nullable: false),
                        Status = c.Int(nullable: false),
                        DeadLine = c.DateTime(nullable: false),
                        FinishDate = c.DateTime(),
                        Name = c.String(nullable: false),
                        CreatedDate = c.DateTime(nullable: false),
                        UpdatedDate = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.WorkItems", t => t.ParentId)
                .ForeignKey("dbo.AspNetUsers", t => t.CreatorId, cascadeDelete: true)
                .ForeignKey("dbo.AspNetUsers", t => t.ExecutorId)
                .Index(t => t.ParentId)
                .Index(t => t.CreatorId)
                .Index(t => t.ExecutorId);
            
            CreateTable(
                "dbo.WorkEvents",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UserId = c.String(maxLength: 128),
                        ObjectId = c.Int(),
                        ObjectStringId = c.String(),
                        Type = c.Int(nullable: false),
                        Data = c.String(),
                        CreatedDate = c.DateTime(nullable: false),
                        UpdatedDate = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.UserId)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.WorkEventUserRelations",
                c => new
                    {
                        EventId = c.Int(nullable: false),
                        UserId = c.String(nullable: false, maxLength: 128),
                        State = c.Int(nullable: false),
                        IsFavorite = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => new { t.EventId, t.UserId })
                .ForeignKey("dbo.WorkEvents", t => t.EventId, cascadeDelete: true)
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.EventId)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.AspNetRoles",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        RoleCode = c.Int(nullable: false),
                        Name = c.String(nullable: false, maxLength: 256),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.Name, unique: true, name: "RoleNameIndex");
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.AspNetUserRoles", "RoleId", "dbo.AspNetRoles");
            DropForeignKey("dbo.WorkEventUserRelations", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.WorkEventUserRelations", "EventId", "dbo.WorkEvents");
            DropForeignKey("dbo.WorkEvents", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.WorkItems", "ExecutorId", "dbo.AspNetUsers");
            DropForeignKey("dbo.WorkItems", "CreatorId", "dbo.AspNetUsers");
            DropForeignKey("dbo.Comments", "WorkItemId", "dbo.WorkItems");
            DropForeignKey("dbo.WorkItems", "ParentId", "dbo.WorkItems");
            DropForeignKey("dbo.Comments", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserRoles", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserLogins", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserClaims", "UserId", "dbo.AspNetUsers");
            DropIndex("dbo.AspNetRoles", "RoleNameIndex");
            DropIndex("dbo.WorkEventUserRelations", new[] { "UserId" });
            DropIndex("dbo.WorkEventUserRelations", new[] { "EventId" });
            DropIndex("dbo.WorkEvents", new[] { "UserId" });
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
            DropTable("dbo.WorkEventUserRelations");
            DropTable("dbo.WorkEvents");
            DropTable("dbo.WorkItems");
            DropTable("dbo.AspNetUserRoles");
            DropTable("dbo.AspNetUserLogins");
            DropTable("dbo.AspNetUserClaims");
            DropTable("dbo.AspNetUsers");
            DropTable("dbo.Comments");
        }
    }
}
