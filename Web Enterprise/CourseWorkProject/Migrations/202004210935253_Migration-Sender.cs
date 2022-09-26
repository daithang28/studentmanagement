namespace CourseWorkProject.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class MigrationSender : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.Senders", "idUser");
            DropColumn("dbo.Senders", "idEvent");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Senders", "idEvent", c => c.Int(nullable: false));
            AddColumn("dbo.Senders", "idUser", c => c.Int(nullable: false));
        }
    }
}
