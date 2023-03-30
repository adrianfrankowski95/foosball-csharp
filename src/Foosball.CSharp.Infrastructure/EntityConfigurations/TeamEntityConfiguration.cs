
using Foosball.CSharp.Domain.TeamAggregateModel;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Foosball.CSharp.Infrastructure.EntityConfigurations;

public class TeamEntityConfiguration : IEntityTypeConfiguration<Team>
{
    public void Configure(EntityTypeBuilder<Team> builder)
    {
        builder.ToTable("teams");

        builder
            .HasKey(x => x.Id);

        builder
            .Property(x => x.Id)
            .HasConversion(x => x.Value, x => TeamId.FromExisting(x));

        builder
            .Property(x => x.Name)
            .HasConversion(x => x.Value, x => new NonEmptyString(x))
            .IsRequired();

        builder
            .HasIndex(x => x.Name)
            .IsUnique();

        builder
            .HasDiscriminator<string>("type")
            .HasValue<OnePlayerTeam>("one-player")
            .HasValue<TwoPlayerTeam>("two-players");
    }
}
