using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using My.Core.Infrastructures.Implementations.Models;

namespace OpenCoreWeb.Controllers
{
    [Authorize]
    public class UserGroupsController : Controller
    {
        private OpenWebSiteEntities db = new OpenWebSiteEntities();

        // GET: UserGroups
        public ActionResult Index()
        {
            var applicationUserGroup = db.ApplicationUserGroup.Include(a => a.ApplicationGroup).Include(a => a.ApplicationUser);
            return View(applicationUserGroup.ToList());
        }

        // GET: UserGroups/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ApplicationUserGroup applicationUserGroup = db.ApplicationUserGroup.Find(id);
            if (applicationUserGroup == null)
            {
                return HttpNotFound();
            }
            return View(applicationUserGroup);
        }

        // GET: UserGroups/Create
        public ActionResult Create()
        {
            ViewBag.GroupId = new SelectList(db.ApplicationGroup, "Id", "Name");
            ViewBag.UserId = new SelectList(db.ApplicationUser, "Id", "UserName");
            return View();
        }

        // POST: UserGroups/Create
        // 若要免於過量張貼攻擊，請啟用想要繫結的特定屬性，如需
        // 詳細資訊，請參閱 http://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "UserId,GroupId,Void")] ApplicationUserGroup applicationUserGroup)
        {
            if (ModelState.IsValid)
            {
                db.ApplicationUserGroup.Add(applicationUserGroup);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.GroupId = new SelectList(db.ApplicationGroup, "Id", "Name", applicationUserGroup.GroupId);
            ViewBag.UserId = new SelectList(db.ApplicationUser, "Id", "UserName", applicationUserGroup.UserId);
            return View(applicationUserGroup);
        }

        // GET: UserGroups/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ApplicationUserGroup applicationUserGroup = db.ApplicationUserGroup.Find(id);
            if (applicationUserGroup == null)
            {
                return HttpNotFound();
            }
            ViewBag.GroupId = new SelectList(db.ApplicationGroup, "Id", "Name", applicationUserGroup.GroupId);
            ViewBag.UserId = new SelectList(db.ApplicationUser, "Id", "UserName", applicationUserGroup.UserId);
            return View(applicationUserGroup);
        }

        // POST: UserGroups/Edit/5
        // 若要免於過量張貼攻擊，請啟用想要繫結的特定屬性，如需
        // 詳細資訊，請參閱 http://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "UserId,GroupId,Void")] ApplicationUserGroup applicationUserGroup)
        {
            if (ModelState.IsValid)
            {
                db.Entry(applicationUserGroup).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.GroupId = new SelectList(db.ApplicationGroup, "Id", "Name", applicationUserGroup.GroupId);
            ViewBag.UserId = new SelectList(db.ApplicationUser, "Id", "UserName", applicationUserGroup.UserId);
            return View(applicationUserGroup);
        }

        // GET: UserGroups/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ApplicationUserGroup applicationUserGroup = db.ApplicationUserGroup.Find(id);
            if (applicationUserGroup == null)
            {
                return HttpNotFound();
            }
            return View(applicationUserGroup);
        }

        // POST: UserGroups/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            ApplicationUserGroup applicationUserGroup = db.ApplicationUserGroup.Find(id);
            db.ApplicationUserGroup.Remove(applicationUserGroup);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
