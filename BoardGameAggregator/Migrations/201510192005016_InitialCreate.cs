namespace BoardGameAggregator.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialCreate : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.BoardGames",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        Name = c.String(),
                        Played = c.Boolean(nullable: false),
                        Owned = c.Boolean(nullable: false),
                        Comments = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.BoardGames");
        }
    }
}
