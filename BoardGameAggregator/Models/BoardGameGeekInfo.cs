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

        public string Description { get; set; }

        public int MinPlayers { get; set; }

        public int MaxPlayers { get; set; }

        public int Rank { get; set; }

        public double Rating { get; set; }

        public long NumRatings { get; set; }

        public int MinPlayingTime { get; set; }

        public int MaxPlayingTime { get; set; }

        public string Link { get; set; }

        public string ImageLink { get; set; }

        [Required]
        public BoardGame BoardGame { get; set; }

    }
}