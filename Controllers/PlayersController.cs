using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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
        /// Getting all players (Получение всех игроков)
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
        /// Creating new player (Создание нового игрока)
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

        /// <summary>
        /// Deleting player (Удаление игрока)
        /// </summary>
        [HttpDelete]
        public async Task<IActionResult> Delete([FromBody] DeletePlayerDto body) {
            var player = await this._context.Players.FindAsync(body.Email);
            if (player == null) {
                return NotFound("Игрок с такой электронной почтой не зарегистрирован");
            }

            this._context.Players.Remove(player);
            await this._context.SaveChangesAsync();

            return NoContent();
        }

        /// <summary>
        /// Updating player (Обновление игрока)
        /// </summary>
        [HttpPut]
        public async Task<IActionResult> Update([FromBody] UpdatePlayerDto body) {
            var player = await this._context.Players.FindAsync(body.Email);
            if (player == null) {
                return NotFound("Игрок с такой электронной почтой не зарегистрирован");
            }

            player.Nick = body.Nick;
            player.Type = body.Type;

            await this._context.SaveChangesAsync();

            return NoContent();
        }
    }
}        