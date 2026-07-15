using FluentAssertions;
using FluentValidation;
using TimeScale.Application.Dtos;
using TimeScale.Application.Validators;

namespace TimeScale.Application.Tests;

public class UploadCsvCommandValidatorTests
{
    private readonly IValidator<UploadCsvCommand> _validator = new UploadCsvCommandValidator();

    [Fact]
    public void Validate_ValidCommand_ReturnsNoErrors()
    {
        var command = new UploadCsvCommand
        {
            FileName = "valid.csv",
            CsvStream = new MemoryStream([1, 2, 3, 4])
        };

        var result = _validator.Validate(command);

        result.IsValid.Should().BeTrue();
        result.Errors.Should().BeEmpty();
    }

    [Fact]
    public void Validate_MissingFileName_ReturnsValidationError()
    {
        var command = new UploadCsvCommand
        {
            FileName = string.Empty,
            CsvStream = new MemoryStream([1, 2, 3, 4])
        };

        var result = _validator.Validate(command);

        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(x => x.PropertyName == nameof(UploadCsvCommand.FileName));
    }

    [Fact]
    public void Validate_NullCsvStream_ReturnsValidationError()
    {
        var command = new UploadCsvCommand
        {
            FileName = "valid.csv",
            CsvStream = null!
        };

        var result = _validator.Validate(command);

        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(x => x.PropertyName == nameof(UploadCsvCommand.CsvStream));
    }
}
