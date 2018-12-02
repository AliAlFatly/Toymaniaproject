namespace Toymania.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class m11 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AspNetUsers", "Balance", c => c.Decimal (nullable: false));
        }
        
        public override void Down()
        {
        }
    }
}
