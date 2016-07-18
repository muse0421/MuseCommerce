namespace MuseCommerce.Data.Repositories.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class HR_EMP : DbMigration
    {
        public override void Up()
        {
            RenameColumn(table: "dbo.HMEmployees", name: "FDuty", newName: "FDutyID");
            RenameIndex(table: "dbo.HMEmployees", name: "IX_FDuty", newName: "IX_FDutyID");
        }
        
        public override void Down()
        {
            RenameIndex(table: "dbo.HMEmployees", name: "IX_FDutyID", newName: "IX_FDuty");
            RenameColumn(table: "dbo.HMEmployees", name: "FDutyID", newName: "FDuty");
        }
    }
}
