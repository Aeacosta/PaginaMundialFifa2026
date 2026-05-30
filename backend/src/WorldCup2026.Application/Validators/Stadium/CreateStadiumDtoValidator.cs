using FluentValidation;
using WorldCup2026.Application.DTOs.Stadium;

namespace WorldCup2026.Application.Validators.Stadium;

public class CreateStadiumDtoValidator : AbstractValidator<CreateStadiumDto>
{
    public CreateStadiumDtoValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Stadium name is required")
            .MaximumLength(200).WithMessage("Stadium name cannot exceed 200 characters");

        RuleFor(x => x.City)
            .NotEmpty().WithMessage("City is required")
            .MaximumLength(100).WithMessage("City name cannot exceed 100 characters");

        RuleFor(x => x.Country)
            .NotEmpty().WithMessage("Country is required")
            .MaximumLength(100).WithMessage("Country name cannot exceed 100 characters");

        RuleFor(x => x.Capacity)
            .GreaterThan(0).WithMessage("Capacity must be greater than 0");

        RuleFor(x => x.Latitude)
            .InclusiveBetween(-90, 90).WithMessage("Latitude must be between -90 and 90")
            .When(x => x.Latitude.HasValue);

        RuleFor(x => x.Longitude)
            .InclusiveBetween(-180, 180).WithMessage("Longitude must be between -180 and 180")
            .When(x => x.Longitude.HasValue);
    }
}

// Made with Bob
