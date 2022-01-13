using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using MyTeam.Models;

namespace MyTeam.Controllers {

    [ApiController]
    [Route("[controller]")]
    public class TeamsController : ControllerBase {
        private readonly Random _random;
        // DI
        private readonly MyTeamContext _context;

        public TeamsController(MyTeamContext context) {
            this._random = new Random();
            this._context = context;
        }

        [HttpGet]
        public ActionResult<IEnumerable<ReadTeamDto>> GetAll() {
            List<ReadTeamDto> output = new List<ReadTeamDto>();

            List<Team2> teams = this._context.Teams.ToList();

            foreach (Team2 team in teams) {
                ReadTeamDto o = new ReadTeamDto() {
                    Id = team.Id,
                    Name = team.Name,
                    OwnerEmail = team.OwnerEmail,
                    WhenCreated = team.WhenCreated
                };
                output.Add(o);
            }

            return Ok(output);
        }

        [HttpPost("My")]
        public ActionResult<IEnumerable<ReadTeamDto2>> GetMy(
            [FromBody] FindTeamsByPlayerEmailDto body
        ) {
            if(String.IsNullOrEmpty(body.Email)) {
                return BadRequest("Email отсутствует или пустой");
            }

            List<ReadTeamDto2> result = new List<ReadTeamDto2>();

            List<Team2> myTeams =
                this._context.TeamsPlayers
                .Where(tp => tp.PlayerEmail == body.Email)
                .Join(this._context.Teams, tp => tp.TeamId, t => t.Id, (tp, t) => t)
                .ToList();

            foreach(Team2 t in myTeams) {
                result.Add(new ReadTeamDto2() {
                    Id = t.Id, Name = t.Name
                });
            }

            return Ok(result);
        }

        [HttpPost]
        public ActionResult Create([FromBody] CreateTeamDto createTeamDto) {
            Team2 teamToCreate = new Team2() {
                Id = this._random.Next(1, 65535),
                Name = createTeamDto.Name,
                Pin = createTeamDto.Pin,
                OwnerEmail = createTeamDto.OwnerEmail,
                WhenCreated = DateTime.Now,
                TeamsPlayers = null
            };

            this._context.Teams.Add(teamToCreate);
            this._context.SaveChanges();

            ReadTeamDto readTeamDto = new ReadTeamDto() {
                Id = teamToCreate.Id,
                Name = teamToCreate.Name,
                OwnerEmail = teamToCreate.OwnerEmail,
                WhenCreated = teamToCreate.WhenCreated
            };
            return Created("", readTeamDto);
        }

        [HttpPost("WithOwner")]
        public ActionResult CreateWithOwner([FromBody] CreateTeamWithOwnerDto body) {
            if (String.IsNullOrEmpty(body.Name)) {
                return BadRequest("Ошибка в названии команды");
            }
            if (String.IsNullOrEmpty(body.Pin)) {
                return BadRequest("Ошибка в пин-коде");
            }
            if (body.Owner == null) {
                return BadRequest("Владелец команды не определен");
            }
            if (String.IsNullOrEmpty(body.Owner.Email)) {
                return BadRequest("Ошибка в электронной почте владельца");
            }
            if (String.IsNullOrEmpty(body.Owner.Nick)) {
                return BadRequest("Ошибка в никнейме владельца");
            }
            if (String.IsNullOrEmpty(body.Owner.Type)) {
                return BadRequest("Ошибка в типе владельца");
            }

            Team2 newTeam = new Team2() {
                Id = this._random.Next(1, 65535),
                Name = body.Name,
                Pin = body.Pin,
                OwnerEmail = body.Owner.Email,
                WhenCreated = DateTime.Now,
                TeamsPlayers = null
            };

            this._context.Teams.Add(newTeam);
            this._context.SaveChanges();

            if (this._context.Players.Find(body.Owner.Email) == null) {
                Player2 newPlayer = new Player2() {
                    Email = body.Owner.Email,
                    Nick = body.Owner.Nick,
                    Type = body.Owner.Type,
                    WhenCreated = DateTime.Now
                };

                Position newPosition = new Position() {
                    Email = body.Owner.Email,
                    Lng = 0.0F,
                    Lat = 0.0F,
                    WhenUpdated = DateTime.Now
                };

                this._context.Players.Add(newPlayer);
                this._context.Positions.Add(newPosition);
                this._context.SaveChanges();
            }     

            TeamPlayer newTeamPlayer = new TeamPlayer() {
                TeamId = newTeam.Id,
                PlayerEmail = body.Owner.Email
            };

            this._context.TeamsPlayers.Add(newTeamPlayer);
            this._context.SaveChanges();

            ReadTeamDto returnResult = new ReadTeamDto() {
                Id = newTeam.Id,
                Name = newTeam.Name,
                OwnerEmail = body.Owner.Email,
                WhenCreated = newTeam.WhenCreated
            };

            return Created("", returnResult);
        }

        [HttpPost("Join")]
        public ActionResult Join([FromBody] JoinTeamDto body) {
            Team2 t = this._context.Teams.Find(body.TeamId);
            if (t == null) {
                return BadRequest("Команды с таким Id не существует");
            }

            if (t.Pin != body.Pin) {
                return BadRequest("Неверный Pin код для команды");
            }

            Player2 p = this._context.Players.Find(body.PlayerEmail);
            if (p == null) {
                return BadRequest("Игрока с таким Email не существует");
            }

            TeamPlayer tp = this._context.TeamsPlayers.Find(body.TeamId, body.PlayerEmail);
            if (tp != null) {
                return BadRequest("Игрок уже в команде");
            }

            TeamPlayer teamPlayer = new TeamPlayer() {
                TeamId = body.TeamId,
                PlayerEmail = body.PlayerEmail
            };

            this._context.TeamsPlayers.Add(teamPlayer);
            this._context.SaveChanges();

            return Created("", new {
                TeamId = t.Id,
                TeamName = t.Name,
                PlayerEmail = p.Email 
            });
        }
    }
}