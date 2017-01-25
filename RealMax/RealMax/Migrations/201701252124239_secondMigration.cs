namespace RealMax.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class secondMigration : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Listings", "LotSize", c => c.Int());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Listings", "LotSize", c => c.Int(nullable: false));
        }
    }
}
