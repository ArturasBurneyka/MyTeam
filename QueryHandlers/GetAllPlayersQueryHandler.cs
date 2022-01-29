using MediatR;
using Microsoft.EntityFrameworkCore;
using MyTeam.Models;
using MyTeam.Queries;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace MyTeam.QueryHandlers
{
	public class GetAllPlayersQueryHandler:
		IRequestHandler<GetAllPlayersQuery, IEnumerable<ReadPlayerDto>>
	{
		private readonly MyTeamContext _context;

		public GetAllPlayersQueryHandler(MyTeamContext context)
		{
			this._context = context;
		}

		public async Task<IEnumerable<ReadPlayerDto>>
		Handle(GetAllPlayersQuery query, CancellationToken cancellationToken)
		{
			List<ReadPlayerDto> output = new List<ReadPlayerDto>();

      List<Player2> players = await this._context.Players.ToListAsync();
      Dictionary<string, Position> positions = 
      	await this._context.Positions.ToDictionaryAsync(k => k.Email);

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

      if (output == null)
      	return null;

      return output;
		}
	}
}