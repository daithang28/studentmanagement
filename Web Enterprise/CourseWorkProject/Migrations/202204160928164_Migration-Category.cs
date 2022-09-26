namespace CourseWorkProject.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class MigrationCategory : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Categories",
                c => new
                    {
                        id = c.Int(nullable: false, identity: true),
                        Creator = c.String(),
                        Name = c.String(nullable: false),
                        CreateDate = c.DateTime(),
                    })
                .PrimaryKey(t => t.id);
            
            AddColumn("dbo.Blogs", "Category_id", c => c.Int());
            CreateIndex("dbo.Blogs", "Category_id");
            AddForeignKey("dbo.Blogs", "Category_id", "dbo.Categories", "id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Blogs", "Category_id", "dbo.Categories");
            DropIndex("dbo.Blogs", new[] { "Category_id" });
            DropColumn("dbo.Blogs", "Category_id");
            DropTable("dbo.Categories");
        }
    }
}
