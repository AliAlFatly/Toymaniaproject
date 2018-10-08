namespace Toymania.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class m1 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Toys",
                c => new
                    {
                        ToysId = c.Int(nullable: false, identity: true),
                        ToysName = c.String(),
                        CategoryId = c.Int(),
                        ProducerId = c.Int(),
                        ItemArtUrl = c.String(),
                        Price = c.Decimal(precision: 18, scale: 2),
                        Counter = c.Int(),
                        MinimumAge = c.Int(),
                        type = c.String(),
                    })
                .PrimaryKey(t => t.ToysId)
                .ForeignKey("dbo.Categories", t => t.CategoryId)
                .ForeignKey("dbo.Producers", t => t.ProducerId)
                .Index(t => t.CategoryId)
                .Index(t => t.ProducerId);
            
            CreateTable(
                "dbo.Carts",
                c => new
                    {
                        RecordId = c.Int(nullable: false, identity: true),
                        CartId = c.String(),
                        ToyId = c.Int(),
                        Count = c.Int(),
                        DateCreated = c.DateTime(),
                        Toy_ToysId = c.Int(),
                    })
                .PrimaryKey(t => t.RecordId)
                .ForeignKey("dbo.Toys", t => t.Toy_ToysId)
                .Index(t => t.Toy_ToysId);
            
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
                "dbo.OrderDetails",
                c => new
                    {
                        OrderDetailId = c.Int(nullable: false, identity: true),
                        OrderId = c.Int(),
                        ToyId = c.Int(),
                        Quantity = c.Int(),
                        UnitPrice = c.Decimal(precision: 18, scale: 2),
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
                        FirstName = c.String(nullable: false, maxLength: 160),
                        LastName = c.String(nullable: false, maxLength: 160),
                        Address = c.String(nullable: false, maxLength: 70),
                        City = c.String(nullable: false, maxLength: 40),
                        State = c.String(nullable: false, maxLength: 40),
                        PostalCode = c.String(nullable: false, maxLength: 10),
                        Country = c.String(nullable: false, maxLength: 24),
                        Email = c.String(),
                        Total = c.Decimal(precision: 18, scale: 2),
                        OrderDate = c.DateTime(),
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
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Toys", "ProducerId", "dbo.Producers");
            DropForeignKey("dbo.OrderDetails", "Toy_ToysId", "dbo.Toys");
            DropForeignKey("dbo.OrderDetails", "OrderId", "dbo.Orders");
            DropForeignKey("dbo.Toys", "CategoryId", "dbo.Categories");
            DropForeignKey("dbo.Carts", "Toy_ToysId", "dbo.Toys");
            DropIndex("dbo.OrderDetails", new[] { "Toy_ToysId" });
            DropIndex("dbo.OrderDetails", new[] { "OrderId" });
            DropIndex("dbo.Carts", new[] { "Toy_ToysId" });
            DropIndex("dbo.Toys", new[] { "ProducerId" });
            DropIndex("dbo.Toys", new[] { "CategoryId" });
            DropTable("dbo.Producers");
            DropTable("dbo.Orders");
            DropTable("dbo.OrderDetails");
            DropTable("dbo.Categories");
            DropTable("dbo.Carts");
            DropTable("dbo.Toys");
        }
    }
}
