namespace PMS.Model.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _4 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.UserSettings", "Regex", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.UserSettings", "Regex");
        }
    }
}
