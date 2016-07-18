namespace MuseCommerce.Data.Repositories.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class hr_base : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.HMEmployees",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 100),
                        FNumber = c.String(nullable: false, maxLength: 100),
                        FName = c.String(nullable: false, maxLength: 100),
                        FEmpGroup = c.String(maxLength: 20),
                        FEmpGroupID = c.String(maxLength: 20),
                        FDepartmentID = c.String(maxLength: 100),
                        FDuty = c.String(maxLength: 100),
                        FJobTypeID = c.String(maxLength: 10),
                        FBankID = c.String(maxLength: 100),
                        FBankAccount = c.String(maxLength: 100),
                        FHireDate = c.DateTime(nullable: false),
                        FLeaveDate = c.DateTime(nullable: false),
                        FDeleted = c.Boolean(nullable: false),
                        CreatedDate = c.DateTime(nullable: false),
                        ModifiedDate = c.DateTime(),
                        CreatedBy = c.String(maxLength: 100),
                        ModifiedBy = c.String(maxLength: 100),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.HMPersons",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 100),
                        Code = c.String(nullable: false, maxLength: 20),
                        FName = c.String(nullable: false, maxLength: 20),
                        IDCardID = c.String(maxLength: 30),
                        SexType = c.String(maxLength: 1),
                        Birthday = c.DateTime(nullable: false),
                        Mobile = c.String(maxLength: 20),
                        EMail = c.String(maxLength: 100),
                        MarriedStatus = c.String(maxLength: 1),
                        StartWordTime = c.DateTime(nullable: false),
                        Status = c.String(maxLength: 1),
                        CreatedDate = c.DateTime(nullable: false),
                        ModifiedDate = c.DateTime(),
                        CreatedBy = c.String(maxLength: 100),
                        ModifiedBy = c.String(maxLength: 100),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.ORGDepartments",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 100),
                        SerialNo = c.String(nullable: false, maxLength: 100),
                        Description = c.String(nullable: false, maxLength: 100),
                        CreatedDate = c.DateTime(nullable: false),
                        ModifiedDate = c.DateTime(),
                        CreatedBy = c.String(maxLength: 100),
                        ModifiedBy = c.String(maxLength: 100),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.ORGPDescriptionDuties",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 100),
                        SerialNo = c.String(nullable: false, maxLength: 100),
                        Description = c.String(nullable: false, maxLength: 100),
                        CreatedDate = c.DateTime(nullable: false),
                        ModifiedDate = c.DateTime(),
                        CreatedBy = c.String(maxLength: 100),
                        ModifiedBy = c.String(maxLength: 100),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.ORGPDescriptionDuties");
            DropTable("dbo.ORGDepartments");
            DropTable("dbo.HMPersons");
            DropTable("dbo.HMEmployees");
        }
    }
}
