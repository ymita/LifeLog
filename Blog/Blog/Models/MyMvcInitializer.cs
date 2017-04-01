using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Blog.Models
{
    public class MyMvcInitializer : System.Data.Entity.CreateDatabaseIfNotExists<DataContext>
    {
        protected override void Seed(DataContext context)
        {
            List<Tag> tags = new List<Tag>();

            Tag tag1 = new Tag();
            tag1.ID = 1;
            tag1.Name = "Test tag 1";
            tags.Add(tag1);

            Tag tag2 = new Tag();
            tag2.ID = 2;
            tag2.Name = "Test tag 2";
            tags.Add(tag2);

            var posts = new List<Post>
            {
                new Post
                {
                    ID = 1,
                    Title = "First post",
                    Description = "This is the first post.",
                    Published = DateTime.Parse("2017-01-10"),
                    IsDraft = false,
                    Tags = tags
                },
            };

            var dashboard = new Dashboard
            {
                Id = 0,
                Description = "This is a blog about xxx."
            };
            posts.ForEach(b => context.Posts.Add(b));
            context.Dashboards.Add(dashboard);
            context.SaveChanges();
        }
    }
}