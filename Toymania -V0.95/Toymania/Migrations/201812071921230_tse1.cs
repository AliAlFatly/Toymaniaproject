namespace Toymania.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class tse1 : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.OrderDetails", "ToyId");
            RenameColumn(table: "dbo.OrderDetails", name: "Toy_ToysId", newName: "ToyId");
            RenameIndex(table: "dbo.OrderDetails", name: "IX_Toy_ToysId", newName: "IX_ToyId");
        }
        
        public override void Down()
        {
            RenameIndex(table: "dbo.OrderDetails", name: "IX_ToyId", newName: "IX_Toy_ToysId");
            RenameColumn(table: "dbo.OrderDetails", name: "ToyId", newName: "Toy_ToysId");
            AddColumn("dbo.OrderDetails", "ToyId", c => c.Int());
        }
    }
}
