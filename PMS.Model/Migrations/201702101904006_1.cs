namespace PMS.Model.Migrations
{
    using System.Data.Entity.Migrations;
    
    public partial class _1 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.WorkItems", "FinishDate", c => c.DateTime());
            AddForeignKey("dbo.WorkItems", "ParentId", "dbo.WorkItems", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.WorkItems", "ParentId", "dbo.WorkItems");
            DropColumn("dbo.WorkItems", "FinishDate");
        }
    }
}
