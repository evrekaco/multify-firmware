namespace DatabaseContext.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UserAccessToken : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.UserProfile", "AccessToken", c => c.String());
            DropColumn("dbo.UserProfile", "FoursquareId");
            DropColumn("dbo.UserProfile", "CheckinCount");
            DropColumn("dbo.UserProfile", "LastUpdated");
        }
        
        public override void Down()
        {
            AddColumn("dbo.UserProfile", "LastUpdated", c => c.DateTime(nullable: false));
            AddColumn("dbo.UserProfile", "CheckinCount", c => c.Int(nullable: false));
            AddColumn("dbo.UserProfile", "FoursquareId", c => c.String());
            DropColumn("dbo.UserProfile", "AccessToken");
        }
    }
}
