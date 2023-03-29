
using Microsoft.EntityFrameworkCore;
using Foosball.CSharp.Domain.AggregateModel;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Foosball.CSharp.Infrastructure.EntityConfigurations;

public class TwoPlayersTeamEntityConfiguration : IEntityTypeConfiguration<TwoPlayersTeam>
{
    public void Configure(EntityTypeBuilder<TwoPlayersTeam> builder)
    {
        builder
            .HasBaseType<Team>();

        builder
            .Property(x => x.FirstPlayerId)
            .HasColumnName("first_player_id")
            .HasConversion(x => x.Value, x => PlayerId.FromExisting(x))
            .IsRequired();

        builder
            .HasOne<Player>()
            .WithMany()
            .HasForeignKey(x => x.FirstPlayerId)
            .IsRequired();

        builder
            .Property(x => x.SecondPlayerId)
            .HasColumnName("second_player_id")
            .HasConversion(x => x.Value, x => PlayerId.FromExisting(x))
            .IsRequired();

        builder
            .HasOne<Player>()
            .WithMany()
            .HasForeignKey(x => x.SecondPlayerId)
            .IsRequired();
    }
}
