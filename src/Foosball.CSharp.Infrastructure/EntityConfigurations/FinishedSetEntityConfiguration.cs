
using Microsoft.EntityFrameworkCore;
using Foosball.CSharp.Domain.GameAggregateModel;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Foosball.CSharp.Domain.TeamAggregateModel;

namespace Foosball.CSharp.Infrastructure.EntityConfigurations;

public class FinishedSetEntityConfiguration : IEntityTypeConfiguration<FinishedSet>
{
    public void Configure(EntityTypeBuilder<FinishedSet> builder)
    {
        builder.HasBaseType<Set>();

        builder
            .HasOne<Team>()
            .WithMany()
            .HasForeignKey(x => x.WinnerTeamId)
            .OnDelete(DeleteBehavior.Restrict);

        builder
            .Property(x => x.WinnerTeamId)
            .HasConversion(x => x.Value, x => TeamId.FromExisting(x))
            .IsRequired();

        builder
            .Property(x => x.FinishedAt)
            .IsRequired();
    }
}
