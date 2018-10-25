namespace Domain.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class update : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Product", "Created", c => c.DateTime());
            AlterColumn("dbo.Product", "Modified", c => c.DateTime());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Product", "Modified", c => c.DateTime(nullable: false));
            AlterColumn("dbo.Product", "Created", c => c.DateTime(nullable: false));
        }
    }
}
