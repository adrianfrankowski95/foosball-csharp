
using Microsoft.EntityFrameworkCore;
using Foosball.CSharp.Domain.GameAggregateModel;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Foosball.CSharp.Domain.TeamAggregateModel;

namespace Foosball.CSharp.Infrastructure.EntityConfigurations;

public class OnePlayerTeamEntityConfiguration : IEntityTypeConfiguration<OnePlayerTeam>
{
    public void Configure(EntityTypeBuilder<OnePlayerTeam> builder)
    {
        builder
            .HasBaseType<Team>();

        builder
            .Property(x => x.PlayerId)
            .HasColumnName("first_player_id")
            .HasConversion(x => x.Value, x => PlayerId.FromExisting(x))
            .IsRequired();

        builder
            .HasOne<Player>()
            .WithMany()
            .HasForeignKey(x => x.PlayerId)
            .OnDelete(DeleteBehavior.Restrict)
            .IsRequired();
    }
}
