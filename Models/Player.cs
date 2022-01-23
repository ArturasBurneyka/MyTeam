using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MyTeam.Models {

	[NotMapped]
    public class Player {
    
        public Player() {
            this.Lng = 0.0M;
            this.Lat = 0.0M;
        }
    
        public string Email { get; set; }
        public string Nick { get; set; }
        public string Type { get; set; }
        public decimal Lng { get; set; }
        public decimal Lat { get; set; }
    }

	[Table("players")]
    public class Player2 {
        public string Email { get; set; }
        public string Nick { get; set; }
        public string Type { get; set; }
        public DateTime WhenCreated { get; set; }
        // Navigation property for one-to-one relationship with Position
        public Position Position { get; set; }
        // Navigation property for many-to-mane relationship with Team2
        public ICollection<TeamPlayer> TeamsPlayers { get; set; }
    }

    [NotMapped]
    public class CreatePlayerDto {
    	public string Email { get; set; }
        public string Nick { get; set; }
        public string Type { get; set; }
    }

    [NotMapped]
    public class ReadPlayerDto {
        public string Email { get; set; }
        public string Nick { get; set; }
        public string Type { get; set; }
        public DateTime WhenCreated { get; set; }
        public ReadPositionDto Position { get; set; }
    }

    [NotMapped]
    public class UpdatePlayerDto {
        public string Email { get; set; }
        public string Nick { get; set; }
        public string Type { get; set; }
    }

    [NotMapped]
    public class DeletePlayerDto {
        public string Email { get; set; }
    }
}