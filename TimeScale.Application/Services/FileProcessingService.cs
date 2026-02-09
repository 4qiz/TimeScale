

using Microsoft.EntityFrameworkCore;
using System.Globalization;
using TimeScale.Application.Dtos;
using TimeScale.Application.Exceptions;
using TimeScale.Application.Interfaces;
using TimeScale.DataAccess;
using TimeScale.DataAccess.Entities;

namespace TimeScale.Application.Services
{
    public sealed class FileProcessingService(
        AppDbContext db,
        ICsvParser parser,
        ICsvDomainValidator validator,
        IResultCalculator calculator) : IFileProcessingService
    {
        public async Task UploadAndProcessAsync(UploadCsvCommand command, CancellationToken ct)
        {
            ArgumentNullException.ThrowIfNull(command);
            ArgumentNullException.ThrowIfNull(command.CsvStream);

            using var transaction = await db.Database.BeginTransactionAsync(ct);

            await RemoveExistingFileAsync(command.FileName, ct);

            var records = await parser.ParseAsync(command.CsvStream, ct);

            validator.Validate(records);

            var uploadedFile = BuildUploadedFile(command.FileName, records);

            uploadedFile.Result = calculator.Calculate(uploadedFile.ValueRecords);

            db.UploadedFiles.Add(uploadedFile);

            await db.SaveChangesAsync(ct);
            await transaction.CommitAsync(ct);
        }

        private async Task RemoveExistingFileAsync(string fileName, CancellationToken ct)
        {
            var existingFile = await db.UploadedFiles
                .Include(x => x.ValueRecords)
                .Include(x => x.Result)
                .FirstOrDefaultAsync(x => x.FileName == fileName, ct);

            if (existingFile is null)
                return;

            db.ValueRecords.RemoveRange(existingFile.ValueRecords);

            if (existingFile.Result != null)
                db.Results.Remove(existingFile.Result);

            db.UploadedFiles.Remove(existingFile);

            await db.SaveChangesAsync(ct);
        }

        private static UploadedFile BuildUploadedFile(
            string fileName,
            IReadOnlyCollection<ValueRecordDto> records)
        {
            var uploadedFile = new UploadedFile
            {
                Id = Guid.NewGuid(),
                FileName = fileName,
                UploadedAtUtc = DateTime.UtcNow,
                RowsCount = records.Count
            };

            foreach (var r in records)
            {
                uploadedFile.ValueRecords.Add(new ValueRecord
                {
                    DateUtc = r.DateUtc,
                    ExecutionTime = r.ExecutionTime,
                    Value = r.Value
                });
            }

            return uploadedFile;
        }
    }

}
