namespace BoardGameAggregator.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddBGGInfo : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.BoardGameGeekInfo",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        Name = c.String(),
                        NumPlayers = c.Int(nullable: false),
                        Rank = c.Int(nullable: false),
                        Rating = c.Double(nullable: false),
                        NumRatings = c.Long(nullable: false),
                        PlayingTime = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.BoardGames", t => t.Id)
                .Index(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.BoardGameGeekInfo", "Id", "dbo.BoardGames");
            DropIndex("dbo.BoardGameGeekInfo", new[] { "Id" });
            DropTable("dbo.BoardGameGeekInfo");
        }
    }
}
