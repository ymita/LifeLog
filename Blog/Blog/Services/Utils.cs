using Blog.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Blog.Services
{
    public static class Utils
    {
        public static Dictionary<DateTime, int> getYearMonthList()
        {
            IEnumerable<Post> _posts = DataContext.Current.Posts.OrderByDescending(p => p.Published);
            #region 年月と対応する記事数のリストをクライアントに渡す。

            // 年月ごとの記事数をカウントする
            ILookup<DateTime, Post> _ordersByYearMonth =
                _posts.ToLookup(order => new DateTime(order.Published.Year, order.Published.Month, 1));

            // 年月ごとの記事数を保持する Dictionary
            Dictionary<DateTime, int> _yearmonthList = new Dictionary<DateTime, int>();
            // 年月ごとの記事数を算出
            foreach (var item in _ordersByYearMonth)
            {
                _yearmonthList.Add(item.Key, item.Count());
            }

            #endregion

            return _yearmonthList;
        }
    }
}