namespace EmbeddedStockTest.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ComplexDataModel : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Category",
                c => new
                    {
                        CategoryId = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                    })
                .PrimaryKey(t => t.CategoryId);
            
            CreateTable(
                "dbo.ComponentType",
                c => new
                    {
                        ComponentTypeId = c.Long(nullable: false, identity: true),
                        ComponentName = c.String(),
                        ComponentInfo = c.String(),
                        Location = c.String(),
                        Status = c.Int(nullable: false),
                        Datasheet = c.String(),
                        ImageUrl = c.String(),
                        Manufacturer = c.String(),
                        WikiLink = c.String(),
                        AdminComment = c.String(),
                        Image_ESImageId = c.Long(),
                    })
                .PrimaryKey(t => t.ComponentTypeId)
                .ForeignKey("dbo.ESImage", t => t.Image_ESImageId)
                .Index(t => t.Image_ESImageId);
            
            CreateTable(
                "dbo.Component",
                c => new
                    {
                        ComponentId = c.Long(nullable: false, identity: true),
                        ComponentTypeId = c.Long(nullable: false),
                        ComponentNumber = c.Int(nullable: false),
                        SerialNo = c.String(),
                        Status = c.Int(nullable: false),
                        AdminComment = c.String(),
                        UserComment = c.String(),
                        CurrentLoanInformationId = c.Long(),
                    })
                .PrimaryKey(t => t.ComponentId)
                .ForeignKey("dbo.ComponentType", t => t.ComponentTypeId)
                .Index(t => t.ComponentTypeId);
            
            CreateTable(
                "dbo.ESImage",
                c => new
                    {
                        ESImageId = c.Long(nullable: false, identity: true),
                        ImageMimeType = c.String(maxLength: 128),
                        Thumbnail = c.Binary(),
                        ImageData = c.Binary(),
                    })
                .PrimaryKey(t => t.ESImageId);
            
            CreateTable(
                "dbo.AspNetRoles",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Name = c.String(nullable: false, maxLength: 256),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.Name, unique: true, name: "RoleNameIndex");
            
            CreateTable(
                "dbo.AspNetUserRoles",
                c => new
                    {
                        UserId = c.String(nullable: false, maxLength: 128),
                        RoleId = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.UserId, t.RoleId })
                .ForeignKey("dbo.AspNetRoles", t => t.RoleId)
                .ForeignKey("dbo.AspNetUsers", t => t.UserId)
                .Index(t => t.UserId)
                .Index(t => t.RoleId);
            
            CreateTable(
                "dbo.AspNetUsers",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Email = c.String(maxLength: 256),
                        EmailConfirmed = c.Boolean(nullable: false),
                        PasswordHash = c.String(),
                        SecurityStamp = c.String(),
                        PhoneNumber = c.String(),
                        PhoneNumberConfirmed = c.Boolean(nullable: false),
                        TwoFactorEnabled = c.Boolean(nullable: false),
                        LockoutEndDateUtc = c.DateTime(),
                        LockoutEnabled = c.Boolean(nullable: false),
                        AccessFailedCount = c.Int(nullable: false),
                        UserName = c.String(nullable: false, maxLength: 256),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.UserName, unique: true, name: "UserNameIndex");
            
            CreateTable(
                "dbo.AspNetUserClaims",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UserId = c.String(nullable: false, maxLength: 128),
                        ClaimType = c.String(),
                        ClaimValue = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.UserId)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.AspNetUserLogins",
                c => new
                    {
                        LoginProvider = c.String(nullable: false, maxLength: 128),
                        ProviderKey = c.String(nullable: false, maxLength: 128),
                        UserId = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.LoginProvider, t.ProviderKey, t.UserId })
                .ForeignKey("dbo.AspNetUsers", t => t.UserId)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.CategoryComponentType",
                c => new
                    {
                        CategoryRefId = c.Int(nullable: false),
                        ComponentTypeRefId = c.Long(nullable: false),
                    })
                .PrimaryKey(t => new { t.CategoryRefId, t.ComponentTypeRefId })
                .ForeignKey("dbo.Category", t => t.CategoryRefId, cascadeDelete: true)
                .ForeignKey("dbo.ComponentType", t => t.ComponentTypeRefId, cascadeDelete: true)
                .Index(t => t.CategoryRefId)
                .Index(t => t.ComponentTypeRefId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.AspNetUserRoles", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserLogins", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserClaims", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserRoles", "RoleId", "dbo.AspNetRoles");
            DropForeignKey("dbo.CategoryComponentType", "ComponentTypeRefId", "dbo.ComponentType");
            DropForeignKey("dbo.CategoryComponentType", "CategoryRefId", "dbo.Category");
            DropForeignKey("dbo.ComponentType", "Image_ESImageId", "dbo.ESImage");
            DropForeignKey("dbo.Component", "ComponentTypeId", "dbo.ComponentType");
            DropIndex("dbo.CategoryComponentType", new[] { "ComponentTypeRefId" });
            DropIndex("dbo.CategoryComponentType", new[] { "CategoryRefId" });
            DropIndex("dbo.AspNetUserLogins", new[] { "UserId" });
            DropIndex("dbo.AspNetUserClaims", new[] { "UserId" });
            DropIndex("dbo.AspNetUsers", "UserNameIndex");
            DropIndex("dbo.AspNetUserRoles", new[] { "RoleId" });
            DropIndex("dbo.AspNetUserRoles", new[] { "UserId" });
            DropIndex("dbo.AspNetRoles", "RoleNameIndex");
            DropIndex("dbo.Component", new[] { "ComponentTypeId" });
            DropIndex("dbo.ComponentType", new[] { "Image_ESImageId" });
            DropTable("dbo.CategoryComponentType");
            DropTable("dbo.AspNetUserLogins");
            DropTable("dbo.AspNetUserClaims");
            DropTable("dbo.AspNetUsers");
            DropTable("dbo.AspNetUserRoles");
            DropTable("dbo.AspNetRoles");
            DropTable("dbo.ESImage");
            DropTable("dbo.Component");
            DropTable("dbo.ComponentType");
            DropTable("dbo.Category");
        }
    }
}
