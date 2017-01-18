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

        //specifies a price range for housing 
        enum group1 : int { min = 40, max = 75};
        enum group2 : int { min = 75, max = 95};
        enum group3 : int { min = 95, max = 130 };
        enum group4 : int { min = 130, max = 200 };


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
            }

            return View(listing.ToList());
        }

        public ActionResult FilterSearchResults(string city, int? priceRange, int? bed, int? bath)
        {
            var listing = from l in db.Listing select l;

            if (!String.IsNullOrEmpty(city)) {
                listing = listing.Where(
                    l => l.City.Contains(city));
            }
            int? price = priceRange;
            return PartialView("ListingIndex", listing.ToList());
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

            //find and return all images in directory Content/Images/Listing/*id*
            string imagesPathName = "~/Content/Images/Listing/" + id +"/";
            try
            {
                ViewBag.Images = Directory.EnumerateFiles(Server.MapPath(imagesPathName)).Select(fn => imagesPathName + Path.GetFileName(fn));
            }
            catch (System.IO.DirectoryNotFoundException)
            {
                ViewBag.Images = null;
            }

            return View(listing);
        }

        //In the future a realtor may be permitted to create/update/delete listings
        //GET: Listing/Create\
        [Authorize(Roles ="Admin,Realtor")]
        public ActionResult Create()
        {
            ViewBag.RealtorID = new SelectList(db.Realtor, "ID", "FirstName");
            return View();
        }

        // POST: Listing/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin,Realtor")]
        public ActionResult Create([Bind(Include = "ListID,HouseNumber,StreetName,ApartmentNumber,City,State,ZipCode,Price,Bed,Bath,RealtorID,ExtraFeatures,SquareFeet,LotSize")] Listing listing)
        {
            if (ModelState.IsValid)
            {
                db.Listing.Add(listing);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.RealtorID = new SelectList(db.Realtor, "ID", "FirstName", listing.RealtorID);
            return View(listing);
        }

        // GET: Listing/Edit/5
        [Authorize(Roles = "Admin,Realtor")]
        public ActionResult Edit(int? id)
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
            ViewBag.RealtorID = new SelectList(db.Realtor, "ID", "FirstName", listing.RealtorID);
            return View(listing);
        }

        // POST: Listing/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin,Realtor")]
        public ActionResult Edit([Bind(Include = "ListID,HouseNumber,StreetName,ApartmentNumber,City,State,ZipCode,Price,Bed,Bath,RealtorID,ExtraFeatures,SquareFeet,LotSize")] Listing listing)
        {
            if (ModelState.IsValid)
            {
                db.Entry(listing).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.RealtorID = new SelectList(db.Realtor, "ID", "FirstName", listing.RealtorID);
            return View(listing);
        }

        // GET: Listing/Delete/5
        [Authorize(Roles = "Admin,Realtor")]
        public ActionResult Delete(int? id)
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

        // POST: Listing/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin,Realtor")]
        public ActionResult DeleteConfirmed(int id)
        {
            Listing listing = db.Listing.Find(id);
            db.Listing.Remove(listing);
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
