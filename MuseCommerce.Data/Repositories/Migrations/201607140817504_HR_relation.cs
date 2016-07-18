namespace MuseCommerce.Data.Repositories.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class HR_relation : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.HMEmployees", "FLeaveDate", c => c.DateTime());
            CreateIndex("dbo.HMEmployees", "FDepartmentID");
            CreateIndex("dbo.HMEmployees", "FDuty");
            AddForeignKey("dbo.HMEmployees", "FDepartmentID", "dbo.ItemCores", "Id");
            AddForeignKey("dbo.HMEmployees", "FDuty", "dbo.ItemCores", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.HMEmployees", "FDuty", "dbo.ItemCores");
            DropForeignKey("dbo.HMEmployees", "FDepartmentID", "dbo.ItemCores");
            DropIndex("dbo.HMEmployees", new[] { "FDuty" });
            DropIndex("dbo.HMEmployees", new[] { "FDepartmentID" });
            AlterColumn("dbo.HMEmployees", "FLeaveDate", c => c.DateTime(nullable: false));
        }
    }
}
