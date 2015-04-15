namespace DatabaseContext.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class VenueManagers : DbMigration
    {
        public override void Up()
        {
            RenameTable(name: "dbo.RequestRecords", newName: "RequestRecord");
            RenameTable(name: "dbo.Venues", newName: "Venue");
            CreateTable(
                "dbo.VenueManager",
                c => new
                    {
                        UserId = c.Int(nullable: false),
                        VenueId = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.UserId, t.VenueId })
                .ForeignKey("dbo.UserProfile", t => t.UserId, cascadeDelete: true)
                .ForeignKey("dbo.Venue", t => t.VenueId, cascadeDelete: true)
                .Index(t => t.UserId)
                .Index(t => t.VenueId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.VenueManager", "VenueId", "dbo.Venue");
            DropForeignKey("dbo.VenueManager", "UserId", "dbo.UserProfile");
            DropIndex("dbo.VenueManager", new[] { "VenueId" });
            DropIndex("dbo.VenueManager", new[] { "UserId" });
            DropTable("dbo.VenueManager");
            RenameTable(name: "dbo.Venue", newName: "Venues");
            RenameTable(name: "dbo.RequestRecord", newName: "RequestRecords");
        }
    }
}
