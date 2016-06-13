namespace MuseCommerce.Data.Repositories.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class updateACCOUNT1 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.MGAccounts", "FPasswordHash", c => c.String(nullable: false, maxLength: 100));
            AlterColumn("dbo.MGAccounts", "FSecurityStamp", c => c.String(nullable: false, maxLength: 100));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.MGAccounts", "FSecurityStamp", c => c.String(maxLength: 100));
            AlterColumn("dbo.MGAccounts", "FPasswordHash", c => c.String(maxLength: 100));
        }
    }
}
