using FluentValidation;
using WorldCup2026.Application.DTOs.Team;
using WorldCup2026.Domain.Enums;

namespace WorldCup2026.Application.Validators.Team;

public class CreateTeamDtoValidator : AbstractValidator<CreateTeamDto>
{
    public CreateTeamDtoValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Team name is required")
            .MaximumLength(100).WithMessage("Team name cannot exceed 100 characters");

        RuleFor(x => x.Code)
            .NotEmpty().WithMessage("Team code is required")
            .Length(3).WithMessage("Team code must be exactly 3 characters")
            .Matches("^[A-Z]{3}$").WithMessage("Team code must be 3 uppercase letters");

        RuleFor(x => x.Confederation)
            .IsInEnum().WithMessage("Invalid confederation");

        RuleFor(x => x.FifaRanking)
            .GreaterThan(0).WithMessage("FIFA ranking must be greater than 0")
            .When(x => x.FifaRanking.HasValue);

        RuleFor(x => x.FlagUrl)
            .Must(url => Uri.TryCreate(url, UriKind.Absolute, out _))
            .WithMessage("Flag URL must be a valid URL")
            .When(x => !string.IsNullOrEmpty(x.FlagUrl));
    }
}

// Made with Bob
