namespace Toymania.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class val2 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Orders", "FirstName", c => c.String());
            AlterColumn("dbo.Orders", "LastName", c => c.String());
            AlterColumn("dbo.Orders", "Address", c => c.String());
            AlterColumn("dbo.Orders", "City", c => c.String());
            AlterColumn("dbo.Orders", "State", c => c.String());
            AlterColumn("dbo.Orders", "PostalCode", c => c.String());
            AlterColumn("dbo.Orders", "Country", c => c.String());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Orders", "Country", c => c.String(nullable: false));
            AlterColumn("dbo.Orders", "PostalCode", c => c.String(nullable: false));
            AlterColumn("dbo.Orders", "State", c => c.String(nullable: false));
            AlterColumn("dbo.Orders", "City", c => c.String(nullable: false));
            AlterColumn("dbo.Orders", "Address", c => c.String(nullable: false));
            AlterColumn("dbo.Orders", "LastName", c => c.String(nullable: false));
            AlterColumn("dbo.Orders", "FirstName", c => c.String(nullable: false));
        }
    }
}
