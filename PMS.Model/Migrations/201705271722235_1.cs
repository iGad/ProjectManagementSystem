namespace PMS.Model.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _1 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Comments", "WorkItemId", "dbo.WorkItems");
            DropIndex("dbo.Comments", new[] { "WorkItemId" });
            DropColumn("dbo.Comments", "WorkItemId");
            CreateTable(
                "dbo.Settings",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Type = c.Int(nullable: false),
                        ValueType = c.Int(nullable: false),
                        Value = c.String(),
                        DefaultValue = c.String(),
                        ValueRegex = c.String(),
                        MinValue = c.Int(),
                        MaxValue = c.Int(),
                        Name = c.String(nullable: false),
                        CreatedDate = c.DateTime(nullable: false),
                        UpdatedDate = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.UserSettings",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ValueType = c.Int(nullable: false),
                        Type = c.Int(nullable: false),
                        DefaultValue = c.String(),
                        Name = c.String(nullable: false),
                        CreatedDate = c.DateTime(nullable: false),
                        UpdatedDate = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.UserSettingValues",
                c => new
                    {
                        UserSettingId = c.Int(nullable: false),
                        UserId = c.String(nullable: false, maxLength: 128),
                        Value = c.String(),
                    })
                .PrimaryKey(t => new { t.UserSettingId, t.UserId })
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .ForeignKey("dbo.UserSettings", t => t.UserSettingId, cascadeDelete: true)
                .Index(t => t.UserSettingId)
                .Index(t => t.UserId);
            
            AddColumn("dbo.Comments", "ObjectId", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.UserSettingValues", "UserSettingId", "dbo.UserSettings");
            DropForeignKey("dbo.UserSettingValues", "UserId", "dbo.AspNetUsers");
            DropIndex("dbo.UserSettingValues", new[] { "UserId" });
            DropIndex("dbo.UserSettingValues", new[] { "UserSettingId" });
            DropIndex("dbo.Comments", new[] { "WorkItem_Id" });
            AlterColumn("dbo.Comments", "WorkItem_Id", c => c.Int(nullable: false));
            DropColumn("dbo.Comments", "ObjectId");
            DropTable("dbo.UserSettingValues");
            DropTable("dbo.UserSettings");
            DropTable("dbo.Settings");
            AddColumn("dbo.Comments","WorkItemId", c=>c.Int(nullable:false));
            CreateIndex("dbo.Comments", "WorkItemId");
            AddForeignKey("dbo.Comments", "WorkItemId", "dbo.WorkItems", "Id", cascadeDelete: true);
        }
    }
}
