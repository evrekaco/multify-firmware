namespace DatabaseContext.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class SubscriptionAccountRequired : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.SubscriptionType", "RequiresAccount", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.SubscriptionType", "RequiresAccount");
        }
    }
}
