﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using RealMax.Models;
using System.Collections;

namespace RealMax.Controllers
{
    public class HomeController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        public PartialViewResult RealtorPartial()
        {
            var realtorModel = from r in db.Realtor select r;

            return PartialView("RealtorPartial", realtorModel.ToList());
        }

        public PartialViewResult ListingPartial()
        {
            var listingModel = from l in db.Listing select l;

            return PartialView("ListingPartial", listingModel.ToList());
        }
    }
}