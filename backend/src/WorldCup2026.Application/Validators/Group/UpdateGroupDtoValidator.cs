using FluentValidation;
using WorldCup2026.Application.DTOs.Group;

namespace WorldCup2026.Application.Validators.Group;

public class UpdateGroupDtoValidator : AbstractValidator<UpdateGroupDto>
{
    public UpdateGroupDtoValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Group name is required")
            .Length(1).WithMessage("Group name must be exactly 1 character")
            .Matches("^[A-L]$").WithMessage("Group name must be a letter from A to L");
    }
}

// Made with Bob
