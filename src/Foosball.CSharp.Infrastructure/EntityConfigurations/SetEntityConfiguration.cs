
using Microsoft.EntityFrameworkCore;
using Foosball.CSharp.Domain.AggregateModel;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

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
            .OwnsOne(x => x.Result, r =>
            {
                r.Property(x => x.TeamAGoals)
                    .HasConversion(
                        g => g.Value,
                        g => g.Goals())
                    .IsRequired();

                r.Property(x => x.TeamBGoals).
                    HasConversion(
                        g => g.Value,
                        g => g.Goals())
                    .IsRequired();

                r.WithOwner();
            });

        builder
            .HasDiscriminator<string>("status")
            .HasValue<SetInProgress>("in_progress")
            .HasValue<FinishedSet>("finished");

        builder
            .HasOne<Game>()
            .WithMany(x => x.Sets)
            .HasForeignKey(x => x.GameId)
            .OnDelete(DeleteBehavior.Cascade)
            .IsRequired();

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

        builder
            .Property<byte[]>("row_version")
            .HasColumnName("row_version")
            .IsRowVersion();
    }
}
