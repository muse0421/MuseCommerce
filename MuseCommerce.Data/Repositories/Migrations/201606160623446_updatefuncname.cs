namespace MuseCommerce.Data.Repositories.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class updatefuncname : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.MGFuncs", "IsActive");
        }
        
        public override void Down()
        {
            AddColumn("dbo.MGFuncs", "IsActive", c => c.Boolean(nullable: false));
        }
    }
}
