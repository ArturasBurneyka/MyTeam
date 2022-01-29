using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MediatR;
using MyTeam.Models;
using MyTeam.Queries;

namespace MyTeam.Controllers {

    [ApiController]
    [Route("[controller]")]
    public class PlayersController : ControllerBase {

        // DI
        private readonly MyTeamContext _context;
        private readonly IMediator _mediator;

        public PlayersController(MyTeamContext context, IMediator mediator) {
            this._context = context;
            this._mediator = mediator;
        }

        /// <summary>
        /// Getting all players (Получение всех игроков)
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> GetAll() {
            return Ok(await this._mediator.Send(new GetAllPlayersQuery()));
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