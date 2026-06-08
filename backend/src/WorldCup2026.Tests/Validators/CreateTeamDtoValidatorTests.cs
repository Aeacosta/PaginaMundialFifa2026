using FluentAssertions;
using FluentValidation.TestHelper;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using WorldCup2026.Application.DTOs.Team;
using WorldCup2026.Application.Validators.Team;
using WorldCup2026.Domain.Enums;

namespace WorldCup2026.Tests.Validators;

[TestClass]
public class CreateTeamDtoValidatorTests
{
    private CreateTeamDtoValidator _validator;

    [TestInitialize]
    public void Setup()
    {
        _validator = new CreateTeamDtoValidator();
    }

    [TestMethod]
    public async Task Validate_WithValidData_ShouldNotHaveErrors()
    {
        // Arrange
        var dto = new CreateTeamDto
        {
            Name = "Argentina",
            Code = "ARG",
            Confederation = Confederation.CONMEBOL,
            FifaRanking = 1,
            GroupId = 1,
            FlagUrl = "https://example.com/flag.png"
        };

        // Act
        var result = await _validator.TestValidateAsync(dto);

        // Assert
        result.ShouldNotHaveAnyValidationErrors();
    }

    [TestMethod]
    public async Task Validate_WithEmptyName_ShouldHaveError()
    {
        // Arrange
        var dto = new CreateTeamDto
        {
            Name = "",
            Code = "ARG",
            Confederation = Confederation.CONMEBOL,
            FifaRanking = 1
        };

        // Act
        var result = await _validator.TestValidateAsync(dto);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Name);
    }

    [TestMethod]
    public async Task Validate_WithNullName_ShouldHaveError()
    {
        // Arrange
        var dto = new CreateTeamDto
        {
            Name = null!,
            Code = "ARG",
            Confederation = Confederation.CONMEBOL,
            FifaRanking = 1
        };

        // Act
        var result = await _validator.TestValidateAsync(dto);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Name);
    }

    [TestMethod]
    public async Task Validate_WithNameTooLong_ShouldHaveError()
    {
        // Arrange
        var dto = new CreateTeamDto
        {
            Name = new string('A', 101),
            Code = "ARG",
            Confederation = Confederation.CONMEBOL,
            FifaRanking = 1
        };

        // Act
        var result = await _validator.TestValidateAsync(dto);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Name);
    }

    [TestMethod]
    public async Task Validate_WithEmptyCode_ShouldHaveError()
    {
        // Arrange
        var dto = new CreateTeamDto
        {
            Name = "Argentina",
            Code = "",
            Confederation = Confederation.CONMEBOL,
            FifaRanking = 1
        };

        // Act
        var result = await _validator.TestValidateAsync(dto);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Code);
    }

    [TestMethod]
    public async Task Validate_WithCodeTooShort_ShouldHaveError()
    {
        // Arrange
        var dto = new CreateTeamDto
        {
            Name = "Argentina",
            Code = "AR",
            Confederation = Confederation.CONMEBOL,
            FifaRanking = 1
        };

        // Act
        var result = await _validator.TestValidateAsync(dto);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Code);
    }

    [TestMethod]
    public async Task Validate_WithCodeTooLong_ShouldHaveError()
    {
        // Arrange
        var dto = new CreateTeamDto
        {
            Name = "Argentina",
            Code = "ARGG",
            Confederation = Confederation.CONMEBOL,
            FifaRanking = 1
        };

        // Act
        var result = await _validator.TestValidateAsync(dto);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Code);
    }

    [TestMethod]
    public async Task Validate_WithNegativeFifaRanking_ShouldHaveError()
    {
        // Arrange
        var dto = new CreateTeamDto
        {
            Name = "Argentina",
            Code = "ARG",
            Confederation = Confederation.CONMEBOL,
            FifaRanking = -1
        };

        // Act
        var result = await _validator.TestValidateAsync(dto);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.FifaRanking);
    }

    [TestMethod]
    public async Task Validate_WithZeroFifaRanking_ShouldHaveError()
    {
        // Arrange
        var dto = new CreateTeamDto
        {
            Name = "Argentina",
            Code = "ARG",
            Confederation = Confederation.CONMEBOL,
            FifaRanking = 0
        };

        // Act
        var result = await _validator.TestValidateAsync(dto);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.FifaRanking);
    }

    [TestMethod]
    public async Task Validate_WithInvalidFlagUrl_ShouldHaveError()
    {
        // Arrange
        var dto = new CreateTeamDto
        {
            Name = "Argentina",
            Code = "ARG",
            Confederation = Confederation.CONMEBOL,
            FifaRanking = 1,
            FlagUrl = "not-a-url"
        };

        // Act
        var result = await _validator.TestValidateAsync(dto);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.FlagUrl);
    }

    [TestMethod]
    public async Task Validate_WithNullFlagUrl_ShouldNotHaveError()
    {
        // Arrange
        var dto = new CreateTeamDto
        {
            Name = "Argentina",
            Code = "ARG",
            Confederation = Confederation.CONMEBOL,
            FifaRanking = 1,
            FlagUrl = null
        };

        // Act
        var result = await _validator.TestValidateAsync(dto);

        // Assert
        result.ShouldNotHaveValidationErrorFor(x => x.FlagUrl);
    }

    [TestMethod]
    public async Task Validate_WithNullGroupId_ShouldNotHaveError()
    {
        // Arrange
        var dto = new CreateTeamDto
        {
            Name = "Argentina",
            Code = "ARG",
            Confederation = Confederation.CONMEBOL,
            FifaRanking = 1,
            GroupId = null
        };

        // Act
        var result = await _validator.TestValidateAsync(dto);

        // Assert
        result.ShouldNotHaveValidationErrorFor(x => x.GroupId);
    }
}

// Made with Bob