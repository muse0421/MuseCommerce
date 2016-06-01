namespace MuseCommerce.Data.Repositories.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Initial2 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.ItemCores", "CreatedDate", c => c.DateTime(nullable: false,defaultValueSql:"getdate()"));
            AlterColumn("dbo.MGAccounts", "CreatedDate", c => c.DateTime(nullable: false,defaultValueSql:"getdate()"));
            AlterColumn("dbo.MGRoles", "CreatedDate", c => c.DateTime(nullable: false,defaultValueSql:"getdate()"));
            AlterColumn("dbo.MGPermissions", "CreatedDate", c => c.DateTime(nullable: false,defaultValueSql:"getdate()"));
            AlterColumn("dbo.MGFuncs", "CreatedDate", c => c.DateTime(nullable: false,defaultValueSql:"getdate()"));
            AlterColumn("dbo.PORequests", "CreatedDate", c => c.DateTime(nullable: false,defaultValueSql:"getdate()"));
            AlterColumn("dbo.PORequestEntries", "CreatedDate", c => c.DateTime(nullable: false,defaultValueSql:"getdate()"));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.PORequestEntries", "CreatedDate", c => c.DateTime(nullable: false));
            AlterColumn("dbo.PORequests", "CreatedDate", c => c.DateTime(nullable: false));
            AlterColumn("dbo.MGFuncs", "CreatedDate", c => c.DateTime(nullable: false));
            AlterColumn("dbo.MGPermissions", "CreatedDate", c => c.DateTime(nullable: false));
            AlterColumn("dbo.MGRoles", "CreatedDate", c => c.DateTime(nullable: false));
            AlterColumn("dbo.MGAccounts", "CreatedDate", c => c.DateTime(nullable: false));
            AlterColumn("dbo.ItemCores", "CreatedDate", c => c.DateTime(nullable: false));
        }
    }
}
