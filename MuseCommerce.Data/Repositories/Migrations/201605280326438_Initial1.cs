namespace MuseCommerce.Data.Repositories.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Initial1 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.ItemCores", "CreatedBy", c => c.String(maxLength: 100));
            AlterColumn("dbo.ItemCores", "ModifiedBy", c => c.String(maxLength: 100));
            AlterColumn("dbo.MGAccounts", "CreatedBy", c => c.String(maxLength: 100));
            AlterColumn("dbo.MGAccounts", "ModifiedBy", c => c.String(maxLength: 100));
            AlterColumn("dbo.MGRoles", "CreatedBy", c => c.String(maxLength: 100));
            AlterColumn("dbo.MGRoles", "ModifiedBy", c => c.String(maxLength: 100));
            AlterColumn("dbo.MGPermissions", "CreatedBy", c => c.String(maxLength: 100));
            AlterColumn("dbo.MGPermissions", "ModifiedBy", c => c.String(maxLength: 100));
            AlterColumn("dbo.MGFuncs", "CreatedBy", c => c.String(maxLength: 100));
            AlterColumn("dbo.MGFuncs", "ModifiedBy", c => c.String(maxLength: 100));
            AlterColumn("dbo.PORequests", "CreatedBy", c => c.String(maxLength: 100));
            AlterColumn("dbo.PORequests", "ModifiedBy", c => c.String(maxLength: 100));
            AlterColumn("dbo.PORequestEntries", "CreatedBy", c => c.String(maxLength: 100));
            AlterColumn("dbo.PORequestEntries", "ModifiedBy", c => c.String(maxLength: 100));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.PORequestEntries", "ModifiedBy", c => c.String());
            AlterColumn("dbo.PORequestEntries", "CreatedBy", c => c.String());
            AlterColumn("dbo.PORequests", "ModifiedBy", c => c.String());
            AlterColumn("dbo.PORequests", "CreatedBy", c => c.String());
            AlterColumn("dbo.MGFuncs", "ModifiedBy", c => c.String());
            AlterColumn("dbo.MGFuncs", "CreatedBy", c => c.String());
            AlterColumn("dbo.MGPermissions", "ModifiedBy", c => c.String());
            AlterColumn("dbo.MGPermissions", "CreatedBy", c => c.String());
            AlterColumn("dbo.MGRoles", "ModifiedBy", c => c.String());
            AlterColumn("dbo.MGRoles", "CreatedBy", c => c.String());
            AlterColumn("dbo.MGAccounts", "ModifiedBy", c => c.String());
            AlterColumn("dbo.MGAccounts", "CreatedBy", c => c.String());
            AlterColumn("dbo.ItemCores", "ModifiedBy", c => c.String());
            AlterColumn("dbo.ItemCores", "CreatedBy", c => c.String());
        }
    }
}
