﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RealMax;
using RealMax.Controllers;
using RealMax.Models;

namespace RealMax.Tests.Controllers
{
    [TestClass]
    public class HomeControllerTest
    {
        [TestMethod]
        public void Index()
        {
            // Arrange
            HomeController controller = new HomeController();

            // Act
            ViewResult result = controller.Index() as ViewResult;

            // Assert
            Assert.IsNotNull(result);
        }

        //[TestMethod]
        //public void About()
        //{
        //    // Arrange
        //    HomeController controller = new HomeController();

        //    // Act
        //    ViewResult result = controller.About() as ViewResult;

        //    // Assert
        //    Assert.AreEqual("Your application description page.", result.ViewBag.Message);
        //}

        //[TestMethod]
        //public void Contact()
        //{
        //    // Arrange
        //    HomeController controller = new HomeController();

        //    // Act
        //    ViewResult result = controller.Contact() as ViewResult;

        //    // Assert
        //    Assert.IsNotNull(result);
        //}
    }

	[TestClass]
	public class RealtorControllerTest
	{
		[TestMethod]
		public void Index()
		{
			//TODO: http://stackoverflow.com/questions/4839459/asp-net-mvc-controller-unit-test-fails
			var list = new List<Realtor>();
			var queryableList = list.AsQueryable();
			RealtorController controller = new RealtorController(queryableList);

			ViewResult result = controller.Index(null) as ViewResult;

			Assert.IsNotNull(result);
		}
	}
}
