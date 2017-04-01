using Blog.Models;
using Blog.Services;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace Blog.Controllers
{
    public class HomeController : Controller
    {
        private DataContext db = new DataContext();
        IDataService _dataService;

        public HomeController()
        {
            _dataService = DataService.Instance;

            // TO DO: This should be done via DataService.Instance
            string description = db.Dashboards.First().Description.ToString();
            if(description != null)
            {
                ViewBag.Description = description;
            }
        }
        // GET: Posts
        public ActionResult Index(int? index)
        {
            int _startIndex = 0;
            int _pageCount = 5;

            if (index != null)
            {
                // index = 1 は 0 にしたい。
                if (index > 0)
                {
                    index = index - 1;
                }
                _startIndex = (int)index * 5;
            }

            int _endIndex = _startIndex + _pageCount;


            if(db.Posts != null)
            {

            }
            // ページャー用に記事総数を設定
            int _articleCount = db.Posts.Count();
            
            // 記事数がページ数で割った際に、端数があれば追加で１ページを追加する
            // （例：合計６記事あり、１ページあたり５記事を表示する場合、更に１ページを追加して６記事目を表示するため。）
            ViewBag.Pages = (_articleCount % _pageCount == 0) ? (_articleCount / _pageCount) : (_articleCount / _pageCount) + 1;

            #region 年月と対応する記事数のリストをクライアントに渡す。
            // 年月と対応する記事数の Dictionary をクライアントに渡す。
            ViewData["yearList"] = _dataService.getYearMonthList(db);
            #endregion

            // 記事を取得
            List<Post> _list = db.Posts.OrderByDescending(a => a.Published).Skip(_startIndex).Take(_endIndex - _startIndex).ToList();
            // 記事の部分表示
            int charCount = 200;
            for (int i = 0; i < _list.Count; i++)
            {
                if (_list[i].Description?.Length > charCount)
                {
                    string shortDesc = _list[i].Description.Substring(0, charCount) + "...";
                    _list[i].Description = shortDesc;
                }
            }

            // 記事をリターン
            return View(_list);
        }

        public ActionResult Date(string id)
        {
            // 年月と対応する記事数の Dictionary をクライアントに渡す。
            ViewData["yearList"] = _dataService.getYearMonthList(db);

            string[] yearMonth = id.Split('-');
            int year = int.Parse(yearMonth[0]);
            int month = int.Parse(yearMonth[1]);

            var listOfDate = db.Posts.Where(p => p.Published.Year == year && p.Published.Month == month).OrderByDescending(item=>item.Published).ToList();

            return View("Index", listOfDate);
        }
        // GET: Posts/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                id = 1;
                //return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Post post = db.Posts.Find(id);
            if (post == null)
            {
                return HttpNotFound();
            }
            return View(post);
        }

        // GET: Posts/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Posts/Create
        // 過多ポスティング攻撃を防止するには、バインド先とする特定のプロパティを有効にしてください。
        // 詳細については、http://go.microsoft.com/fwlink/?LinkId=317598 を参照してください。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,Title,Description,Published")] Post post)
        {
            if (ModelState.IsValid)
            {
                db.Posts.Add(post);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(post);
        }

        // GET: Posts/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Post post = db.Posts.Find(id);
            if (post == null)
            {
                return HttpNotFound();
            }
            return View(post);
        }

        // POST: Posts/Edit/5
        // 過多ポスティング攻撃を防止するには、バインド先とする特定のプロパティを有効にしてください。
        // 詳細については、http://go.microsoft.com/fwlink/?LinkId=317598 を参照してください。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,Title,Description,Published")] Post post)
        {
            if (ModelState.IsValid)
            {
                db.Entry(post).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(post);
        }

        // GET: Posts/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Post post = db.Posts.Find(id);
            if (post == null)
            {
                return HttpNotFound();
            }
            return View(post);
        }

        // POST: Posts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Post post = db.Posts.Find(id);
            db.Posts.Remove(post);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        [HttpPost]
        public ActionResult Dashboard()
        {
            //ViewBag.Message = "Your contact page.";

            return View("Dashboard");
        }

        [HttpGet]
        public ActionResult Search(string keyword)
        {
            IEnumerable<Post> posts = new List<Post>();

            if (!string.IsNullOrEmpty(keyword))
            {
                var postsFromDescription = db.Posts.Where(p => p.Description.Contains(keyword));
                var postsFromTitle = db.Posts.Where(p => p.Title.Contains(keyword));

                posts = postsFromDescription.Union(postsFromTitle).ToList();
            }

            return View("Index", posts);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        //public ActionResult Contact()
        //{
        //    ViewBag.Message = "Your contact page.";

        //    return View();
        //}
    }
}