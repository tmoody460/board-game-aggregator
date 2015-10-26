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

        [Required]
        [StringLength(80)]
        public string Name { get; set; }

        [Required]
        public bool Played { get; set; }
        
        [Required]
        public bool Owned { get; set;  }

        [Required]
        [Range(0,10)]
        public double Rating { get; set; }

        public string Comments { get; set; }
        
        public BoardGameGeekInfo Info { get; set; }

    }
}