using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using RealMax.Models;

namespace RealMax.Controllers
{
    public class ListingController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Listing
        //divide up search string by whitespace and determine if any words match any fields in the listing
        public ActionResult Index(string id)
        {
            string searchString = id;

            var listing = from l in db.Listing select l;

            if (!String.IsNullOrEmpty(searchString))
            {
                string[] searchStringArray = searchString.Split();
                foreach(string word in searchStringArray) {
                    listing = listing.Where(
                    l => l.ApartmentNumber.Contains(word) ||
                    l.HouseNumber.Contains(word) ||
                    l.StreetName.Contains(word) ||
                    l.City.Contains(word) ||
                    l.Realtor.FirstName.Contains(word) ||
                    l.Realtor.LastName.Contains(word)
                    );
                }
                //ViewBag.Images = Directory.EnumerateFiles(Server.MapPath("~/Content/Images/"))


            }
                //db.Listing.Include(l => l.Realtor);
            return View(listing.ToList());
        }
        
        // GET: Listing/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Listing listing = db.Listing.Find(id);
            if (listing == null)
            {
                return HttpNotFound();
            }
            return View(listing);
        }

        //In the future a realtor may be permitted to create/update/delete listings
        //GET: Listing/Create
        //public ActionResult Create()
        //{
        //    ViewBag.RealtorID = new SelectList(db.Realtor, "ID", "FirstName");
        //    return View();
        //}

        //// POST: Listing/Create
        //// To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        //// more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult Create([Bind(Include = "ListID,HouseNumber,StreetName,ApartmentNumber,City,State,ZipCode,Price,Bed,Bath,RealtorID,ExtraFeatures,SquareFeet,LotSize")] Listing listing)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        db.Listing.Add(listing);
        //        db.SaveChanges();
        //        return RedirectToAction("Index");
        //    }

        //    ViewBag.RealtorID = new SelectList(db.Realtor, "ID", "FirstName", listing.RealtorID);
        //    return View(listing);
        //}

        //// GET: Listing/Edit/5
        //public ActionResult Edit(int? id)
        //{
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    Listing listing = db.Listing.Find(id);
        //    if (listing == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    ViewBag.RealtorID = new SelectList(db.Realtor, "ID", "FirstName", listing.RealtorID);
        //    return View(listing);
        //}

        //// POST: Listing/Edit/5
        //// To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        //// more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult Edit([Bind(Include = "ListID,HouseNumber,StreetName,ApartmentNumber,City,State,ZipCode,Price,Bed,Bath,RealtorID,ExtraFeatures,SquareFeet,LotSize")] Listing listing)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        db.Entry(listing).State = EntityState.Modified;
        //        db.SaveChanges();
        //        return RedirectToAction("Index");
        //    }
        //    ViewBag.RealtorID = new SelectList(db.Realtor, "ID", "FirstName", listing.RealtorID);
        //    return View(listing);
        //}

        //// GET: Listing/Delete/5
        //public ActionResult Delete(int? id)
        //{
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    Listing listing = db.Listing.Find(id);
        //    if (listing == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    return View(listing);
        //}

        //// POST: Listing/Delete/5
        //[HttpPost, ActionName("Delete")]
        //[ValidateAntiForgeryToken]
        //public ActionResult DeleteConfirmed(int id)
        //{
        //    Listing listing = db.Listing.Find(id);
        //    db.Listing.Remove(listing);
        //    db.SaveChanges();
        //    return RedirectToAction("Index");
        //}

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
