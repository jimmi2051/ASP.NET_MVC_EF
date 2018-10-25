namespace Domain.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class createMessage1 : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.Message", "request");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Message", "request", c => c.Int(nullable: false));
        }
    }
}
