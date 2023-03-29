
using Microsoft.EntityFrameworkCore;
using Foosball.CSharp.Domain.AggregateModel;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Foosball.CSharp.Infrastructure.EntityConfigurations;

public class GameEntityConfiguration : IEntityTypeConfiguration<Game>
{
    public void Configure(EntityTypeBuilder<Game> builder)
    {
        builder.ToTable("games");

        builder
            .HasKey(x => x.Id);

        builder
            .Property(x => x.Id)
            .HasConversion(x => x.Value, x => GameId.FromExisting(x));

        builder
            .Property(x => x.StartedAt)
            .IsRequired();

        builder
            .HasIndex(x => x.StartedAt)
            .IsDescending(true);

        builder
            .HasDiscriminator<string>("status")
            .HasValue<GameInProgress>("in_progress")
            .HasValue<FinishedGame>("finished");
    }
}
