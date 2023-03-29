
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Foosball.CSharp.Domain.TeamAggregateModel;

namespace Foosball.CSharp.Infrastructure.EntityConfigurations;

public class PlayerEntityConfiguration : IEntityTypeConfiguration<Player>
{
    public void Configure(EntityTypeBuilder<Player> builder)
    {
        builder.ToTable("players");

        builder
            .HasKey(x => x.Id);

        builder
            .Property(x => x.Id)
            .HasConversion(x => x.Value, x => PlayerId.FromExisting(x));

        builder
            .Property(x => x.FirstName)
            .HasConversion(x => x.Value, x => new NonEmptyString(x))
            .IsRequired();

        builder
            .Property(x => x.LastName)
            .HasConversion(x => x.Value, x => new NonEmptyString(x))
            .IsRequired();
    }
}
