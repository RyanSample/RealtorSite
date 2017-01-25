namespace RealMax.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class secondMigration1 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Listings", "HouseNumber", c => c.String(nullable: false));
            AlterColumn("dbo.Listings", "StreetName", c => c.String(nullable: false));
            AlterColumn("dbo.Listings", "City", c => c.String(nullable: false));
            AlterColumn("dbo.Listings", "State", c => c.String(nullable: false));
            AlterColumn("dbo.Realtors", "FirstName", c => c.String(nullable: false));
            AlterColumn("dbo.Realtors", "LastName", c => c.String(nullable: false));
            AlterColumn("dbo.Realtors", "PhoneNumber", c => c.Int());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Realtors", "PhoneNumber", c => c.Int(nullable: false));
            AlterColumn("dbo.Realtors", "LastName", c => c.String());
            AlterColumn("dbo.Realtors", "FirstName", c => c.String());
            AlterColumn("dbo.Listings", "State", c => c.String());
            AlterColumn("dbo.Listings", "City", c => c.String());
            AlterColumn("dbo.Listings", "StreetName", c => c.String());
            AlterColumn("dbo.Listings", "HouseNumber", c => c.String());
        }
    }
}
