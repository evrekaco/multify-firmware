namespace DatabaseContext.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ExtraUserInfo : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.UserProfile", "FoursquareId", c => c.String());
            AddColumn("dbo.UserProfile", "Email", c => c.String());
            AddColumn("dbo.UserProfile", "PhoneNumber", c => c.String());
            AddColumn("dbo.UserProfile", "CheckinCount", c => c.Int(nullable: false));
            AddColumn("dbo.UserProfile", "LastUpdated", c => c.DateTime(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.UserProfile", "LastUpdated");
            DropColumn("dbo.UserProfile", "CheckinCount");
            DropColumn("dbo.UserProfile", "PhoneNumber");
            DropColumn("dbo.UserProfile", "Email");
            DropColumn("dbo.UserProfile", "FoursquareId");
        }
    }
}
