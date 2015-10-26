namespace BoardGameAggregator.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class FixDatabasePluralizingError : DbMigration
    {
        public override void Up()
        {
            RenameTable(name: "dbo.BoardGames", newName: "BoardGame");
        }
        
        public override void Down()
        {
            RenameTable(name: "dbo.BoardGame", newName: "BoardGames");
        }
    }
}
