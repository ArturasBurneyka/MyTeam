using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MyTeam.Models {

	[Table("positions")]
	public class Position {
		public string Email { get; set; }
		public float Lng { get; set; }
		public float Lat { get; set; }
		public DateTime WhenUpdated { get; set; }
		// Navigation properties for one-to-one relationship with Player2
		public Player2 Player { get; set; }
	}

	[NotMapped]
	public class ReadPositionDto {
		public float Lng { get; set; }
		public float Lat { get; set; }
		public DateTime WhenUpdated { get; set; }
	}

	[NotMapped]
	public class UpdatePositionDto {
		public string Email { get; set; }
		public float Lng { get; set; }
		public float Lat { get; set; }
	}

	[NotMapped]
	public class UpdatePositionAndRetTeammatesDto {
		public string Email { get; set; }
		public float Lng { get; set; }
		public float Lat { get; set; }
		public int TeamId { get; set; }
		public string Pin { get; set; }
	}

}