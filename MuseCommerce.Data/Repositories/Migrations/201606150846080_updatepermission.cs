namespace MuseCommerce.Data.Repositories.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class updatepermission : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.MGPermissions", "FCode", c => c.String(maxLength: 100));
        }
        
        public override void Down()
        {
            DropColumn("dbo.MGPermissions", "FCode");
        }
    }
}
