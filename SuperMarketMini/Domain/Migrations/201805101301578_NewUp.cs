namespace Domain.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class NewUp : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Category", "GroupName", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Category", "GroupName");
        }
    }
}
