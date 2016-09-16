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
        //TODO: create DBSeed
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
                new Realtor { FirstName = "Tony", LastName = "Johnson", Email = "tjohnson@gmail.com", PhoneNumber = 1231457890 },
                new Realtor { FirstName = "Sarah", LastName = "Sanders", Email = "ssanders@realmax.com", PhoneNumber = 1234561987 },
                new Realtor { FirstName = "Jessica", LastName = "Gray", Email = "jgray@realmax.com", PhoneNumber = 1234577892 },
                new Realtor { FirstName = "Sam", LastName = "Hansen", Email = "shansen@realmax.com", PhoneNumber = 1234567432 }
            };
            realtors.ForEach(s => context.Realtor.AddOrUpdate(p => p.Email, s));
            context.SaveChanges();

            var listings = new List<Listing>
            {
                new Listing {ListID=1, HouseNumber = "227", StreetName = "W Virginia Ave", City = "Peoria", State = "IL", ZipCode = 61604,
                    Price = 110000, Bed = 3, Bath = 2, SquareFeet = 1230, RealtorID = realtors.Single(s => s.Email == "rsample@realmax.com").ID}
            };
            listings.ForEach(s => context.Listing.AddOrUpdate(p => p.ListID, s));
            context.SaveChanges();
        }
    }
}
