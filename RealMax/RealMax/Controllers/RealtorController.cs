using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using RealMax.Models;
using System.IO;

namespace RealMax.Controllers
{
    public class RealtorController : Controller
    {
        private ApplicationDbContext _db = new ApplicationDbContext();
		private IQueryable<Realtor> _realtorDbList;

		public RealtorController()
		{
			_realtorDbList = from r in _db.Realtor select r;

		}

		public RealtorController(IQueryable<Realtor> realtorDbList)
		{
			_realtorDbList = realtorDbList;
		}

        // GET: Realtor
        public ViewResult Index(string id)
        {
            string searchString = id;
            var realtors = _realtorDbList /*from r in _db.Realtor select r*/;

            if (!String.IsNullOrEmpty(searchString))
            {
                realtors = realtors.Where(r => r.LastName.Contains(searchString) || r.FirstName.Contains(searchString));
            }
            return View(realtors.ToList());
        }

        // GET: Realtor/Details/5
        public ActionResult Details(int? id)
        {
			//var realtorTest = _realtorDbList.Where(r => r.ID.Equals(id)); 
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Realtor realtor = _db.Realtor.Find(id);
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
        public ActionResult Create([Bind(Include = "ID,FirstName,LastName,Email,PhoneNumber,Bio")] Realtor realtor, HttpPostedFileBase profilePic)
        {
            if (ModelState.IsValid)
            {
                _db.Realtor.Add(realtor);
                _db.SaveChanges();

				int id = realtor.ID;
				var directoryPath = Path.Combine("~/Content/Images/Realtor", id.ToString());

				if (profilePic != null)
				{
					var fileExtension = Path.GetExtension(profilePic.FileName);
					var fileName = "thumbnail" + fileExtension;
					var fullPath = Path.Combine(directoryPath, fileName);

					if (!Directory.Exists(Server.MapPath(directoryPath)))
						Directory.CreateDirectory(Server.MapPath(directoryPath));

					profilePic.SaveAs(Server.MapPath(fullPath));

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
            Realtor realtor = _db.Realtor.Find(id);
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
        public ActionResult Edit([Bind(Include = "ID,FirstName,LastName,Email,PhoneNumber,Bio")] Realtor realtor, HttpPostedFileBase addOrChangePicture, bool RemovePicture)
        {
			if (ModelState.IsValid)
			{
				_db.Entry(realtor).State = EntityState.Modified;
				_db.SaveChanges();
				if(addOrChangePicture != null && addOrChangePicture.ContentLength > 0)
				{
					//if there is already a picture then we need to remove it and upload the new one as the same name
					var directoryPath = Path.Combine("~/Content/Images/Realtor", realtor.ID.ToString());
					string fileName = "thumbnail.jpg";
					string filePath = Path.Combine(directoryPath, fileName);
					//check for directory path and create it if it has not already been done(I goofed when i was originaly creating the realtor data and didn't add directories)
					if (!Directory.Exists(Server.MapPath(directoryPath))){
						Directory.CreateDirectory(Server.MapPath(directoryPath));
					}
					//check for and then delete previous picture using helper function
					removeRealtorPicture(realtor.ID);

					try {
						//save the picture
						addOrChangePicture.SaveAs(Server.MapPath(filePath));
					}
					catch (Exception /*e*/)
					{						
						//Found this while digging through the error view; It doesnt seem like the error view actually uses this model.
						//System.Web.Mvc.HandleErrorInfo errorMessage = new System.Web.Mvc.HandleErrorInfo(e, "Realtor", "Edit");
						return View("Error");
					}
					

				}

				if(RemovePicture == true)
				{
					removeRealtorPicture(realtor.ID);
				}

				return RedirectToAction("Index");
			}
            return View(realtor);
        }

		[Authorize(Roles = "Admin")]
		public ActionResult EditList()
		{
			var list = _realtorDbList;
			return View(list.ToList());
		}

		[Authorize(Roles = "Admin")]
		public ActionResult Remove()
        {
            var realtors = from r in _db.Realtor select r;
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
            Realtor realtor = _db.Realtor.Find(id);
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
            Realtor realtor = _db.Realtor.Find(id);
            _db.Realtor.Remove(realtor);
            _db.SaveChanges();

			try
			{
				var directoryPath = Path.Combine("~/Content/Images/Realtor", realtor.ID.ToString());
				//delete the folder associated with this model.
				Directory.Delete(Server.MapPath(directoryPath), true);
			}
			catch (Exception e)
			{
				return View("Error", e);
			}

			return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _db.Dispose();
            }
            base.Dispose(disposing);
        }

		private void removeRealtorPicture(int id) 
		{
			try
			{			
				var directoryPath = Path.Combine("~/Content/Images/Realtor", id.ToString());
				string fileName = "thumbnail.jpg";
				string filePath = Path.Combine(directoryPath, fileName);
				//check for and then delete file
				if (System.IO.File.Exists(Server.MapPath(filePath)))
					System.IO.File.Delete(Server.MapPath(filePath));
				}
			catch (Exception)
			{
				
			}
		}
    }
}
