using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CMS.Models;
using WebMatrix.WebData;
using System.Web.Security;
using PagedList;

namespace CMS.Controllers
{
    public class ForPostController : Controller
    {
        private UsersContext db = new UsersContext();

        //
        // GET: /ForPost/
        [Authorize]
        public ActionResult Index(int  ?page)
        {
            var model = db.Posts.OrderByDescending(p=>p.Date);
            int pageNumber = page ?? 1;
            int pagesize = 5;
            return View(model.ToPagedList(pageNumber,pagesize));
        }

        public ActionResult MyRecord(int? page)
        {

            int myid = (int)WebSecurity.CurrentUserId;
            string id = myid.ToString();
            var myrecords = db.Posts.Where(p => p.UserID == id).OrderByDescending(p => p.Date);
            int pageNumber = page ?? 1;
            int pagesize = 5;
            return View(myrecords.ToPagedList(pageNumber, pagesize));



        }



        // GET: /ForPost/Create
        [Authorize(Roles = "admin,writer")]

        public ActionResult Create()
        {
            return View();
        }


        // POST: /ForPost/Create

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Post post)
        {
            if (ModelState.IsValid)
            {

                post.Date = DateTime.Now;
                post.Author = User.Identity.Name;
                int myid = (int)WebSecurity.CurrentUserId;
                post.UserID = myid.ToString();
                db.Posts.Add(post);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View();
        }

        //
        // GET: /ForPost/Edit/5

        public ActionResult Edit(int id = 0)
        {
            Post post = db.Posts.Find(id);
            if (post == null)
            {
                return HttpNotFound();
            }
            return View(post);
        }

        //
        // POST: /ForPost/Edit/5

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Post post)
        {
            if (ModelState.IsValid)
            {
                post.Date = DateTime.Now;
                post.Author = User.Identity.Name;
                int mu2 = (int)Membership.GetUser().ProviderUserKey;
                post.UserID = mu2.ToString();
                db.Entry(post).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(post);
        }

        //
        // GET: /ForPost/Delete/5

        public ActionResult Delete(int id = 0)
        {
            Post post = db.Posts.Find(id);
            if (post == null)
            {
                return HttpNotFound();
            }
            return View(post);
        }

        //
        // POST: /ForPost/Delete/5

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Post post = db.Posts.Find(id);
            db.Posts.Remove(post);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}