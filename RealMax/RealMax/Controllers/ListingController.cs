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

            if(priceRange != null && priceRange >= 0)
            {
                
                int max, min;

                switch (priceRange)
                {
                    case 0:
                        max = (int)group1.max;
                        min = (int)group1.min;
                        break;
                    case 1:
                        max = (int)group2.max;
                        min = (int)group2.min;
                        break;
                    case 2:
                        max = (int)group3.max;
                        min = (int)group3.min;
                        break;
                    case 3:
                        max = (int)group4.max;
                        min = (int)group4.min;
                        break;
                    default:
                        min = (int)group1.min;
                        max = (int)group2.max;
                        break;
                }
                max = max * 1000;
                min = min * 1000;
                listing = listing.Where(
                    p => p.Price > min && p.Price <= max).OrderBy(p => p.Price);
            }

            if(bed != null && bed > 0)
            {
                listing = listing.Where(
                    l => l.Bed == bed);
            }

            if(bath != null && bath > 0)
            {
                listing = listing.Where(
                    l => l.Bath == bath);
            }
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
        public ActionResult Create([Bind(Include = "ListID,HouseNumber,StreetName,ApartmentNumber,City,State,ZipCode,Price,Bed,Bath,RealtorID,ExtraFeatures,SquareFeet,LotSize")] Listing listing, HttpPostedFileBase thumbnailUpload, IEnumerable<HttpPostedFileBase> picsUpload)
        {
            //TODO: http://haacked.com/archive/2010/07/16/uploading-files-with-aspnetmvc.aspx/ also look up using multiple for input type of file http://stackoverflow.com/questions/3853767/maximum-request-length-exceeded <- for max file length problems

            if (ModelState.IsValid)
            {
                

                //Dont want to make any changes in db until testing is finished
                db.Listing.Add(listing);
                db.SaveChanges();

				int id = listing.ListID;
				var directoryPath = Path.Combine("~/Content/Images/Listing", id.ToString());
				if (thumbnailUpload != null)
				{
					var fileExtension = Path.GetExtension(thumbnailUpload.FileName);
					var fileName = "thumbnail" + fileExtension;
					var fullPath = Path.Combine(directoryPath, fileName);

					if (!Directory.Exists(Server.MapPath(directoryPath)))
						Directory.CreateDirectory(Server.MapPath(directoryPath));

					thumbnailUpload.SaveAs(Server.MapPath(fullPath));

					foreach(var file in picsUpload)
					{
						if(file.ContentLength > 0)
						{
							var path = Path.Combine(directoryPath, Path.GetFileName(file.FileName));
							file.SaveAs(Server.MapPath(path));
						}
					}
				}
				else
				{
					//create directory for images to be added to later
					if (!Directory.Exists(Server.MapPath(directoryPath)))
					{
						Directory.CreateDirectory(Server.MapPath(directoryPath));
					}

				}

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

		[Authorize(Roles = "Admin,Realtor")]
		public ActionResult EditList()
		{
			var listings = from l in db.Listing select l;
			return View(listings.ToList());
		}

        [Authorize(Roles = "Admin,Realtor")]
        public ActionResult Remove()
        {
            var listings = from l in db.Listing select l;

            return View(listings.ToList());
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
			try
			{
				var directoryPath = Path.Combine("~/Content/Images/Listing", listing.ListID.ToString());
				//delete the folder associated with this model.
				Directory.Delete(Server.MapPath(directoryPath), true);
			}
			catch (Exception /*e*/)
			{

				return View("Error");
			}
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
