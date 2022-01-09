using Microsoft.AspNetCore.Mvc;
using MyTeam.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MyTeam.Controllers {
	[ApiController]
	[Route("[controller]")]
	public class PositionsController : ControllerBase {

		private readonly MyTeamContext _context;

		public PositionsController(MyTeamContext context) {
			this._context = context;
		}

		[HttpPut]
		public ActionResult Update([FromBody] UpdatePositionDto body)
		{
			if(String.IsNullOrEmpty(body.Email)) {
                return BadRequest("Электронная почта отсутствует или пустая");
            }

			var position = this._context.Positions.Find(body.Email);

			if (position == null) {
				return NotFound("Игрок с такой электронной почтой не зарегистрирован");
			}
			
			position.Lng = body.Lng;
			position.Lat = body.Lat;
			position.WhenUpdated = DateTime.Now;

			this._context.SaveChanges();

			return NoContent();
		}

		[HttpPut("RetTeammates")]
		public ActionResult UpdateAndRetTeammates(
			[FromBody] UpdatePositionAndRetTeammatesDto body
		)
		{
			if (String.IsNullOrEmpty(body.Email)) {
                return BadRequest("Электронная почта не указана и (или) пустая");
            }
            if (String.IsNullOrEmpty(body.Pin)) {
            	return BadRequest("Пин-код для команды не указан и (или) пустой");
            }

			var team = this._context.Teams.Find(body.TeamId);
			if (team == null) {
				return NotFound("Команда с таким Id не найдена");
			}
			if (team.Pin != body.Pin) {
				return Unauthorized("Неверный пин-код");
			}

			var position = this._context.Positions.Find(body.Email);
			if (position == null) {
				return NotFound("Игрок с такой электронной почтой не зарегистрирован");
			}

			position.Lng = body.Lng;
			position.Lat = body.Lat;
			position.WhenUpdated = DateTime.Now;

			this._context.SaveChanges();

			// Return result will be type of List<Position>
			var returnResult =
				this._context.TeamsPlayers
				.Where(tp => tp.TeamId == body.TeamId)
				.Where(tp => tp.PlayerEmail != body.Email)
				.Join(this._context.Positions, tp => tp.PlayerEmail, p => p.Email, (tp, p) => p)
				.ToList();

			return Ok(returnResult);
		}
	}
}