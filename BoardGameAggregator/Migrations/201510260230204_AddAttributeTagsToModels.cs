namespace BoardGameAggregator.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddAttributeTagsToModels : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.BoardGameGeekInfoes", "Name", c => c.String(nullable: false));
            AlterColumn("dbo.BoardGames", "Name", c => c.String(nullable: false, maxLength: 80));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.BoardGames", "Name", c => c.String());
            AlterColumn("dbo.BoardGameGeekInfoes", "Name", c => c.String());
        }
    }
}
