namespace BoardGameAggregator.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ChangeBggInfoToMatchBggBetter : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.BoardGameGeekInfoes", "Description", c => c.String());
            AddColumn("dbo.BoardGameGeekInfoes", "MinPlayers", c => c.Int(nullable: false));
            AddColumn("dbo.BoardGameGeekInfoes", "MaxPlayers", c => c.Int(nullable: false));
            AddColumn("dbo.BoardGameGeekInfoes", "MinPlayingTime", c => c.Int(nullable: false));
            AddColumn("dbo.BoardGameGeekInfoes", "MaxPlayingTime", c => c.Int(nullable: false));
            AddColumn("dbo.BoardGameGeekInfoes", "ImageLink", c => c.String());
            DropColumn("dbo.BoardGameGeekInfoes", "NumPlayers");
            DropColumn("dbo.BoardGameGeekInfoes", "PlayingTime");
        }
        
        public override void Down()
        {
            AddColumn("dbo.BoardGameGeekInfoes", "PlayingTime", c => c.String());
            AddColumn("dbo.BoardGameGeekInfoes", "NumPlayers", c => c.Int(nullable: false));
            DropColumn("dbo.BoardGameGeekInfoes", "ImageLink");
            DropColumn("dbo.BoardGameGeekInfoes", "MaxPlayingTime");
            DropColumn("dbo.BoardGameGeekInfoes", "MinPlayingTime");
            DropColumn("dbo.BoardGameGeekInfoes", "MaxPlayers");
            DropColumn("dbo.BoardGameGeekInfoes", "MinPlayers");
            DropColumn("dbo.BoardGameGeekInfoes", "Description");
        }
    }
}
