using MediatR;
using MyTeam.Models;
using System.Collections.Generic;

namespace MyTeam.Queries
{
	public class GetAllPlayersQuery : IRequest<IEnumerable<ReadPlayerDto>>
	{

	}
}