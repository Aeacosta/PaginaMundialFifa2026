using FluentValidation;
using WorldCup2026.Application.DTOs.Match;
using WorldCup2026.Domain.Enums;

namespace WorldCup2026.Application.Validators.Match;

public class CreateMatchDtoValidator : AbstractValidator<CreateMatchDto>
{
    public CreateMatchDtoValidator()
    {
        RuleFor(x => x.HomeTeamId)
            .NotEmpty().WithMessage("Home team ID is required");

        RuleFor(x => x.AwayTeamId)
            .NotEmpty().WithMessage("Away team ID is required")
            .NotEqual(x => x.HomeTeamId).WithMessage("Away team must be different from home team");

        RuleFor(x => x.StadiumId)
            .NotEmpty().WithMessage("Stadium ID is required");

        RuleFor(x => x.MatchDate)
            .NotEmpty().WithMessage("Match date is required")
            .GreaterThan(DateTime.UtcNow).WithMessage("Match date must be in the future");

        RuleFor(x => x.Phase)
            .IsInEnum().WithMessage("Invalid match phase");

        RuleFor(x => x.GroupId)
            .NotEmpty().WithMessage("Group ID is required for group stage matches")
            .When(x => x.Phase == MatchPhase.GroupStage);

        RuleFor(x => x.GroupId)
            .Empty().WithMessage("Group ID should not be set for knockout stage matches")
            .When(x => x.Phase != MatchPhase.GroupStage);
    }
}

// Made with Bob
