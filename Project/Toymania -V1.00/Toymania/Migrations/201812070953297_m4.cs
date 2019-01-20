namespace Toymania.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class m4 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AspNetUsers", "balance", c => c.Decimal(nullable: false, precision: 18, scale: 2, defaultValue: 0.00m));
            AddColumn("dbo.AspNetUsers", "Role", c => c.String(nullable: false, defaultValue: "Customer"));
        }
        
        public override void Down()
        {
            DropColumn("dbo.AspNetUsers", "Role");
            DropColumn("dbo.AspNetUsers", "balance");
        }
    }
}
