using System.Collections.Generic;

namespace MyTeam.Models {
    public class MockRepository {

        private static Dictionary<ushort, Team> _teams = new Dictionary<ushort, Team>() {
            [11] = new Team {
                Id = 11, Name = "DevTeam", Pin = "7777", OwnerEmail = "DevTeam@gmail.com",
                Players = new List<string>() { "EvgenGaneev@gmail.com", "ArtemUzlov@gmail.com" }
            },
            [22] = new Team { Id = 22 },
            [33] = new Team { Id = 33 }
        };
                
        public static Dictionary<ushort, Team> Teams {
            get { return _teams; }
        }
        
        private static Dictionary<string, Player> _players = new Dictionary<string, Player>() {
            ["EvgenGaneev@gmail.com"] = new Player { Nick = "Evgen", Email = "EvgenGaneev@gmail.com" },
            ["ArtemUzlov@gmail.com"] = new Player { Nick = "Artem", Email = "ArtemUzlov@gmail.com" }
        };
        
        public static Dictionary<string, Player> Players {
            get { return _players; }
        }
    }
}
