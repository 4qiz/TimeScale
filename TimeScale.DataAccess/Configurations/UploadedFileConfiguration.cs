using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;
using TimeScale.DataAccess.Entities;

namespace TimeScale.DataAccess.Configurations
{
    internal class UploadedFileConfiguration : IEntityTypeConfiguration<UploadedFile>
    {
        public void Configure(EntityTypeBuilder<UploadedFile> builder)
        {
            builder.ToTable("uploaded_files");

            builder.HasKey(x => x.Id);

            builder.Property(x => x.FileName)
                .IsRequired()
                .HasMaxLength(255);

            builder.HasIndex(x => x.FileName)
                .IsUnique();

            builder.Property(x => x.UploadedAtUtc)
                .IsRequired();

            builder.Property(x => x.RowsCount)
                .IsRequired();

            builder.HasMany(x => x.ValueRecords)
                .WithOne(v => v.UploadedFile)
                .HasForeignKey(v => v.UploadedFileId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(x => x.Result)
                .WithOne(r => r.UploadedFile)
                .HasForeignKey<Result>(r => r.UploadedFileId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }

}
