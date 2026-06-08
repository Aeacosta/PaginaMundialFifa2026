using FluentAssertions;
using FluentValidation.TestHelper;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using WorldCup2026.Application.DTOs.Match;
using WorldCup2026.Application.Validators.Match;

namespace WorldCup2026.Tests.Validators;

[TestClass]
public class UpdateMatchResultDtoValidatorTests
{
    private UpdateMatchResultDtoValidator _validator;

    [TestInitialize]
    public void Setup()
    {
        _validator = new UpdateMatchResultDtoValidator();
    }

    [TestMethod]
    public async Task Validate_WithValidData_ShouldNotHaveErrors()
    {
        // Arrange
        var dto = new UpdateMatchResultDto
        {
            HomeTeamScore = 2,
            AwayTeamScore = 1
        };

        // Act
        var result = await _validator.TestValidateAsync(dto);

        // Assert
        result.ShouldNotHaveAnyValidationErrors();
    }

    [TestMethod]
    public async Task Validate_WithZeroScores_ShouldNotHaveErrors()
    {
        // Arrange
        var dto = new UpdateMatchResultDto
        {
            HomeTeamScore = 0,
            AwayTeamScore = 0
        };

        // Act
        var result = await _validator.TestValidateAsync(dto);

        // Assert
        result.ShouldNotHaveAnyValidationErrors();
    }

    [TestMethod]
    public async Task Validate_WithNegativeHomeTeamScore_ShouldHaveError()
    {
        // Arrange
        var dto = new UpdateMatchResultDto
        {
            HomeTeamScore = -1,
            AwayTeamScore = 1
        };

        // Act
        var result = await _validator.TestValidateAsync(dto);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.HomeTeamScore);
    }

    [TestMethod]
    public async Task Validate_WithNegativeAwayTeamScore_ShouldHaveError()
    {
        // Arrange
        var dto = new UpdateMatchResultDto
        {
            HomeTeamScore = 1,
            AwayTeamScore = -1
        };

        // Act
        var result = await _validator.TestValidateAsync(dto);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.AwayTeamScore);
    }

    [TestMethod]
    public async Task Validate_WithHighScores_ShouldNotHaveErrors()
    {
        // Arrange
        var dto = new UpdateMatchResultDto
        {
            HomeTeamScore = 10,
            AwayTeamScore = 8
        };

        // Act
        var result = await _validator.TestValidateAsync(dto);

        // Assert
        result.ShouldNotHaveAnyValidationErrors();
    }

    [TestMethod]
    public async Task Validate_WithBothScoresNegative_ShouldHaveMultipleErrors()
    {
        // Arrange
        var dto = new UpdateMatchResultDto
        {
            HomeTeamScore = -2,
            AwayTeamScore = -3
        };

        // Act
        var result = await _validator.TestValidateAsync(dto);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.HomeTeamScore);
        result.ShouldHaveValidationErrorFor(x => x.AwayTeamScore);
    }
}

// Made with Bob