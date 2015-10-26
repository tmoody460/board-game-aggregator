using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Linq;
using System.Web;

namespace BoardGameAggregator.Models
{
    public class SystemContext : DbContext
    {
        // You can add custom code to this file. Changes will not be overwritten.
        // 
        // If you want Entity Framework to drop and regenerate your database
        // automatically whenever you change your model schema, please use data migrations.
        // For more information refer to the documentation:
        // http://msdn.microsoft.com/en-us/data/jj591621.aspx
    
        public SystemContext() : base("name=SystemContext")
        {
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
            base.OnModelCreating(modelBuilder);
        }

        public System.Data.Entity.DbSet<BoardGameAggregator.Models.BoardGame> BoardGames { get; set; }

        public System.Data.Entity.DbSet<BoardGameAggregator.Models.BoardGameGeekInfo> BoardGameGeekInfo { get; set; }
    }
}
