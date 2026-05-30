using FluentValidation;
using WorldCup2026.Application.DTOs.Match;

namespace WorldCup2026.Application.Validators.Match;

public class UpdateMatchResultDtoValidator : AbstractValidator<UpdateMatchResultDto>
{
    public UpdateMatchResultDtoValidator()
    {
        RuleFor(x => x.HomeTeamScore)
            .GreaterThanOrEqualTo(0).WithMessage("Home team score cannot be negative");

        RuleFor(x => x.AwayTeamScore)
            .GreaterThanOrEqualTo(0).WithMessage("Away team score cannot be negative");

        RuleFor(x => x.HomeTeamPenalties)
            .GreaterThanOrEqualTo(0).WithMessage("Home team penalties cannot be negative")
            .When(x => x.HomeTeamPenalties.HasValue);

        RuleFor(x => x.AwayTeamPenalties)
            .GreaterThanOrEqualTo(0).WithMessage("Away team penalties cannot be negative")
            .When(x => x.AwayTeamPenalties.HasValue);

        // Both penalty scores must be set together or not at all
        RuleFor(x => x)
            .Must(x => (x.HomeTeamPenalties.HasValue && x.AwayTeamPenalties.HasValue) ||
                      (!x.HomeTeamPenalties.HasValue && !x.AwayTeamPenalties.HasValue))
            .WithMessage("Both penalty scores must be provided together or not at all");

        // Penalty scores should only be used when regular scores are tied
        RuleFor(x => x)
            .Must(x => !x.HomeTeamPenalties.HasValue || x.HomeTeamScore == x.AwayTeamScore)
            .WithMessage("Penalty scores should only be used when regular time scores are tied");
    }
}

// Made with Bob
