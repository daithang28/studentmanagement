namespace CourseWorkProject.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class MigrationV2 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Senders",
                c => new
                    {
                        id = c.Int(nullable: false, identity: true),
                        year = c.Int(nullable: false),
                        isSend = c.Boolean(nullable: false),
                        idUser = c.Int(nullable: false),
                        idEvent = c.Int(nullable: false),
                        Event_id = c.Int(),
                        User_id = c.Int(),
                    })
                .PrimaryKey(t => t.id)
                .ForeignKey("dbo.Events", t => t.Event_id)
                .ForeignKey("dbo.Users", t => t.User_id)
                .Index(t => t.Event_id)
                .Index(t => t.User_id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Senders", "User_id", "dbo.Users");
            DropForeignKey("dbo.Senders", "Event_id", "dbo.Events");
            DropIndex("dbo.Senders", new[] { "User_id" });
            DropIndex("dbo.Senders", new[] { "Event_id" });
            DropTable("dbo.Senders");
        }
    }
}
