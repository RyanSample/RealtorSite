﻿using System;
using System.Data.Entity;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Collections;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
//TODO:
/* http://www.asp.net/web-api/overview/data/using-web-api-with-entity-framework/part-2
 * http://www.asp.net/web-api/overview/data/using-web-api-with-entity-framework/part-3
 * http://www.asp.net/web-api/overview/data/using-web-api-with-entity-framework/part-4
 * http://www.asp.net/mvc/overview/getting-started/getting-started-with-ef-using-mvc/migrations-and-deployment-with-the-entity-framework-in-an-asp-net-mvc-application
 * */
namespace RealMax.Models
{
    public class Listing
    {
        [Key]
        public int ListID { get; set; }

        [Required]
        public string HouseNumber { get; set; }

        [Required]
        public string StreetName { get; set; }

        public string ApartmentNumber { get; set; }

        [Required]
        public string City { get; set; }

        [Required]
        public string State { get; set; }

        public int ZipCode { get; set; }

        [DisplayFormat(DataFormatString = "{0:C0}")]
        public int Price { get; set; }

        [Display(Name = "Bedrooms")]
        public int Bed { get; set; }

        [Display(Name = "Bathrooms")]
        public int Bath { get; set; }

        //Foreign Key
        public int RealtorID { get; set; }

        public virtual Realtor Realtor { get; set; }

        public string ExtraFeatures { get; set; }

        [Display(Name = "Sq. Feet")]
        public int SquareFeet { get; set; }

        public int? LotSize { get; set; }

    }
}
