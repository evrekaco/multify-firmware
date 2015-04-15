namespace DatabaseContext.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddVenue : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Venues",
                c => new
                    {
                        VenueId = c.String(nullable: false, maxLength: 128),
                        Name = c.String(),
                        Checkins = c.Int(nullable: false),
                        lastUpdated = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.VenueId);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.Venues");
        }
    }
}
