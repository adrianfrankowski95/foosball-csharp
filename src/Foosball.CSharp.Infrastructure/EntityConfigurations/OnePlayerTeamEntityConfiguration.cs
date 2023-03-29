
using Microsoft.EntityFrameworkCore;
using Foosball.CSharp.Domain.AggregateModel;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

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
