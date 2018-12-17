namespace Toymania.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class archive : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Toys", "Archive", c => c.String());
            DropColumn("dbo.Toys", "ItemArtUrl3");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Toys", "ItemArtUrl3", c => c.String());
            DropColumn("dbo.Toys", "Archive");
        }
    }
}
