namespace Domain.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class createMessage : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Message",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Sendby = c.String(nullable: false),
                        Displayname = c.String(),
                        Content = c.String(),
                        Created = c.DateTime(nullable: false),
                        status = c.Int(nullable: false),
                        request = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.Message");
        }
    }
}
