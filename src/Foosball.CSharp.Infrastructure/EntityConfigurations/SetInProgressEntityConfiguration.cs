
using Microsoft.EntityFrameworkCore;
using Foosball.CSharp.Domain.GameAggregateModel;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Foosball.CSharp.Infrastructure.EntityConfigurations;

public class SetInProgressEntityConfiguration : IEntityTypeConfiguration<SetInProgress>
{
    public void Configure(EntityTypeBuilder<SetInProgress> builder)
    {
        builder.HasBaseType<Set>();
    }
}
