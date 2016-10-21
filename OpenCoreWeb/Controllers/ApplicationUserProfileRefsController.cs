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
    public class ApplicationUserProfileRefsController : Controller
    {
        private OpenWebSiteEntities db = new OpenWebSiteEntities();

        // GET: ApplicationUserProfileRefs
        public ActionResult Index()
        {
            var applicationUserProfileRef = db.ApplicationUserProfileRef.Include(a => a.ApplicationUser).Include(a => a.ApplicationUserProfile);
            return View(applicationUserProfileRef.ToList());
        }

        // GET: ApplicationUserProfileRefs/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ApplicationUserProfileRef applicationUserProfileRef = db.ApplicationUserProfileRef.Find(id);
            if (applicationUserProfileRef == null)
            {
                return HttpNotFound();
            }
            return View(applicationUserProfileRef);
        }

        // GET: ApplicationUserProfileRefs/Create
        public ActionResult Create(int? id, int? profileId)
        {
            if (id.HasValue && profileId.HasValue)
            {
                db.ApplicationUserProfileRef.Add(new ApplicationUserProfileRef()
                {
                    UserId = id.Value,
                    ProfileId = profileId.Value,
                    Void = false,
                    CreateTime = DateTime.Now.ToUniversalTime(),
                    LastUpdateTime = DateTime.Now.ToUniversalTime()
                });
                db.SaveChanges();
                return RedirectToAction("Index", "Manage");
            }

            ViewBag.UserId = new SelectList(db.ApplicationUser, "Id", "UserName");
            ViewBag.ProfileId = new SelectList(db.ApplicationUserProfile, "Id", "Address");
            return View();
        }

        // POST: ApplicationUserProfileRefs/Create
        // 若要免於過量張貼攻擊，請啟用想要繫結的特定屬性，如需
        // 詳細資訊，請參閱 http://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "UserId,ProfileId,Void,CreateTime,LastUpdateTime")] ApplicationUserProfileRef applicationUserProfileRef)
        {
            if (ModelState.IsValid)
            {
                db.ApplicationUserProfileRef.Add(applicationUserProfileRef);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.UserId = new SelectList(db.ApplicationUser, "Id", "UserName", applicationUserProfileRef.UserId);
            ViewBag.ProfileId = new SelectList(db.ApplicationUserProfile, "Id", "Address", applicationUserProfileRef.ProfileId);
            return View(applicationUserProfileRef);
        }

        // GET: ApplicationUserProfileRefs/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ApplicationUserProfileRef applicationUserProfileRef = db.ApplicationUserProfileRef.Find(id);
            if (applicationUserProfileRef == null)
            {
                return HttpNotFound();
            }
            ViewBag.UserId = new SelectList(db.ApplicationUser, "Id", "UserName", applicationUserProfileRef.UserId);
            ViewBag.ProfileId = new SelectList(db.ApplicationUserProfile, "Id", "Address", applicationUserProfileRef.ProfileId);
            return View(applicationUserProfileRef);
        }

        // POST: ApplicationUserProfileRefs/Edit/5
        // 若要免於過量張貼攻擊，請啟用想要繫結的特定屬性，如需
        // 詳細資訊，請參閱 http://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "UserId,ProfileId,Void,CreateTime,LastUpdateTime")] ApplicationUserProfileRef applicationUserProfileRef)
        {
            if (ModelState.IsValid)
            {
                db.Entry(applicationUserProfileRef).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.UserId = new SelectList(db.ApplicationUser, "Id", "UserName", applicationUserProfileRef.UserId);
            ViewBag.ProfileId = new SelectList(db.ApplicationUserProfile, "Id", "Address", applicationUserProfileRef.ProfileId);
            return View(applicationUserProfileRef);
        }

        // GET: ApplicationUserProfileRefs/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ApplicationUserProfileRef applicationUserProfileRef = db.ApplicationUserProfileRef.Find(id);
            if (applicationUserProfileRef == null)
            {
                return HttpNotFound();
            }
            return View(applicationUserProfileRef);
        }

        // POST: ApplicationUserProfileRefs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            ApplicationUserProfileRef applicationUserProfileRef = db.ApplicationUserProfileRef.Find(id);
            db.ApplicationUserProfileRef.Remove(applicationUserProfileRef);
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
