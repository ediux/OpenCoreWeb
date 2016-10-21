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
    public class GroupsController : Controller
    {
        private OpenWebSiteEntities db = new OpenWebSiteEntities();

        // GET: Groups
        public ActionResult Index()
        {
            return View(db.ApplicationGroup.ToList());
        }

        // GET: Groups/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ApplicationGroup applicationGroup = db.ApplicationGroup.Find(id);
            if (applicationGroup == null)
            {
                return HttpNotFound();
            }
            return View(applicationGroup);
        }

        // GET: Groups/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Groups/Create
        // 若要免於過量張貼攻擊，請啟用想要繫結的特定屬性，如需
        // 詳細資訊，請參閱 http://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Name,Void,CreateUserId,CreateTime,LastUpdateUserId,LastUpdateTime")] ApplicationGroup applicationGroup)
        {
            if (ModelState.IsValid)
            {
                db.ApplicationGroup.Add(applicationGroup);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(applicationGroup);
        }

        // GET: Groups/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ApplicationGroup applicationGroup = db.ApplicationGroup.Find(id);
            if (applicationGroup == null)
            {
                return HttpNotFound();
            }
            return View(applicationGroup);
        }

        // POST: Groups/Edit/5
        // 若要免於過量張貼攻擊，請啟用想要繫結的特定屬性，如需
        // 詳細資訊，請參閱 http://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Name,Void,CreateUserId,CreateTime,LastUpdateUserId,LastUpdateTime")] ApplicationGroup applicationGroup)
        {
            if (ModelState.IsValid)
            {
                db.Entry(applicationGroup).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(applicationGroup);
        }

        // GET: Groups/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ApplicationGroup applicationGroup = db.ApplicationGroup.Find(id);
            if (applicationGroup == null)
            {
                return HttpNotFound();
            }
            return View(applicationGroup);
        }

        // POST: Groups/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            ApplicationGroup applicationGroup = db.ApplicationGroup.Find(id);
            db.ApplicationGroup.Remove(applicationGroup);
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
