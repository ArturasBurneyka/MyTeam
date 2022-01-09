using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MyTeam.Models {

    [NotMapped]
    public class Team {
    
        public Team() {
            this.Id = 0;
            this.Players = new List<string>();
        }
    
        public ushort Id { get; set; }
        public string Name { get; set; }
        public string Pin { get; set; }
        public string OwnerEmail { get; set; }
        public List<string> Players { get; set; }
    }
    
    [NotMapped]
    public class RepresentationalTeam {
    
        public RepresentationalTeam() {
            this.Id = 0;
            this.Players = new List<Player>();
        }
        
        public RepresentationalTeam(Team _team) {
            this.Id = _team.Id;
            this.Name = _team.Name;
            this.Pin = _team.Pin;
            this.OwnerEmail = _team.OwnerEmail;
            this.Players = new List<Player>();
            foreach (string s in _team.Players) {
                if (!MockRepository.Players.ContainsKey(s)) {
                    this.Players.Add(new Player());
                } else {
                    this.Players.Add(MockRepository.Players[s]);
                }
            }
        }
    
        public ushort Id { get; set; }
        public string Name { get; set; }
        public string Pin { get; set; }
        public string OwnerEmail { get; set; }
        public List<Player> Players { get; set; }
    }
    
    [NotMapped]
    public class ShortTeam {
    
        public ShortTeam() {
            this.Id = 0;
        }
        
        public ushort Id { get; set; }
        public string Name { get; set; }
    }

    // New entities (since November 09, 2021)
    [Table("teams")]
    public class Team2 {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Pin { get; set; }
        public string OwnerEmail { get; set; }
        public DateTime WhenCreated { get; set; }
        public ICollection<TeamPlayer> TeamsPlayers { get; set; }
    }

    [NotMapped]
    public class CreateTeamDto {
        public string Name { get; set; }
        public string Pin { get; set; }
        public string OwnerEmail { get; set; }
    }

    [NotMapped]
    public class CreateTeamWithOwnerDto {
        public string Name { get; set; }
        public string Pin { get; set; }
        public CreatePlayerDto Owner { get; set; }
    }

    [NotMapped]
    public class ReadTeamDto {
        public int Id { get; set; }
        public string Name { get; set; }
        public string OwnerEmail { get; set; }
        public DateTime WhenCreated { get; set; }
    }

    [NotMapped]
    public class ReadTeamDto2 {
        public int Id { get; set; }
        public string Name { get; set; }
    }

    [NotMapped]
    public class JoinTeamDto {
        public int TeamId { get; set; }
        public string Pin { get; set; }
        public string PlayerEmail { get; set; }
    }

    [NotMapped]
    public class FindTeamsByPlayerEmailDto {
        public string Email { get; set; }
    }

    [NotMapped]
    public class Team2VM {
        public Team2VM() {
            this.Players = new List<Player2>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public string Pin { get; set; }
        public string OwnerEmail { get; set; }
        public ICollection<Player2> Players { get; set; }
    }

    [Table("teams_players")]
    public class TeamPlayer {
        public int TeamId { get; set; }
        public Team2 Team { get; set; }
        public string PlayerEmail { get; set; }
        public Player2 Player { get; set; }
    }
}