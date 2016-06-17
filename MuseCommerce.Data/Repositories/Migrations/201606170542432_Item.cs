namespace MuseCommerce.Data.Repositories.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Item : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.PORequests", "FStatus", c => c.String(maxLength: 100));
            AlterColumn("dbo.ItemCores", "FDeleted", c => c.Short());
            DropColumn("dbo.PORequests", "FDeleted");
            DropColumn("dbo.PORequests", "FForbidden");
            DropColumn("dbo.MGPermissions", "FForbidden");
            DropColumn("dbo.MGRoles", "FForbidden");
            //DropColumn("dbo.ItemCores", "FForbidden");
            //DropColumn("dbo.MGRoles", "FForbidden");
            DropColumn("dbo.ItemCores", "FForbidden");
            AddColumn("dbo.ItemCores", "FForbidden", c => c.Boolean(nullable: false));
            AddColumn("dbo.MGRoles", "FForbidden", c => c.Boolean(nullable: false));
            AddColumn("dbo.MGPermissions", "FForbidden", c => c.Boolean(nullable: false));
            AddColumn("dbo.PORequests", "FForbidden", c => c.Boolean(nullable: false));
            AddColumn("dbo.PORequests", "FDeleted", c => c.Boolean(nullable: false));
            AlterColumn("dbo.ItemCores", "FDeleted", c => c.Boolean(nullable: false));
            AlterColumn("dbo.PORequests", "FStatus", c => c.String(maxLength: 10));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.PORequests", "FStatus", c => c.String(maxLength: 100));
            AlterColumn("dbo.ItemCores", "FDeleted", c => c.Short());
            DropColumn("dbo.PORequests", "FDeleted");
            DropColumn("dbo.PORequests", "FForbidden");
            DropColumn("dbo.MGPermissions", "FForbidden");
            DropColumn("dbo.MGRoles", "FForbidden");
            DropColumn("dbo.ItemCores", "FForbidden");
        }
    }
}
