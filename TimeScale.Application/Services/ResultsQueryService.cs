using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using TimeScale.Application.Dtos;
using TimeScale.Application.Interfaces;
using TimeScale.DataAccess;

namespace TimeScale.Application.Services
{
    public class ResultsQueryService(AppDbContext db) : IResultsQueryService
    {
        public async Task<IReadOnlyCollection<ResultDto>> GetResultsAsync(ResultFilterDto filter, CancellationToken ct)
        {
            var query = db.Results
                .Include(x => x.UploadedFile)
                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(filter.FileName))
                query = query.Where(x => x.UploadedFile.FileName == filter.FileName);

            if (filter.StartDateFrom.HasValue)
                query = query.Where(x => x.StartDateUtc >= filter.StartDateFrom);

            if (filter.StartDateTo.HasValue)
                query = query.Where(x => x.StartDateUtc <= filter.StartDateTo);

            if (filter.AvgValueFrom.HasValue)
                query = query.Where(x => x.AvgValue >= filter.AvgValueFrom);

            if (filter.AvgValueTo.HasValue)
                query = query.Where(x => x.AvgValue <= filter.AvgValueTo);

            if (filter.AvgExecutionTimeFrom.HasValue)
                query = query.Where(x => x.AvgExecutionTime >= filter.AvgExecutionTimeFrom);

            if (filter.AvgExecutionTimeTo.HasValue)
                query = query.Where(x => x.AvgExecutionTime <= filter.AvgExecutionTimeTo);

            return await query
                .Select(x => new ResultDto
                {
                    Id = x.Id,
                    FileName = x.UploadedFile.FileName,
                    StartDateUtc = x.StartDateUtc,
                    TimeDeltaSeconds = x.TimeDeltaSeconds,
                    AvgExecutionTime = x.AvgExecutionTime,
                    AvgValue = x.AvgValue,
                    MedianValue = x.MedianValue,
                    MaxValue = x.MaxValue,
                    MinValue = x.MinValue
                })
                .ToListAsync(ct);
        }

        public async Task<IReadOnlyCollection<ValueRecordDto>> GetLastValuesAsync(string fileName, CancellationToken ct)
        {
            return await db.ValueRecords
                .Where(x => x.UploadedFile.FileName == fileName)
                .OrderByDescending(x => x.DateUtc)
                .Take(10)
                .Select(x => new ValueRecordDto
                {
                    DateUtc = x.DateUtc,
                    ExecutionTime = x.ExecutionTime,
                    Value = x.Value
                })
                .ToListAsync(ct);
        }
    }
}
