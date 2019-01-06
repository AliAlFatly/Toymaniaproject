namespace Toymania.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class tse : DbMigration
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
                        ToysName = c.String(),
                        CategoryId = c.Int(),
                        ProducerId = c.Int(),
                        ItemArtUrl = c.String(),
                        Price = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Counter = c.Int(),
                        MinimumAge = c.Int(),
                        SubCategoryId = c.Int(),
                        ItemArtUrl2 = c.String(),
                        ItemArtUrl3 = c.String(),
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
                        Toy_ToysId = c.Int(),
                    })
                .PrimaryKey(t => t.OrderDetailId)
                .ForeignKey("dbo.Orders", t => t.OrderId)
                .ForeignKey("dbo.Toys", t => t.Toy_ToysId)
                .Index(t => t.OrderId)
                .Index(t => t.Toy_ToysId);
            
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
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Carts", "ToyId", "dbo.Toys");
            DropForeignKey("dbo.Wishlists", "ToysId", "dbo.Toys");
            DropForeignKey("dbo.Toys", "ProducerId", "dbo.Producers");
            DropForeignKey("dbo.OrderDetails", "Toy_ToysId", "dbo.Toys");
            DropForeignKey("dbo.OrderDetails", "OrderId", "dbo.Orders");
            DropForeignKey("dbo.Toys", "CategoryId", "dbo.Categories");
            DropForeignKey("dbo.Toys", "SubCategoryId", "dbo.SubCategories");
            DropForeignKey("dbo.SubCategories", "CategoryId", "dbo.Categories");
            DropIndex("dbo.Wishlists", new[] { "ToysId" });
            DropIndex("dbo.OrderDetails", new[] { "Toy_ToysId" });
            DropIndex("dbo.OrderDetails", new[] { "OrderId" });
            DropIndex("dbo.SubCategories", new[] { "CategoryId" });
            DropIndex("dbo.Toys", new[] { "SubCategoryId" });
            DropIndex("dbo.Toys", new[] { "ProducerId" });
            DropIndex("dbo.Toys", new[] { "CategoryId" });
            DropIndex("dbo.Carts", new[] { "ToyId" });
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
