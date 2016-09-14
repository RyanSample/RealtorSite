using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace RealMax.Models
{
    public class Realtor
    {
        public int ID { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Email { get; set; }

        public int PhoneNumber { get; set; }
        
        public string Bio { get; set; } 

        public virtual ICollection<Listing> Listings { get; set; }
    }
}