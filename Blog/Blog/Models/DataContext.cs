namespace Blog.Models
{
    using System;
    using System.Data.Entity;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;
    using System.Web;

    public partial class DataContext : DbContext
    {
        public DataContext()
            : base("name=DataContext")
        {
        }

        public virtual DbSet<Post> Posts { get; set; }
        public virtual DbSet<Tag> Tags { get; set; }

        public virtual DbSet<Dashboard> Dashboards { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Post>()
                .HasMany(e => e.Tags)
                .WithOptional(e => e.Post)
                .HasForeignKey(e => e.Post_ID);

            modelBuilder.Entity<Dashboard>();
        }


        //One instance per request
        private const string CacheKey = "__DataContext__";

        public static bool HasCurrent
        {
            get { return HttpContext.Current.Items[CacheKey] != null; }
        }

        public static DataContext Current
        {
            get
            {
                var context = (DataContext)HttpContext.Current.Items[CacheKey];

                if (context == null)
                {
                    context = new DataContext();

                    HttpContext.Current.Items[CacheKey] = context;
                }

                return context;
            }
        }
    }
}
