namespace ClothBazar.Database.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CountryAdded : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Registrations", "Country", c => c.String(nullable: false));
            DropColumn("dbo.Registrations", "LastName");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Registrations", "LastName", c => c.String(nullable: false));
            DropColumn("dbo.Registrations", "Country");
        }
    }
}
