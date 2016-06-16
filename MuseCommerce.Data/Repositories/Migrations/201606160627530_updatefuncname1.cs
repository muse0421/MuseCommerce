namespace MuseCommerce.Data.Repositories.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class updatefuncname1 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.MGFuncs", "IsActive", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
        }
    }
}
