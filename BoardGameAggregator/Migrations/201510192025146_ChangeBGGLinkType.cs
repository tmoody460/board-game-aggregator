namespace BoardGameAggregator.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ChangeBGGLinkType : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.BoardGameGeekInfo", "Link", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.BoardGameGeekInfo", "Link");
        }
    }
}
