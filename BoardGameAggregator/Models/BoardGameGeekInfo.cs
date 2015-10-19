using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace BoardGameAggregator.Models
{
    public class BoardGameGeekInfo
    {

        [Key]
        public Guid Id { get; set; }

        public string Name { get; set; }

        public int NumPlayers { get; set; }

        public int Rank { get; set; }

        public double Rating { get; set; }

        public long NumRatings { get; set; }

        public string PlayingTime { get; set; }

        public string Link { get; set; }

        [Required]
        public BoardGame BoardGame { get; set; }

    }
}