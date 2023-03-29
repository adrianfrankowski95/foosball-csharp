
using Microsoft.EntityFrameworkCore;
using Foosball.CSharp.Domain.GameAggregateModel;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Foosball.CSharp.Domain.TeamAggregateModel;

namespace Foosball.CSharp.Infrastructure.EntityConfigurations;

public class SetEntityConfiguration : IEntityTypeConfiguration<Set>
{
    public void Configure(EntityTypeBuilder<Set> builder)
    {
        builder.ToTable("sets");

        builder
            .HasKey(x => x.Id);

        builder
            .Property(x => x.Id)
            .HasConversion(x => x.Value, x => SetId.FromExisting(x));

        builder
            .OwnsOne(x => x.Scores, r =>
            {
                r.Property(x => x.TeamAScore)
                    .HasConversion(
                        g => g.Value,
                        g => g.Goals())
                    .HasColumnName("team_a_score")
                    .IsRequired();

                r.Property(x => x.TeamBScore)
                    .HasConversion(
                        g => g.Value,
                        g => g.Goals())
                    .HasColumnName("team_b_score")
                    .IsRequired();

                r.WithOwner();
            });

        builder
            .HasDiscriminator<string>("status")
            .HasValue<SetInProgress>("in_progress")
            .HasValue<FinishedSet>("finished");

        builder
            .Property(x => x.GameId)
            .HasConversion(x => x.Value, x => GameId.FromExisting(x))
            .IsRequired();

        builder
            .HasOne<Team>()
            .WithMany()
            .HasForeignKey(x => x.TeamAId)
            .OnDelete(DeleteBehavior.Restrict)
            .IsRequired();

        builder
            .Property(x => x.TeamAId)
            .HasConversion(x => x.Value, x => TeamId.FromExisting(x))
            .IsRequired();

        builder
            .HasOne<Team>()
            .WithMany()
            .HasForeignKey(x => x.TeamBId)
            .OnDelete(DeleteBehavior.Restrict)
            .IsRequired();

        builder
            .Property(x => x.TeamBId)
            .HasConversion(x => x.Value, x => TeamId.FromExisting(x))
            .IsRequired();
    }
}
