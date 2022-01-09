using Microsoft.EntityFrameworkCore;

namespace MyTeam.Models {
	public class MyTeamContext : DbContext {

		public MyTeamContext(DbContextOptions options): base(options) {

		}

		public DbSet<Team2> Teams { get; set; }
		public DbSet<Player2> Players { get; set; }
		public DbSet<Position> Positions { get; set; }
		public DbSet<TeamPlayer> TeamsPlayers { get; set; }

		protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) {
			//optionsBuilder.UseNpgsql("Host=localhost;Database=myteam;Username=postgres;Password=888");
		}

		protected override void OnModelCreating(ModelBuilder modelBuilder) {
			modelBuilder.Entity<TeamPlayer>()
				.HasKey(tp => new{ tp.TeamId, tp.PlayerEmail });

			modelBuilder.Entity<TeamPlayer>()
				.HasOne(tp => tp.Team)
				.WithMany(t => t.TeamsPlayers)
				.HasForeignKey(tp => tp.TeamId);

			modelBuilder.Entity<TeamPlayer>()
				.HasOne(tp => tp.Player)
				.WithMany(p => p.TeamsPlayers)
				.HasForeignKey(tp => tp.PlayerEmail);

			modelBuilder.Entity<Player2>().HasKey(player => player.Email);

			modelBuilder.Entity<Player2>()
				.HasOne(player => player.Position)
				.WithOne(pos => pos.Player)
				.HasForeignKey<Position>(pos => pos.Email);

			modelBuilder.Entity<Position>().HasKey(pos => pos.Email);

			modelBuilder.Entity<Team2>().HasKey(team => team.Id);

			modelBuilder.Entity<FindTeamsByPlayerEmailDto>().HasNoKey();

			modelBuilder.Entity<FindTeamsByPlayerEmailDto>()
				.Property(x => x.Email).IsRequired();
		}
	}
}