namespace DatabaseContext.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddPushCheckins : DbMigration
    {
        public override void Up()
        {
            //NOTE:manually changed
            RenameColumn("dbo.Venue", "Checkins", "CheckinCount");
            //AddColumn("dbo.Venue", "CheckinCount", c => c.Int(nullable: false));
            //DropColumn("dbo.Venue", "Checkins");

            CreateTable(
                "dbo.Checkin",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        UserId = c.String(),
                        UserName = c.String(),
                        CheckinTime = c.DateTime(nullable: false),
                        VenueId = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Venue", t => t.VenueId, cascadeDelete: true)
                .Index(t => t.VenueId);

            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Checkin", "VenueId", "dbo.Venue");
            DropIndex("dbo.Checkin", new[] { "VenueId" });
            DropTable("dbo.Checkin");

            //NOTE:manually changed
            RenameColumn("dbo.Venue", "CheckinCount", "Checkins");
            AddColumn("dbo.Venue", "Checkins", c => c.Int(nullable: false));
            DropColumn("dbo.Venue", "CheckinCount");
        }
    }
}
