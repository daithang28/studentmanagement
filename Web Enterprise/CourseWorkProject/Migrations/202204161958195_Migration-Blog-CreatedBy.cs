namespace CourseWorkProject.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class MigrationBlogCreatedBy : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Blogs", "CreatedBy_id", c => c.Int());
            CreateIndex("dbo.Blogs", "CreatedBy_id");
            AddForeignKey("dbo.Blogs", "CreatedBy_id", "dbo.Users", "id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Blogs", "CreatedBy_id", "dbo.Users");
            DropIndex("dbo.Blogs", new[] { "CreatedBy_id" });
            DropColumn("dbo.Blogs", "CreatedBy_id");
        }
    }
}
