namespace BoardGameAggregator.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddBggId : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.BoardGameGeekInfo", "BggId", c => c.Long(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.BoardGameGeekInfo", "BggId");
        }
    }
}
