namespace CourseWorkProject.DAL
{
    using System;
    using System.Data.Entity;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;

    public partial class CWContext : DbContext
    {
        public CWContext()
            : base("name=CWContext")
        {
        }

        public System.Data.Entity.DbSet<CourseWorkProject.Models.User> Users { get; set; }
        public System.Data.Entity.DbSet<CourseWorkProject.Models.Role> Roles { get; set; }
        public System.Data.Entity.DbSet<CourseWorkProject.Models.Profile> Profiles { get; set; }

        public System.Data.Entity.DbSet<CourseWorkProject.Models.Group> Groups { get; set; }

        public System.Data.Entity.DbSet<CourseWorkProject.Models.GroupMember> GroupMembers { get; set; }

        public System.Data.Entity.DbSet<CourseWorkProject.Models.Blog> Blogs { get; set; }

        public System.Data.Entity.DbSet<CourseWorkProject.Models.Comment> Comments { get; set; }
        public System.Data.Entity.DbSet<CourseWorkProject.Models.Chat> Chats { get; set; }

        public System.Data.Entity.DbSet<CourseWorkProject.Models.ChatBox> ChatBoxes { get; set; }

        public System.Data.Entity.DbSet<CourseWorkProject.Models.Event> Events { get; set; }
        public System.Data.Entity.DbSet<CourseWorkProject.Models.Sender> Senders { get; set; }
        public System.Data.Entity.DbSet<CourseWorkProject.Models.Category> Categories { get; set; }
        public System.Data.Entity.DbSet<CourseWorkProject.Models.Like> Likes { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
        }
    }
}
