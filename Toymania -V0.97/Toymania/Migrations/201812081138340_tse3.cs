namespace Toymania.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class tse3 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.OrderDetails", "tracker", c => c.String(defaultValue:"In the Storage"));
        }
        
        public override void Down()
        {
            DropColumn("dbo.OrderDetails", "tracker");
        }
    }
}
