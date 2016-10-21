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
    public class UserProfilesController : Controller
    {
        private OpenWebSiteEntities db = new OpenWebSiteEntities();

        // GET: UserProfiles
        public ActionResult Index()
        {
            return View(db.ApplicationUserProfile.ToList());
        }

        // GET: UserProfiles/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ApplicationUserProfile applicationUserProfile = db.ApplicationUserProfile.Find(id);
            if (applicationUserProfile == null)
            {
                return HttpNotFound();
            }
            return View(applicationUserProfile);
        }

        // GET: UserProfiles/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: UserProfiles/Create
        // 若要免於過量張貼攻擊，請啟用想要繫結的特定屬性，如需
        // 詳細資訊，請參閱 http://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Address,EMail,EMailConfirmed,PhoneNumber,PhoneConfirmed,CreateTime,LastUpdateTime,DisplayName")] ApplicationUserProfile applicationUserProfile)
        {
            if (ModelState.IsValid)
            {
                db.ApplicationUserProfile.Add(applicationUserProfile);
                db.SaveChanges();
                return RedirectToAction("Create", "ApplicationUserProfileRefs", new { id = User.Identity.GetUserId<int>(), profileId = applicationUserProfile.Id });
            }

            return View(applicationUserProfile);
        }

        // GET: UserProfiles/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ApplicationUserProfile applicationUserProfile = db.ApplicationUserProfile.Find(id);
            if (applicationUserProfile == null)
            {
                return HttpNotFound();
            }
            return View(applicationUserProfile);
        }

        // POST: UserProfiles/Edit/5
        // 若要免於過量張貼攻擊，請啟用想要繫結的特定屬性，如需
        // 詳細資訊，請參閱 http://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Address,EMail,EMailConfirmed,PhoneNumber,PhoneConfirmed,CreateTime,LastUpdateTime,DisplayName")] ApplicationUserProfile applicationUserProfile)
        {
            if (ModelState.IsValid)
            {
                db.Entry(applicationUserProfile).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(applicationUserProfile);
        }

        // GET: UserProfiles/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ApplicationUserProfile applicationUserProfile = db.ApplicationUserProfile.Find(id);
            if (applicationUserProfile == null)
            {
                return HttpNotFound();
            }
            return View(applicationUserProfile);
        }

        // POST: UserProfiles/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            ApplicationUserProfile applicationUserProfile = db.ApplicationUserProfile.Find(id);
            db.ApplicationUserProfile.Remove(applicationUserProfile);
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
