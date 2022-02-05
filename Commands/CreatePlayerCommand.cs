using MediatR;

namespace MyTeam.Commands
{
	public class CreatePlayerCommand : IRequest<int>
	{
		public string Email { get; set; }
		public string Nick { get; set; }
		public string Type { get; set; }
	}
}