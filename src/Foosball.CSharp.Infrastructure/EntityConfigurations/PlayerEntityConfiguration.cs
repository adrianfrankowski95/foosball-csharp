
using Microsoft.EntityFrameworkCore;
using Foosball.CSharp.Domain.AggregateModel;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Foosball.CSharp.Domain;

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

        builder
            .Property<byte[]>("row_version")
            .HasColumnName("row_version")
            .IsRowVersion();
    }
}
