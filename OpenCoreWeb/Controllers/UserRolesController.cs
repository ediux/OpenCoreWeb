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
    public class UserRolesController : Controller
    {
        private OpenWebSiteEntities db = new OpenWebSiteEntities();

        // GET: UserRoles
        public ActionResult Index()
        {
            var applicationUserRole = db.ApplicationUserRole.Include(a => a.ApplicationUser).Include(a => a.ApplicationRole);
            return View(applicationUserRole.ToList());
        }

        // GET: UserRoles/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ApplicationUserRole applicationUserRole = db.ApplicationUserRole.Find(id);
            if (applicationUserRole == null)
            {
                return HttpNotFound();
            }
            return View(applicationUserRole);
        }

        // GET: UserRoles/Create
        public ActionResult Create()
        {
            ViewBag.UserId = new SelectList(db.ApplicationUser, "Id", "UserName");
            ViewBag.RoleId = new SelectList(db.ApplicationRole, "Id", "Name");
            return View();
        }

        // POST: UserRoles/Create
        // 若要免於過量張貼攻擊，請啟用想要繫結的特定屬性，如需
        // 詳細資訊，請參閱 http://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "UserId,RoleId,Void")] ApplicationUserRole applicationUserRole)
        {
            if (ModelState.IsValid)
            {
                db.ApplicationUserRole.Add(applicationUserRole);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.UserId = new SelectList(db.ApplicationUser, "Id", "UserName", applicationUserRole.UserId);
            ViewBag.RoleId = new SelectList(db.ApplicationRole, "Id", "Name", applicationUserRole.RoleId);
            return View(applicationUserRole);
        }

        // GET: UserRoles/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ApplicationUserRole applicationUserRole = db.ApplicationUserRole.Find(id);
            if (applicationUserRole == null)
            {
                return HttpNotFound();
            }
            ViewBag.UserId = new SelectList(db.ApplicationUser, "Id", "UserName", applicationUserRole.UserId);
            ViewBag.RoleId = new SelectList(db.ApplicationRole, "Id", "Name", applicationUserRole.RoleId);
            return View(applicationUserRole);
        }

        // POST: UserRoles/Edit/5
        // 若要免於過量張貼攻擊，請啟用想要繫結的特定屬性，如需
        // 詳細資訊，請參閱 http://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "UserId,RoleId,Void")] ApplicationUserRole applicationUserRole)
        {
            if (ModelState.IsValid)
            {
                db.Entry(applicationUserRole).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.UserId = new SelectList(db.ApplicationUser, "Id", "UserName", applicationUserRole.UserId);
            ViewBag.RoleId = new SelectList(db.ApplicationRole, "Id", "Name", applicationUserRole.RoleId);
            return View(applicationUserRole);
        }

        // GET: UserRoles/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ApplicationUserRole applicationUserRole = db.ApplicationUserRole.Find(id);
            if (applicationUserRole == null)
            {
                return HttpNotFound();
            }
            return View(applicationUserRole);
        }

        // POST: UserRoles/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            ApplicationUserRole applicationUserRole = db.ApplicationUserRole.Find(id);
            db.ApplicationUserRole.Remove(applicationUserRole);
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
