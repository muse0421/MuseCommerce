namespace MuseCommerce.Data.Repositories.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class updateACCOUNT : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.MGAccounts", "FPasswordHash", c => c.String(maxLength: 100));
            AddColumn("dbo.MGAccounts", "FSecurityStamp", c => c.String(maxLength: 100));
            AddColumn("dbo.MGAccounts", "FEmail", c => c.String(maxLength: 100));
            AddColumn("dbo.MGAccounts", "FLockoutEnabled", c => c.Boolean(nullable: false));
            AddColumn("dbo.MGAccounts", "FTwoFactorEnabled", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.MGAccounts", "FTwoFactorEnabled");
            DropColumn("dbo.MGAccounts", "FLockoutEnabled");
            DropColumn("dbo.MGAccounts", "FEmail");
            DropColumn("dbo.MGAccounts", "FSecurityStamp");
            DropColumn("dbo.MGAccounts", "FPasswordHash");
        }
    }
}
