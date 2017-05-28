namespace PMS.Model.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _2 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Comments", "UpdatedDate", c => c.DateTime());
            AlterColumn("dbo.WorkEvents", "UpdatedDate", c => c.DateTime());
            AlterColumn("dbo.Settings", "UpdatedDate", c => c.DateTime());
            AlterColumn("dbo.UserSettings", "UpdatedDate", c => c.DateTime());
            AlterColumn("dbo.WorkItems", "UpdatedDate", c => c.DateTime());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.WorkItems", "UpdatedDate", c => c.DateTime(nullable: false));
            AlterColumn("dbo.UserSettings", "UpdatedDate", c => c.DateTime(nullable: false));
            AlterColumn("dbo.Settings", "UpdatedDate", c => c.DateTime(nullable: false));
            AlterColumn("dbo.WorkEvents", "UpdatedDate", c => c.DateTime(nullable: false));
            AlterColumn("dbo.Comments", "UpdatedDate", c => c.DateTime(nullable: false));
        }
    }
}
