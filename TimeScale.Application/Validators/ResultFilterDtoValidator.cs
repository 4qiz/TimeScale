using FluentValidation;
using TimeScale.Application.Dtos;

namespace TimeScale.Application.Validators
{
    public sealed class ResultFilterDtoValidator : AbstractValidator<ResultFilterDto>
    {
        public ResultFilterDtoValidator()
        {
            RuleFor(x => x.FileName)
                .MaximumLength(255)
                .WithMessage("File name must not exceed 255 characters.");

            RuleFor(x => x.StartDateFrom)
                .LessThanOrEqualTo(x => x.StartDateTo)
                .When(x => x.StartDateFrom.HasValue && x.StartDateTo.HasValue)
                .WithMessage("StartDateFrom must be less than or equal to StartDateTo.");

            RuleFor(x => x.AvgValueFrom)
                .LessThanOrEqualTo(x => x.AvgValueTo)
                .When(x => x.AvgValueFrom.HasValue && x.AvgValueTo.HasValue)
                .WithMessage("AvgValueFrom must be less than or equal to AvgValueTo.");

            RuleFor(x => x.AvgExecutionTimeFrom)
                .LessThanOrEqualTo(x => x.AvgExecutionTimeTo)
                .When(x => x.AvgExecutionTimeFrom.HasValue && x.AvgExecutionTimeTo.HasValue)
                .WithMessage("AvgExecutionTimeFrom must be less than or equal to AvgExecutionTimeTo.");
        }
    }
}
