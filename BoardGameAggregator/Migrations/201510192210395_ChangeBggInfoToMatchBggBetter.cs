namespace BoardGameAggregator.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ChangeBggInfoToMatchBggBetter : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.BoardGameGeekInfo", "Description", c => c.String());
            AddColumn("dbo.BoardGameGeekInfo", "MinPlayers", c => c.Int(nullable: false));
            AddColumn("dbo.BoardGameGeekInfo", "MaxPlayers", c => c.Int(nullable: false));
            AddColumn("dbo.BoardGameGeekInfo", "MinPlayingTime", c => c.Int(nullable: false));
            AddColumn("dbo.BoardGameGeekInfo", "MaxPlayingTime", c => c.Int(nullable: false));
            AddColumn("dbo.BoardGameGeekInfo", "ImageLink", c => c.String());
            DropColumn("dbo.BoardGameGeekInfo", "NumPlayers");
            DropColumn("dbo.BoardGameGeekInfo", "PlayingTime");
        }
        
        public override void Down()
        {
            AddColumn("dbo.BoardGameGeekInfo", "PlayingTime", c => c.String());
            AddColumn("dbo.BoardGameGeekInfo", "NumPlayers", c => c.Int(nullable: false));
            DropColumn("dbo.BoardGameGeekInfo", "ImageLink");
            DropColumn("dbo.BoardGameGeekInfo", "MaxPlayingTime");
            DropColumn("dbo.BoardGameGeekInfo", "MinPlayingTime");
            DropColumn("dbo.BoardGameGeekInfo", "MaxPlayers");
            DropColumn("dbo.BoardGameGeekInfo", "MinPlayers");
            DropColumn("dbo.BoardGameGeekInfo", "Description");
        }
    }
}
