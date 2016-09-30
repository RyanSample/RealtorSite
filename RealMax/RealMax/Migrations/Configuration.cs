namespace RealMax.Migrations
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;
    using RealMax.Models;
    using System.Collections.Generic;
    internal sealed class Configuration : DbMigrationsConfiguration<RealMax.Models.ApplicationDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }
       
        protected override void Seed(RealMax.Models.ApplicationDbContext context)
        {
            //  This method will be called after migrating to the latest version.

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method 
            //  to avoid creating duplicate seed data. E.g.
            //
            //    context.People.AddOrUpdate(
            //      p => p.FullName,
            //      new Person { FullName = "Andrew Peters" },
            //      new Person { FullName = "Brice Lambson" },
            //      new Person { FullName = "Rowan Miller" }
            //    );
            //

            var realtors = new List<Realtor>
            {
                new Realtor { FirstName = "Ryan", LastName = "Sample", Email = "rsample@realmax.com", PhoneNumber = 1234567892},
                new Realtor { FirstName = "Tony", LastName = "Johnson", Email = "tjohnson@realmax.com", PhoneNumber = 1231457890 },
                new Realtor { FirstName = "Sarah", LastName = "Sanders", Email = "ssanders@realmax.com", PhoneNumber = 1234561987 },
                new Realtor { FirstName = "Jessica", LastName = "Gray", Email = "jgray@realmax.com", PhoneNumber = 1234577892 },
                new Realtor { FirstName = "Sam", LastName = "Hansen", Email = "shansen@realmax.com", PhoneNumber = 1234567432 }
            };
            realtors.ForEach(s => context.Realtor.AddOrUpdate(p => p.PhoneNumber, s));//TODO: change back to email after updating database
            context.SaveChanges();

            var listings = new List<Listing>
            {
                new Listing {ListID=1, HouseNumber = "227", StreetName = "W Virginia Ave", City = "Peoria", State = "IL", ZipCode = 61604,
                    Price = 110000, Bed = 3, Bath = 2, SquareFeet = 1230, RealtorID = realtors.Single(s => s.Email == "rsample@realmax.com").ID},

                new Listing {ListID=2, HouseNumber = "123", StreetName = "W Grand St", City = "Dunlap", State="IL", ZipCode = 61525,
                    Price = 89000, Bed = 2, Bath = 1, SquareFeet = 995, RealtorID = realtors.Single(s => s.Email == "shansen@realmax.com").ID},

                new Listing {ListID=3, HouseNumber = "1354", StreetName = "E Jackson St", City = "Peoria Heights", State = "IL", ZipCode = 61616,
                    Price = 95500, Bed = 3, Bath = 1, SquareFeet= 1570, RealtorID = realtors.Single(s => s.Email == "ssanders@realmax.com").ID},

                new Listing {ListID=4, HouseNumber = "2417", StreetName = "Piper Rd", City = "West Peoria", State = "IL",ZipCode = 61604, Price = 76300,
                Bed = 3, Bath = 2, SquareFeet = 1100, RealtorID = realtors.Single(s => s.Email == "jgray@realmax.com").ID},

                new Listing {ListID=5, HouseNumber = "8074", StreetName = "Hilldale Dr", City = "Bellevue", State = "IL", ZipCode = 61604,
                    Price = 93150, Bed = 3, Bath = 2, SquareFeet = 1360, RealtorID = realtors.Single(s => s.Email == "tjohnson@realmax.com").ID},

                new Listing {ListID=6, HouseNumber = "26", StreetName = "Oak Ct", City = "Peoria", State = "IL", ZipCode = 61615,
                    Price = 153000, Bed = 4, Bath = 2, SquareFeet = 1750, RealtorID = realtors.Single(s => s.Email == "jgray@realmax.com").ID},

                new Listing {ListID=7, HouseNumber = "562", StreetName = "Lincoln Rd", City = "Peoria Heights", State = "IL", ZipCode = 61616,
                    Price = 121560, Bed = 4, Bath = 2, SquareFeet = 1890, RealtorID = realtors.Single(s => s.Email == "shansen@realmax.com").ID},

                new Listing {ListID=8, HouseNumber = "992", StreetName = "Cherry St", City = "Peoria", State = "IL", ZipCode = 61615,
                    Price = 189180, Bed = 4, Bath = 3, SquareFeet = 2300, RealtorID = realtors.Single(s => s.Email == "ssanders@realmax.com").ID},                

                new Listing {ListID=9, HouseNumber = "755", StreetName = "Maple Ln", City = "Bellevue", State = "IL", ZipCode = 61604,
                    Price = 75760, Bed = 2, Bath = 1, SquareFeet = 1120, RealtorID= realtors.Single(s => s.Email == "jgray@realmax.com").ID },

                new Listing {ListID=10, HouseNumber = "2892", StreetName= "College St", City = "Peoria", State="IL", ZipCode = 61604,
                    Price = 45560, Bed = 2, Bath=1, SquareFeet = 780, RealtorID = realtors.Single(s => s.Email == "tjohnson@realmax.com").ID}

            };
            listings.ForEach(s => context.Listing.AddOrUpdate(p => p.ListID, s));
            context.SaveChanges();
        }
    }
}
