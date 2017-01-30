using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using RealMax.Models;

namespace RealMax.Controllers
{
    public class RealtorController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Realtor
        public ViewResult Index(string id)
        {
            string searchString = id;
            var realtors = from r in db.Realtor select r;

            if (!String.IsNullOrEmpty(searchString))
            {
                realtors = realtors.Where(r => r.LastName.Contains(searchString) || r.FirstName.Contains(searchString));
            }
            return View(realtors.ToList());
        }

        // GET: Realtor/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Realtor realtor = db.Realtor.Find(id);
            if (realtor == null)
            {
                return HttpNotFound();
            }
            return View(realtor);
        }

        //The following actions that have been commented out are left behind for later when some sort of user roles will be implemented

        // GET: Realtor/Create
        //TODO: https://docs.microsoft.com/en-us/azure/app-service-web/web-sites-dotnet-deploy-aspnet-mvc-app-membership-oauth-sql-database
        //most likely to work for adding roles:
        //https://code.msdn.microsoft.com/ASPNET-MVC-5-Security-And-44cbdb97
        [Authorize(Roles = "Admin")]
        public ActionResult Create()
        {
            return View();
        }

        //// POST: Realtor/Create
        //// To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        //// more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public ActionResult Create([Bind(Include = "ID,FirstName,LastName,Email,PhoneNumber,Bio")] Realtor realtor)
        {
            if (ModelState.IsValid)
            {
                db.Realtor.Add(realtor);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(realtor);
        }

        // GET: Realtor/Edit/5
        [Authorize(Roles = "Admin")]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Realtor realtor = db.Realtor.Find(id);
            if (realtor == null)
            {
                return HttpNotFound();
            }
            return View(realtor);
        }

        // POST: Realtor/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public ActionResult Edit([Bind(Include = "ID,FirstName,LastName,Email,PhoneNumber,Bio")] Realtor realtor)
        {
            if (ModelState.IsValid)
            {
                db.Entry(realtor).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(realtor);
        }

        public ActionResult Remove()
        {
            var realtors = from r in db.Realtor select r;
            return View(realtors.ToList());
        }

        // GET: Realtor/Delete/5
        [Authorize(Roles = "Admin")]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Realtor realtor = db.Realtor.Find(id);
            if (realtor == null)
            {
                return HttpNotFound();
            }
            return View(realtor);
        }

        // POST: Realtor/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public ActionResult DeleteConfirmed(int id)
        {
            Realtor realtor = db.Realtor.Find(id);
            db.Realtor.Remove(realtor);
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
