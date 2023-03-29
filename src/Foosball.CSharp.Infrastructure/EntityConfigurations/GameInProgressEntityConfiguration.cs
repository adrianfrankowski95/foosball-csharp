
using Microsoft.EntityFrameworkCore;
using Foosball.CSharp.Domain.GameAggregateModel;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Foosball.CSharp.Infrastructure.EntityConfigurations;

public class GameInProgressEntityConfiguration : IEntityTypeConfiguration<GameInProgress>
{
    public void Configure(EntityTypeBuilder<GameInProgress> builder)
    {
        builder.HasBaseType<Game>();

        builder
            .HasMany(x => x.Sets)
            .WithOne()
            .HasForeignKey(x => x.GameId);

        builder
            .Navigation(x => x.Sets)
            .HasField("_sets")
            .UsePropertyAccessMode(PropertyAccessMode.Field)
            .AutoInclude();
    }
}
