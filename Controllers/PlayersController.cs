using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using MyTeam.Models;

namespace MyTeam.Controllers {

    [ApiController]
    [Route("[controller]")]
    public class PlayersController : ControllerBase {

        // DI
        private readonly MyTeamContext _context;

        public PlayersController(MyTeamContext context) {
            this._context = context;
        }

        /// <summary>
        /// Getting all players
        /// </summary>
        [HttpGet]
        public IActionResult GetAll() {
            List<ReadPlayerDto> output = new List<ReadPlayerDto>();

            List<Player2> players = this._context.Players.ToList();
            Dictionary<string, Position> positions =
                this._context.Positions.ToDictionary(k => k.Email);

            foreach (Player2 player in players) {
                ReadPlayerDto o = new ReadPlayerDto() {
                    Email = player.Email,
                    Nick = player.Nick,
                    Type = player.Type,
                    WhenCreated = player.WhenCreated,
                    Position = new ReadPositionDto {
                        Lng = positions[player.Email].Lng,
                        Lat = positions[player.Email].Lat,
                        WhenUpdated = positions[player.Email].WhenUpdated
                    }
                };
                
                output.Add(o);
            }

            return Ok(output);
        }

        /// <summary>
        /// Creating new player
        /// </summary>
        [HttpPost]
        public IActionResult Create([FromBody] CreatePlayerDto createPlayerDto) {

            Player2 player = new Player2() {
                Email = createPlayerDto.Email,
                Nick = createPlayerDto.Nick,
                Type = createPlayerDto.Type,
                WhenCreated = DateTime.Now
            };

            Position pos = new Position() {
                Email = createPlayerDto.Email,
                Lng = 0.0F,
                Lat = 0.0F,
                WhenUpdated = DateTime.Now
            };

            this._context.Players.Add(player);
            this._context.Positions.Add(pos);
            this._context.SaveChanges();

            return Created("", null);
        }

    }
}        