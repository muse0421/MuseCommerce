namespace MuseCommerce.Data.Repositories.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class FForbidden : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.MGRoles", "FForbidden", c => c.Boolean(nullable: false));
            AddColumn("dbo.MGPermissions", "FForbidden", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.MGPermissions", "FForbidden");
            DropColumn("dbo.MGRoles", "FForbidden");
        }
    }
}
