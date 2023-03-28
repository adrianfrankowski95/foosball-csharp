
using Microsoft.EntityFrameworkCore;
using Foosball.CSharp.Domain.AggregateModel;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Foosball.CSharp.Domain;

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
            .HasValue<TwoPlayersTeam>("two-players");

        builder
            .Property<byte[]>("row_version")
            .HasColumnName("row_version")
            .IsRowVersion();
    }
}
