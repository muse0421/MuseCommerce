namespace MuseCommerce.Data.Repositories.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Initial : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.ItemCores",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 100),
                        FNumber = c.String(nullable: false, maxLength: 100),
                        FShortNumber = c.String(maxLength: 100),
                        FHelpCode = c.String(maxLength: 100),
                        FName = c.String(nullable: false, maxLength: 100),
                        FModel = c.String(maxLength: 100),
                        FDeleted = c.Short(),
                        CreatedDate = c.DateTime(nullable: false),
                        ModifiedDate = c.DateTime(),
                        CreatedBy = c.String(),
                        ModifiedBy = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.MGAccounts",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 100),
                        FUseName = c.String(nullable: false, maxLength: 100),
                        FAccountState = c.Int(nullable: false),
                        FIsAdministrator = c.Boolean(nullable: false),
                        FUserType = c.Int(nullable: false),
                        CreatedDate = c.DateTime(nullable: false),
                        ModifiedDate = c.DateTime(),
                        CreatedBy = c.String(),
                        ModifiedBy = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.MGRoles",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 100),
                        FName = c.String(nullable: false, maxLength: 100),
                        FDescription = c.String(nullable: false, maxLength: 100),
                        CreatedDate = c.DateTime(nullable: false),
                        ModifiedDate = c.DateTime(),
                        CreatedBy = c.String(),
                        ModifiedBy = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.MGPermissions",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 100),
                        FName = c.String(nullable: false, maxLength: 100),
                        FDescription = c.String(nullable: false, maxLength: 100),
                        CreatedDate = c.DateTime(nullable: false),
                        ModifiedDate = c.DateTime(),
                        CreatedBy = c.String(),
                        ModifiedBy = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.MGFuncs",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 100),
                        FName = c.String(nullable: false, maxLength: 100),
                        FUrl = c.String(nullable: false, maxLength: 100),
                        IsActive = c.Boolean(nullable: false),
                        FPriority = c.Int(nullable: false),
                        FParentID = c.String(maxLength: 100),
                        FPermissionID = c.String(maxLength: 100),
                        CreatedDate = c.DateTime(nullable: false),
                        ModifiedDate = c.DateTime(),
                        CreatedBy = c.String(),
                        ModifiedBy = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.MGFuncs", t => t.FParentID)
                .ForeignKey("dbo.MGPermissions", t => t.FPermissionID)
                .Index(t => t.FParentID)
                .Index(t => t.FPermissionID);
            
            CreateTable(
                "dbo.PORequests",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 100),
                        FBillNo = c.String(maxLength: 100),
                        FDate = c.DateTime(nullable: false),
                        FTranType = c.Int(nullable: false),
                        FStatus = c.String(maxLength: 100),
                        FNote = c.String(maxLength: 200),
                        PoAddress_StreetNumber = c.String(maxLength: 50),
                        PoAddress_StreetName = c.String(maxLength: 200),
                        CreatedDate = c.DateTime(nullable: false),
                        ModifiedDate = c.DateTime(),
                        CreatedBy = c.String(),
                        ModifiedBy = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.PORequestEntries",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 100),
                        FInterID = c.String(maxLength: 100),
                        FQty = c.Decimal(nullable: false, precision: 18, scale: 2),
                        FPrice = c.Decimal(nullable: false, precision: 18, scale: 2),
                        FSecQty = c.Decimal(nullable: false, precision: 18, scale: 2),
                        FItemID = c.String(maxLength: 100),
                        CreatedDate = c.DateTime(nullable: false),
                        ModifiedDate = c.DateTime(),
                        CreatedBy = c.String(),
                        ModifiedBy = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.ItemCores", t => t.FItemID)
                .ForeignKey("dbo.PORequests", t => t.FInterID)
                .Index(t => t.FInterID)
                .Index(t => t.FItemID);
            
            CreateTable(
                "dbo.MGRolePermission",
                c => new
                    {
                        FRoleID = c.String(nullable: false, maxLength: 100),
                        FPermissionID = c.String(nullable: false, maxLength: 100),
                    })
                .PrimaryKey(t => new { t.FRoleID, t.FPermissionID })
                .ForeignKey("dbo.MGRoles", t => t.FRoleID, cascadeDelete: true)
                .ForeignKey("dbo.MGPermissions", t => t.FPermissionID, cascadeDelete: true)
                .Index(t => t.FRoleID)
                .Index(t => t.FPermissionID);
            
            CreateTable(
                "dbo.MGRoleAssignment",
                c => new
                    {
                        FAccountID = c.String(nullable: false, maxLength: 100),
                        FRoleID = c.String(nullable: false, maxLength: 100),
                    })
                .PrimaryKey(t => new { t.FAccountID, t.FRoleID })
                .ForeignKey("dbo.MGAccounts", t => t.FAccountID, cascadeDelete: true)
                .ForeignKey("dbo.MGRoles", t => t.FRoleID, cascadeDelete: true)
                .Index(t => t.FAccountID)
                .Index(t => t.FRoleID);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.PORequestEntries", "FInterID", "dbo.PORequests");
            DropForeignKey("dbo.PORequestEntries", "FItemID", "dbo.ItemCores");
            DropForeignKey("dbo.MGFuncs", "FPermissionID", "dbo.MGPermissions");
            DropForeignKey("dbo.MGFuncs", "FParentID", "dbo.MGFuncs");
            DropForeignKey("dbo.MGRoleAssignment", "FRoleID", "dbo.MGRoles");
            DropForeignKey("dbo.MGRoleAssignment", "FAccountID", "dbo.MGAccounts");
            DropForeignKey("dbo.MGRolePermission", "FPermissionID", "dbo.MGPermissions");
            DropForeignKey("dbo.MGRolePermission", "FRoleID", "dbo.MGRoles");
            DropIndex("dbo.MGRoleAssignment", new[] { "FRoleID" });
            DropIndex("dbo.MGRoleAssignment", new[] { "FAccountID" });
            DropIndex("dbo.MGRolePermission", new[] { "FPermissionID" });
            DropIndex("dbo.MGRolePermission", new[] { "FRoleID" });
            DropIndex("dbo.PORequestEntries", new[] { "FItemID" });
            DropIndex("dbo.PORequestEntries", new[] { "FInterID" });
            DropIndex("dbo.MGFuncs", new[] { "FPermissionID" });
            DropIndex("dbo.MGFuncs", new[] { "FParentID" });
            DropTable("dbo.MGRoleAssignment");
            DropTable("dbo.MGRolePermission");
            DropTable("dbo.PORequestEntries");
            DropTable("dbo.PORequests");
            DropTable("dbo.MGFuncs");
            DropTable("dbo.MGPermissions");
            DropTable("dbo.MGRoles");
            DropTable("dbo.MGAccounts");
            DropTable("dbo.ItemCores");
        }
    }
}
