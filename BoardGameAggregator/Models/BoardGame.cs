using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace BoardGameAggregator.Models
{
    public class BoardGame
    {
        [Key]
        public Guid Id { get; set; }

        public string Name { get; set; }

        public bool Played { get; set; }
        
        public bool Owned { get; set;  }

        public double Rating { get; set; }

        public string Comments { get; set; }

        public BoardGameGeekInfo Info { get; set; }

    }
}