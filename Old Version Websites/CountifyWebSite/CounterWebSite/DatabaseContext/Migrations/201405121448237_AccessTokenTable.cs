namespace DatabaseContext.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AccessTokenTable : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.AccessToken",
                c => new
                    {
                        FoursquareId = c.String(nullable: false, maxLength: 128),
                        Token = c.String(),
                    })
                .PrimaryKey(t => t.FoursquareId);

            //TODO: table prefixes are hardcoded, but different in local and production settingss
            //populate access token table
            Sql("INSERT INTO dbo.AccessToken SELECT ProviderUserId AS FoursquareId, AccessToken FROM dbo.UserProfile AS users JOIN dbo.webpages_OAuthMembership as membership on users.UserId = membership.UserId");

            //add the user profile column
            AddColumn("dbo.UserProfile", "FoursquareId", c => c.String(nullable: false, maxLength: 128));

            //add foursquare ids to userprofiles
            Sql("UPDATE dbo.UserProfile SET FoursquareId = (Select ProviderUserId AS FourSquareId FROM dbo.webpages_OAuthMembership AS membership WHERE membership.UserId = UserProfile.UserId)");

            //create the index and foriegn key on the column
            CreateIndex("dbo.UserProfile", "FoursquareId");
            AddForeignKey("dbo.UserProfile", "FoursquareId", "dbo.AccessToken", "FoursquareId", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.UserProfile", "FoursquareId", "dbo.AccessToken");
            DropIndex("dbo.UserProfile", new[] { "FoursquareId" });
            DropColumn("dbo.UserProfile", "FoursquareId");
            DropTable("dbo.AccessToken");
        }
    }
}
