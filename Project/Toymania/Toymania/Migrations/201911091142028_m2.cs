namespace Toymania.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class m2 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Categories", "CategoryName", c => c.String());
            AddColumn("dbo.SubCategories", "SubCategoryName", c => c.String());
            AddColumn("dbo.OrderDetails", "CategoryName", c => c.String());
            AddColumn("dbo.OrderDetails", "SubCategoryName", c => c.String());
            DropColumn("dbo.Categories", "CName");
            DropColumn("dbo.SubCategories", "SCName");
            DropColumn("dbo.OrderDetails", "CName");
            DropColumn("dbo.OrderDetails", "SCName");
        }
        
        public override void Down()
        {
            AddColumn("dbo.OrderDetails", "SCName", c => c.String());
            AddColumn("dbo.OrderDetails", "CName", c => c.String());
            AddColumn("dbo.SubCategories", "SCName", c => c.String());
            AddColumn("dbo.Categories", "CName", c => c.String());
            DropColumn("dbo.OrderDetails", "SubCategoryName");
            DropColumn("dbo.OrderDetails", "CategoryName");
            DropColumn("dbo.SubCategories", "SubCategoryName");
            DropColumn("dbo.Categories", "CategoryName");
        }
    }
}
