namespace DatabaseContext.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RequestLogging : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.RequestRecords",
                c => new
                    {
                        ID = c.Long(nullable: false, identity: true),
                        Url = c.String(),
                        RequestType = c.String(),
                        RequestHeader = c.String(),
                        RequestBody = c.String(),
                        IPAddress = c.String(),
                        Action = c.String(),
                        Controller = c.String(),
                        RequestDate = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.ID);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.RequestRecords");
        }
    }
}
