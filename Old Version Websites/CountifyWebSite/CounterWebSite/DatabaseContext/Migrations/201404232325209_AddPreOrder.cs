namespace DatabaseContext.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddPreOrder : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.PreOrder",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        BusinessName = c.String(nullable: false),
                        Name = c.String(nullable: false),
                        EmailAddress = c.String(),
                        PhoneNumber = c.String(),
                        Address = c.String(),
                        Message = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.PreOrder");
        }
    }
}
