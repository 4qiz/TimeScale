using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TimeScale.Application.Entities;

namespace TimeScale.DataAccess.Configurations
{
    internal class ResultConfiguration : IEntityTypeConfiguration<Result>
    {
        public void Configure(EntityTypeBuilder<Result> builder)
        {
            builder.ToTable("results");

            builder.HasKey(x => x.Id);

            builder.Property(x => x.TimeDeltaSeconds).IsRequired();
            builder.Property(x => x.StartDateUtc).IsRequired();
            builder.Property(x => x.AvgExecutionTime).IsRequired();
            builder.Property(x => x.AvgValue).IsRequired();
            builder.Property(x => x.MedianValue).IsRequired();
            builder.Property(x => x.MaxValue).IsRequired();
            builder.Property(x => x.MinValue).IsRequired();

            builder.HasIndex(x => x.StartDateUtc);
            builder.HasIndex(x => x.AvgValue);
            builder.HasIndex(x => x.AvgExecutionTime);
        }

    }
}
