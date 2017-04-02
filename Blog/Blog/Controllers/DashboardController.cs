using Blog.Models;
using Blog.Services;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;

namespace Blog.Controllers
{
    public class DashboardController : Controller
    {
        private int StartIndex = 0;
        private static readonly int PageCount = 10;

        public DashboardController()
        {
            
        }

        // GET: Dashboard
        public ActionResult Index()
        {
            return RedirectToAction("AllPosts");
        }

        public ActionResult AllPosts(int? index)
        {
            if (index != null)
            {
                // index = 1 は 0 にしたい。
                if (index > 0)
                {
                    index = index - 1;
                }
                StartIndex = (int)index * PageCount;
            }

            int _endIndex = StartIndex + PageCount;


            // ページャー用に記事総数を設定
            int articleCount = DataContext.Current.Posts.Count();
            ViewBag.Pages = (articleCount % PageCount == 0) ? articleCount / PageCount : (articleCount / PageCount) + 1;
            
            // 記事を取得
            List<Post> list = DataContext.Current.Posts.OrderByDescending(a => a.ID).Skip(StartIndex).Take(_endIndex - StartIndex).ToList();

            int charCount = 100;
            for (int i = 0; i < list.Count; i++)
            {
                if (list[i].Description?.Length > charCount)
                {
                    string shortDesc = list[i].Description.Substring(0, charCount) + "...";
                    list[i].Description = shortDesc;
                }

            }
            
            // 記事をリターン
            return View(list);
        }

        public ActionResult Draft(int? index)
        {
            if (index != null)
            {
                // index = 1 は 0 にしたい。
                if (index > 0)
                {
                    index = index - 1;
                }
                StartIndex = (int)index * PageCount;
            }

            int _endIndex = StartIndex + PageCount;


            // ページャー用に記事総数を設定
            int articleCount = DataContext.Current.Posts.Where(p=>p.IsDraft == true).Count();
            ViewBag.Pages = (articleCount % PageCount == 0) ? articleCount / PageCount : (articleCount / PageCount) + 1;
            // 記事を取得
            List<Post> list = DataContext.Current.Posts.Where(p => p.IsDraft == true).OrderByDescending(a => a.ID).Skip(StartIndex).Take(_endIndex - StartIndex).ToList();
            
            // 記事をリターン
            return View("AllPosts", list);
        }

        public ActionResult Published(int? index)
        {
            if (index != null)
            {
                // index = 1 は 0 にしたい。
                if (index > 0)
                {
                    index = index - 1;
                }
                StartIndex = (int)index * PageCount;
            }

            int _endIndex = StartIndex + PageCount;


            // ページャー用に記事総数を設定
            int articleCount = DataContext.Current.Posts.Where(p => p.IsDraft == false).Count();
            ViewBag.Pages = (articleCount % PageCount == 0) ? articleCount / PageCount : (articleCount / PageCount) + 1;
            // 記事を取得
            List<Post> list = DataContext.Current.Posts.Where(p => p.IsDraft == false).OrderByDescending(a => a.ID).Skip(StartIndex).Take(_endIndex - StartIndex).ToList();
            
            // 記事をリターン
            return View("AllPosts", list);
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
        public ActionResult Create([Bind(Include = "ID,Title,Description,Published,IsDraft")] Post post)
        {
            if (ModelState.IsValid)
            {
                DataContext.Current.Posts.Add(post);
                DataContext.Current.SaveChanges();
                return RedirectToAction("AllPosts");
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
            Post post = DataContext.Current.Posts.Find(id);
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
        public ActionResult Edit([Bind(Include = "ID,Title,Description,Published,IsDraft")] Post post)
        {
            if (ModelState.IsValid)
            {
                DataContext.Current.Entry(post).State = EntityState.Modified;
                DataContext.Current.SaveChanges();
                return RedirectToAction("AllPosts");
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
            Post post = DataContext.Current.Posts.Find(id);
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
            Post post = DataContext.Current.Posts.Find(id);
            DataContext.Current.Posts.Remove(post);
            DataContext.Current.SaveChanges();
            return RedirectToAction("AllPosts");
        }

        public ActionResult Configuration()
        {
            var dashboardInfo = DataContext.Current.Dashboards.First();
            return View(dashboardInfo);
        }

        public ActionResult Update(string description)
        {
            DataContext.Current.Dashboards.First().Description = description;
            DataContext.Current.SaveChanges();
            var dashboardInfo = DataContext.Current.Dashboards.First();
            return View("Configuration", dashboardInfo);
        }

        [HttpPost]
        public ActionResult Dashboard()
        {
            ViewBag.Message = "Your contact page.";

            return View("Dashboard");
        }
    }
}