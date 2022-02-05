using MediatR;
using MyTeam.Commands;
using MyTeam.Models;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace MyTeam.CommandHandlers
{
	public class CreatePlayerCommandHandler
		: IRequestHandler<CreatePlayerCommand, int>
	{ 
		private readonly MyTeamContext _context;

		public CreatePlayerCommandHandler(MyTeamContext context)
		{
			this._context = context;
		}

		public async Task<int> Handle(
			CreatePlayerCommand command,
			CancellationToken cancellationToken
		)
		{
			DateTime currentTime = DateTime.Now;

      Position pos = new Position() {
        Email = command.Email,
        Lng = 0.0F,
        Lat = 0.0F,
        WhenUpdated = currentTime
      };

      Player2 player = new Player2() {
        Email = command.Email,
        Nick = command.Nick,
        Type = command.Type,
        WhenCreated = currentTime,
        Position = pos
      };

      await this._context.Players.AddAsync(player);
      await this._context.SaveChangesAsync();

			return 777;
		}
	}
}