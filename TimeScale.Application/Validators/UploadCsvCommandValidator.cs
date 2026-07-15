using FluentValidation;
using TimeScale.Application.Dtos;

namespace TimeScale.Application.Validators
{
    public sealed class UploadCsvCommandValidator : AbstractValidator<UploadCsvCommand>
    {
        public UploadCsvCommandValidator()
        {
            RuleFor(x => x.FileName)
                .NotEmpty()
                .WithMessage("File name is required.")
                .MaximumLength(255)
                .WithMessage("File name must not exceed 255 characters.");

            RuleFor(x => x.CsvStream)
                .NotNull()
                .WithMessage("CSV content is required.")
                .Must(stream => stream is not null && stream.CanRead)
                .WithMessage("CSV stream must be readable.");
        }
    }
}
