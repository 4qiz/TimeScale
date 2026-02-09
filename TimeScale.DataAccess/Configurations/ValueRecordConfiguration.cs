using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TimeScale.DataAccess.Entities;

namespace TimeScale.DataAccess.Configurations
{
    internal class ValueRecordConfiguration : IEntityTypeConfiguration<ValueRecord>
    {
        public void Configure(EntityTypeBuilder<ValueRecord> builder)
        {
            builder.ToTable("values");

            builder.HasKey(x => x.Id);

            builder.Property(x => x.DateUtc)
                .IsRequired();

            builder.Property(x => x.ExecutionTime)
                .IsRequired();

            builder.Property(x => x.Value)
                .IsRequired();

            builder.HasIndex(x => new { x.UploadedFileId, x.DateUtc });
        }
    }

}
