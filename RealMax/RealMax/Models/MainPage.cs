﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace RealMax.Models
{
    public class MainPage
    {
        public IEnumerable<Listing> Listings { get; set; }
        public IEnumerable<Realtor> Realtors { get; set; }
    }
}