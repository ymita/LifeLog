using Blog.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace Blog.Services
{
    /// <summary>
    /// Description of DataService.
    /// </summary>
    public sealed class DataService : IDataService//, IDisposable
    {
        private DataContext db = new DataContext();
        private static DataService instance = new DataService();

        public static DataService Instance
        {
            get
            {
                return instance;
            }
        }

        private DataService()
        {
        }

        private ILookup<DateTime, Post> _ordersByYearMonth;
        private Dictionary<DateTime, int> _yearmonthList;

        public Dictionary<DateTime, int> getYearMonthList()
        {
            var _posts = db.Posts.OrderByDescending(p => p.Published);
            #region 年月と対応する記事数のリストをクライアントに渡す。

            // 年月ごとの記事数をカウントする
            _ordersByYearMonth =
                _posts.ToLookup(order => new DateTime(order.Published.Year, order.Published.Month, 1));

            // 年月ごとの記事数を保持する Dictionary
            _yearmonthList = new Dictionary<DateTime, int>();
            // 年月ごとの記事数を算出
            foreach (var item in _ordersByYearMonth)
            {
                //if (!_yearmonthList.Keys.Contains(item.Key))
                //{
                    _yearmonthList.Add(item.Key, item.Count());
                //}
            }

            #endregion

            return _yearmonthList;
        }

        public DbSet<Post> getPosts()
        {
            return db.Posts;
            throw new NotImplementedException();
        }

        public bool addPost(Post post)
        {
            bool result = false;
            try
            {
                db.Posts.Add(post);
                db.SaveChanges();
                result = true;
                return result;
            }
            catch(Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
            }
            return result;
        }

        public string getDescription()
        {
            string description = db.Dashboards.First().Description.ToString();
            return description;
        }

        //public void Dispose()
        //{
        //    Dispose(true);
        //    GC.SuppressFinalize(this);
        //}

        //void Dispose(bool disposing)
        //{
        //    if (disposing)
        //    {
        //        db.Dispose();
        //    }
        //}

        //~DataService()
        //{
        //    Dispose(false);
        //}

        //public string getDescription(DataContext db)
        //{

        //    if (db.Dashboards.Count() > 1)
        //    {
        //        var description = db.Dashboards.First().Description;
        //        return description;
        //    }
        //    return "error";
        //}
    }

    //public class DataService : IDataService
    //{
    //    //public DataService()
    //    //{

    //    //}

        
    //}
}