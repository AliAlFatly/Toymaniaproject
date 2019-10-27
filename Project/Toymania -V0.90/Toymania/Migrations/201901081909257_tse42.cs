namespace Toymania.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class tse42 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Orders", "FirstName", c => c.String(maxLength: 160));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Orders", "FirstName", c => c.String());
        }
    }
}
