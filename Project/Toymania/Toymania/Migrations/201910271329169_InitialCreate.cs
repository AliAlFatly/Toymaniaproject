namespace Toymania.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialCreate : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Carts",
                c => new
                    {
                        RecordId = c.Int(nullable: false, identity: true),
                        CartId = c.String(),
                        ToyId = c.Int(nullable: false),
                        Count = c.Int(nullable: false),
                        DateCreated = c.DateTime(),
                    })
                .PrimaryKey(t => t.RecordId)
                .ForeignKey("dbo.Toys", t => t.ToyId, cascadeDelete: true)
                .Index(t => t.ToyId);
            
            CreateTable(
                "dbo.Toys",
                c => new
                    {
                        ToysId = c.Int(nullable: false, identity: true),
                        ToysName = c.String(nullable: false),
                        CategoryId = c.Int(),
                        ProducerId = c.Int(),
                        ItemArtUrl = c.String(),
                        Price = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Counter = c.Int(),
                        MinimumAge = c.Int(nullable: false),
                        SubCategoryId = c.Int(),
                        ItemArtUrl2 = c.String(),
                        Archive = c.String(),
                    })
                .PrimaryKey(t => t.ToysId)
                .ForeignKey("dbo.SubCategories", t => t.SubCategoryId)
                .ForeignKey("dbo.Categories", t => t.CategoryId)
                .ForeignKey("dbo.Producers", t => t.ProducerId)
                .Index(t => t.CategoryId)
                .Index(t => t.ProducerId)
                .Index(t => t.SubCategoryId);
            
            CreateTable(
                "dbo.Categories",
                c => new
                    {
                        CategoryId = c.Int(nullable: false, identity: true),
                        CName = c.String(),
                        Description = c.String(),
                    })
                .PrimaryKey(t => t.CategoryId);
            
            CreateTable(
                "dbo.SubCategories",
                c => new
                    {
                        SubCategoryId = c.Int(nullable: false, identity: true),
                        SCName = c.String(),
                        Description = c.String(),
                        CategoryId = c.Int(),
                    })
                .PrimaryKey(t => t.SubCategoryId)
                .ForeignKey("dbo.Categories", t => t.CategoryId)
                .Index(t => t.CategoryId);
            
            CreateTable(
                "dbo.OrderDetails",
                c => new
                    {
                        OrderDetailId = c.Int(nullable: false, identity: true),
                        OrderId = c.Int(),
                        ToyId = c.Int(),
                        Quantity = c.Int(),
                        UnitPrice = c.Decimal(precision: 18, scale: 2),
                        Status = c.String(),
                        CName = c.String(),
                        SCName = c.String(),
                        Week = c.Int(),
                        Month = c.Int(),
                        year = c.Int(),
                        Day = c.Int(),
                        Hour = c.Int(),
                        Minute = c.Int(),
                        tracker = c.String(),
                    })
                .PrimaryKey(t => t.OrderDetailId)
                .ForeignKey("dbo.Orders", t => t.OrderId)
                .ForeignKey("dbo.Toys", t => t.ToyId)
                .Index(t => t.OrderId)
                .Index(t => t.ToyId);
            
            CreateTable(
                "dbo.Orders",
                c => new
                    {
                        OrderId = c.Int(nullable: false, identity: true),
                        Username = c.String(),
                        FirstName = c.String(),
                        LastName = c.String(),
                        Address = c.String(),
                        City = c.String(),
                        State = c.String(),
                        PostalCode = c.String(),
                        Country = c.String(),
                        Total = c.Decimal(precision: 18, scale: 2),
                        OrderDate = c.DateTime(),
                        year = c.Int(),
                        month = c.Int(),
                        day = c.Int(),
                        minute = c.Int(),
                        second = c.Int(),
                        hour = c.Int(),
                        week = c.Int(),
                    })
                .PrimaryKey(t => t.OrderId);
            
            CreateTable(
                "dbo.Producers",
                c => new
                    {
                        ProducerId = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                    })
                .PrimaryKey(t => t.ProducerId);
            
            CreateTable(
                "dbo.Wishlists",
                c => new
                    {
                        WishlistId = c.Int(nullable: false, identity: true),
                        Email = c.String(),
                        ToysId = c.Int(),
                    })
                .PrimaryKey(t => t.WishlistId)
                .ForeignKey("dbo.Toys", t => t.ToysId)
                .Index(t => t.ToysId);
            
            CreateTable(
                "dbo.Coupons",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Code = c.String(),
                        Value = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Used = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
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
                        balance = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Role = c.String(),
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
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.AspNetUserRoles", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserLogins", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserClaims", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserRoles", "RoleId", "dbo.AspNetRoles");
            DropForeignKey("dbo.Carts", "ToyId", "dbo.Toys");
            DropForeignKey("dbo.Wishlists", "ToysId", "dbo.Toys");
            DropForeignKey("dbo.Toys", "ProducerId", "dbo.Producers");
            DropForeignKey("dbo.OrderDetails", "ToyId", "dbo.Toys");
            DropForeignKey("dbo.OrderDetails", "OrderId", "dbo.Orders");
            DropForeignKey("dbo.Toys", "CategoryId", "dbo.Categories");
            DropForeignKey("dbo.Toys", "SubCategoryId", "dbo.SubCategories");
            DropForeignKey("dbo.SubCategories", "CategoryId", "dbo.Categories");
            DropIndex("dbo.AspNetUserLogins", new[] { "UserId" });
            DropIndex("dbo.AspNetUserClaims", new[] { "UserId" });
            DropIndex("dbo.AspNetUsers", "UserNameIndex");
            DropIndex("dbo.AspNetUserRoles", new[] { "RoleId" });
            DropIndex("dbo.AspNetUserRoles", new[] { "UserId" });
            DropIndex("dbo.AspNetRoles", "RoleNameIndex");
            DropIndex("dbo.Wishlists", new[] { "ToysId" });
            DropIndex("dbo.OrderDetails", new[] { "ToyId" });
            DropIndex("dbo.OrderDetails", new[] { "OrderId" });
            DropIndex("dbo.SubCategories", new[] { "CategoryId" });
            DropIndex("dbo.Toys", new[] { "SubCategoryId" });
            DropIndex("dbo.Toys", new[] { "ProducerId" });
            DropIndex("dbo.Toys", new[] { "CategoryId" });
            DropIndex("dbo.Carts", new[] { "ToyId" });
            DropTable("dbo.AspNetUserLogins");
            DropTable("dbo.AspNetUserClaims");
            DropTable("dbo.AspNetUsers");
            DropTable("dbo.AspNetUserRoles");
            DropTable("dbo.AspNetRoles");
            DropTable("dbo.Coupons");
            DropTable("dbo.Wishlists");
            DropTable("dbo.Producers");
            DropTable("dbo.Orders");
            DropTable("dbo.OrderDetails");
            DropTable("dbo.SubCategories");
            DropTable("dbo.Categories");
            DropTable("dbo.Toys");
            DropTable("dbo.Carts");
        }
    }
}
