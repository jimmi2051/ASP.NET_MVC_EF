namespace Domain.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class xinchao : DbMigration
    {
        public override void Up()
        {
            DropTable("dbo.Message");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.Message",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Sendby = c.String(nullable: false),
                        Displayname = c.String(),
                        Content = c.String(),
                        Created = c.DateTime(nullable: false),
                        status = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ID);           
        }
    }
}
