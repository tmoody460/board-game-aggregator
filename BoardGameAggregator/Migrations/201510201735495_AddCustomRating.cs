namespace BoardGameAggregator.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddCustomRating : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.BoardGames", "Rating", c => c.Double(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.BoardGames", "Rating");
        }
    }
}
