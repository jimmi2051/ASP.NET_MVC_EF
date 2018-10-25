namespace Domain.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class NewUpdate : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Order_Detail", "Price", c => c.Single(nullable: false));
            AlterColumn("dbo.Receipt_Note", "Amount", c => c.Single(nullable: false));
            AlterColumn("dbo.Receipt_Note_Detail", "Price", c => c.Single(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Receipt_Note_Detail", "Price", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AlterColumn("dbo.Receipt_Note", "Amount", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AlterColumn("dbo.Order_Detail", "Price", c => c.Decimal(nullable: false, precision: 18, scale: 2));
        }
    }
}
