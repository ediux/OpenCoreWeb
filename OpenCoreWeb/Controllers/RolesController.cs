using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using My.Core.Infrastructures.Implementations.Models;
using Microsoft.AspNet.Identity;

namespace OpenCoreWeb.Controllers
{
    [Authorize]
    public class RolesController : Controller
    {
        private OpenWebSiteEntities db = new OpenWebSiteEntities();

        // GET: Roles
        public ActionResult Index()
        {
            return View(db.ApplicationRole.Where(w => w.Void == false).ToList());
        }

        // GET: Roles/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ApplicationRole applicationRole = db.ApplicationRole.Find(id);
            if (applicationRole == null)
            {
                return HttpNotFound();
            }
            return View(applicationRole);
        }

        // GET: Roles/Create
        public ActionResult Create()
        {
            var model = ApplicationRole.Create();
            model.CreateUserId = model.LastUpdateUserId = User.Identity.GetUserId<int>();
            return View(model);
        }

        // POST: Roles/Create
        // 若要免於過量張貼攻擊，請啟用想要繫結的特定屬性，如需
        // 詳細資訊，請參閱 http://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Name,Void,CreateUserId,CreateTime,LastUpdateUserId,LastUpdateTime")] ApplicationRole applicationRole)
        {
            if (ModelState.IsValid)
            {
                var roles = db.ApplicationRole.Where(w => w.Name == applicationRole.Name);
                if (roles.Any())
                {
                    var role = roles.Single();

                    role.Name = applicationRole.Name;
                    role.Void = false;
                    role.CreateUserId = role.LastUpdateUserId = User.Identity.GetUserId<int>();
                    role.CreateTime = role.LastUpdateTime = DateTime.Now.ToUniversalTime();
                    if (role.ApplicationUserRole.Any())
                    {
                        role.ApplicationUserRole.Clear();
                    }
                    db.Entry(role).State = EntityState.Modified;
                }
                else
                {
                    db.ApplicationRole.Add(applicationRole);
                }

                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(applicationRole);
        }

        // GET: Roles/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ApplicationRole applicationRole = db.ApplicationRole.Find(id);

            if (applicationRole == null)
            {
                return HttpNotFound();
            }

            applicationRole.LastUpdateTime = DateTime.Now.ToUniversalTime();
            applicationRole.LastUpdateUserId = User.Identity.GetUserId<int>();

            return View(applicationRole);
        }

        // POST: Roles/Edit/5
        // 若要免於過量張貼攻擊，請啟用想要繫結的特定屬性，如需
        // 詳細資訊，請參閱 http://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Name,Void,CreateUserId,CreateTime,LastUpdateUserId,LastUpdateTime")] ApplicationRole applicationRole)
        {
            if (ModelState.IsValid)
            {
                db.Entry(applicationRole).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(applicationRole);
        }

        // GET: Roles/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ApplicationRole applicationRole = db.ApplicationRole.Find(id);

            if (applicationRole == null)
            {
                return HttpNotFound();
            }
            return View(applicationRole);
        }

        // POST: Roles/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            ApplicationRole applicationRole = db.ApplicationRole.Find(id);
            //db.ApplicationRole.Remove(applicationRole);
            applicationRole.Void = true;
            db.Entry(applicationRole).State = EntityState.Modified;
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
