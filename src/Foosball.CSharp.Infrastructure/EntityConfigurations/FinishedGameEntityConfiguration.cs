
using Microsoft.EntityFrameworkCore;
using Foosball.CSharp.Domain.AggregateModel;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

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
            .Property(x => x.WinnerTeamId)
            .HasConversion(x => x.Value, x => TeamId.FromExisting(x))
            .IsRequired();

        builder
            .Property<byte[]>("row_version")
            .HasColumnName("row_version")
            .IsRowVersion();
    }
}
