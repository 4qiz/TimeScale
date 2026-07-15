using FluentValidation;
using TimeScale.Api.Dtos;

namespace TimeScale.Api.Validators
{
    /// <summary>
    /// Validates the multipart form payload used for CSV uploads.
    /// </summary>
    public sealed class UploadCsvFormValidator : AbstractValidator<UploadCsvForm>
    {
        private static readonly string[] AllowedExtensions = [".csv"];

        /// <summary>
        /// Initializes a new instance of the <see cref="UploadCsvFormValidator"/> class.
        /// </summary>
        public UploadCsvFormValidator()
        {
            RuleFor(x => x.File)
                .NotNull()
                .WithMessage("A CSV file is required.");

            RuleFor(x => x.File)
                .Must(file => file is null || file.Length > 0)
                .WithMessage("The uploaded file must not be empty.");

            RuleFor(x => x.File)
                .Must(file => file is null || AllowedExtensions.Contains(Path.GetExtension(file?.FileName ?? string.Empty)))
                .WithMessage("Only CSV files are supported.");
        }
    }
}
