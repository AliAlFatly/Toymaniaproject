namespace Toymania.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class va4 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Toys", "MinimumAge", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Toys", "MinimumAge", c => c.Int());
        }
    }
}
