namespace DatabaseContext.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddSubscriptions : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Subscription",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        SubscriptionTypeId = c.Int(nullable: false),
                        Email = c.String(),
                        UserId = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.SubscriptionType", t => t.SubscriptionTypeId, cascadeDelete: true)
                .ForeignKey("dbo.UserProfile", t => t.UserId)
                .Index(t => t.SubscriptionTypeId)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.SubscriptionType",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false),
                        Description = c.String(nullable: false),
                        AdministratorOnly = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Subscription", "UserId", "dbo.UserProfile");
            DropForeignKey("dbo.Subscription", "SubscriptionTypeId", "dbo.SubscriptionType");
            DropIndex("dbo.Subscription", new[] { "UserId" });
            DropIndex("dbo.Subscription", new[] { "SubscriptionTypeId" });
            DropTable("dbo.SubscriptionType");
            DropTable("dbo.Subscription");
        }
    }
}
