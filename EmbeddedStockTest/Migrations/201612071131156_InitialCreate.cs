namespace EmbeddedStockTest.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialCreate : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Categories",
                c => new
                    {
                        CategoryId = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                    })
                .PrimaryKey(t => t.CategoryId);
            
            CreateTable(
                "dbo.ComponentTypes",
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
                .ForeignKey("dbo.ESImages", t => t.Image_ESImageId)
                .Index(t => t.Image_ESImageId);
            
            CreateTable(
                "dbo.Components",
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
                .ForeignKey("dbo.ComponentTypes", t => t.ComponentTypeId, cascadeDelete: true)
                .Index(t => t.ComponentTypeId);
            
            CreateTable(
                "dbo.ESImages",
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
                .ForeignKey("dbo.AspNetRoles", t => t.RoleId, cascadeDelete: true)
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
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
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
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
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.ComponentTypeCategories",
                c => new
                    {
                        ComponentType_ComponentTypeId = c.Long(nullable: false),
                        Category_CategoryId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.ComponentType_ComponentTypeId, t.Category_CategoryId })
                .ForeignKey("dbo.ComponentTypes", t => t.ComponentType_ComponentTypeId, cascadeDelete: true)
                .ForeignKey("dbo.Categories", t => t.Category_CategoryId, cascadeDelete: true)
                .Index(t => t.ComponentType_ComponentTypeId)
                .Index(t => t.Category_CategoryId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.AspNetUserRoles", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserLogins", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserClaims", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserRoles", "RoleId", "dbo.AspNetRoles");
            DropForeignKey("dbo.ComponentTypes", "Image_ESImageId", "dbo.ESImages");
            DropForeignKey("dbo.Components", "ComponentTypeId", "dbo.ComponentTypes");
            DropForeignKey("dbo.ComponentTypeCategories", "Category_CategoryId", "dbo.Categories");
            DropForeignKey("dbo.ComponentTypeCategories", "ComponentType_ComponentTypeId", "dbo.ComponentTypes");
            DropIndex("dbo.ComponentTypeCategories", new[] { "Category_CategoryId" });
            DropIndex("dbo.ComponentTypeCategories", new[] { "ComponentType_ComponentTypeId" });
            DropIndex("dbo.AspNetUserLogins", new[] { "UserId" });
            DropIndex("dbo.AspNetUserClaims", new[] { "UserId" });
            DropIndex("dbo.AspNetUsers", "UserNameIndex");
            DropIndex("dbo.AspNetUserRoles", new[] { "RoleId" });
            DropIndex("dbo.AspNetUserRoles", new[] { "UserId" });
            DropIndex("dbo.AspNetRoles", "RoleNameIndex");
            DropIndex("dbo.Components", new[] { "ComponentTypeId" });
            DropIndex("dbo.ComponentTypes", new[] { "Image_ESImageId" });
            DropTable("dbo.ComponentTypeCategories");
            DropTable("dbo.AspNetUserLogins");
            DropTable("dbo.AspNetUserClaims");
            DropTable("dbo.AspNetUsers");
            DropTable("dbo.AspNetUserRoles");
            DropTable("dbo.AspNetRoles");
            DropTable("dbo.ESImages");
            DropTable("dbo.Components");
            DropTable("dbo.ComponentTypes");
            DropTable("dbo.Categories");
        }
    }
}
