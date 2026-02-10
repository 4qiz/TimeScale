using TimeScale.Application.Dtos;
using TimeScale.Application.Entities;
using TimeScale.Application.Interfaces;

namespace TimeScale.Application.Services
{
    public sealed class FileProcessingService(
    IUploadedFileRepository uploadedFileRepository,
    IResultRepository resultRepository,
    IValueRecordRepository valueRecordRepository,
    IUnitOfWork unitOfWork,
    ICsvParser parser,
    ICsvDomainValidator validator,
    IResultCalculator calculator) : IFileProcessingService
    {
        public async Task UploadAndProcessAsync(UploadCsvCommand command, CancellationToken ct)
        {
            ArgumentNullException.ThrowIfNull(command);
            ArgumentNullException.ThrowIfNull(command.CsvStream);

            await unitOfWork.BeginTransactionAsync(ct);

            try
            {
                await RemoveExistingFileAsync(command.FileName, ct);

                var records = await parser.ParseAsync(command.CsvStream, ct);

                validator.Validate(records);

                UploadedFile uploadedFile = BuildUploadedFile(command.FileName, records);

                uploadedFile.Result = calculator.Calculate(uploadedFile.ValueRecords);

                await uploadedFileRepository.AddAsync(uploadedFile, ct);

                await unitOfWork.SaveChangesAsync(ct);
                await unitOfWork.CommitAsync(ct);
            }
            catch
            {
                await unitOfWork.RollbackAsync(ct);
                throw;
            }
        }

        private async Task RemoveExistingFileAsync(string fileName, CancellationToken ct)
        {
            var existingFile =
                await uploadedFileRepository.GetByFileNameWithRelationsAsync(fileName, ct);

            if (existingFile is null)
            {
                return;
            }

            await valueRecordRepository.RemoveRangeAsync(existingFile.ValueRecords, ct);

            if (existingFile.Result != null)
                await resultRepository.RemoveAsync(existingFile.Result, ct);

            await uploadedFileRepository.RemoveAsync(existingFile, ct);

            await unitOfWork.SaveChangesAsync(ct);
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
