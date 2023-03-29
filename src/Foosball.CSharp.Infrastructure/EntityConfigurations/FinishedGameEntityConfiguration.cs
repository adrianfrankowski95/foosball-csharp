
using Microsoft.EntityFrameworkCore;
using Foosball.CSharp.Domain.GameAggregateModel;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Foosball.CSharp.Domain.TeamAggregateModel;

namespace Foosball.CSharp.Infrastructure.EntityConfigurations;

public class FinishedGameEntityConfiguration : IEntityTypeConfiguration<FinishedGame>
{
    public void Configure(EntityTypeBuilder<FinishedGame> builder)
    {
        builder.HasBaseType<Game>();

        builder
            .HasOne<Team>()
            .WithMany()
            .HasForeignKey(x => x.WinnerTeamId)
            .OnDelete(DeleteBehavior.Restrict);

        builder
            .HasMany(x => x.Sets)
            .WithOne()
            .HasForeignKey(x => x.GameId);

        builder
            .Navigation(x => x.Sets)
            .HasField("_sets")
            .UsePropertyAccessMode(PropertyAccessMode.Field)
            .AutoInclude();

        builder
            .Property(x => x.WinnerTeamId)
            .HasConversion(x => x.Value, x => TeamId.FromExisting(x))
            .IsRequired();
    }
}
