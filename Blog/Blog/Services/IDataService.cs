using Blog.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blog.Services
{
    interface IDataService
    {
        Dictionary<DateTime, int> getYearMonthList();

        DbSet<Post> getPosts();

        bool addPost(Post post);
        string getDescription();
    }
}
