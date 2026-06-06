using FluentValidation;
using Microsoft.EntityFrameworkCore;
using WorldCup2026.Application.Interfaces;
using WorldCup2026.Application.Mappings;
using WorldCup2026.Application.Services;
using WorldCup2026.Application.Validators.Team;
using WorldCup2026.Domain.Interfaces;
using WorldCup2026.Infrastructure.Data;
using WorldCup2026.Infrastructure.Data.Seeding;
using WorldCup2026.Infrastructure.Repositories;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container
builder.Services.AddControllers();

// Configure Database Context
builder.Services.AddDbContext<WorldCupDbContext>(options =>
    options.UseSqlite(
        builder.Configuration.GetConnectionString("DefaultConnection"),
        sqliteOptions => sqliteOptions.MigrationsAssembly("WorldCup2026.Infrastructure")
    )
);

// Register AutoMapper
builder.Services.AddAutoMapper(typeof(MappingProfile));

// Register FluentValidation
builder.Services.AddValidatorsFromAssemblyContaining<CreateTeamDtoValidator>();

// Register Repositories
builder.Services.AddScoped<ITeamRepository, TeamRepository>();
builder.Services.AddScoped<IGroupRepository, GroupRepository>();
builder.Services.AddScoped<IMatchRepository, MatchRepository>();
builder.Services.AddScoped<IStandingRepository, StandingRepository>();
builder.Services.AddScoped<IStadiumRepository, StadiumRepository>();

// Register Unit of Work
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

// Register Services
builder.Services.AddScoped<ITeamService, TeamService>();
builder.Services.AddScoped<IGroupService, GroupService>();
builder.Services.AddScoped<IStadiumService, StadiumService>();
builder.Services.AddScoped<IStandingService, StandingService>();
builder.Services.AddScoped<IMatchService, MatchService>();
builder.Services.AddScoped<IDashboardService, DashboardService>();

// Register Data Seeders
builder.Services.AddScoped<GroupSeeder>();
builder.Services.AddScoped<StadiumSeeder>();
builder.Services.AddScoped<TeamSeeder>();
builder.Services.AddScoped<StandingSeeder>();
builder.Services.AddScoped<MatchSeeder>();
builder.Services.AddScoped<JsonMatchSeeder>();
builder.Services.AddScoped<DataSeeder>();

// Configure CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});

// Configure Swagger/OpenAPI
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/swagger/v1/swagger.json", "FIFA World Cup 2026 API v1");
        options.RoutePrefix = string.Empty; // Swagger UI at root
    });
}

app.UseHttpsRedirection();

app.UseCors("AllowAll");

app.UseAuthorization();

app.MapControllers();

// Add seeding endpoint for development
if (app.Environment.IsDevelopment())
{
    app.MapPost("/api/seed", async (DataSeeder seeder) =>
    {
        await seeder.SeedAllAsync();
        return Results.Ok(new { message = "Database seeded successfully" });
    })
    .WithName("SeedDatabase")
    .WithOpenApi();
}

app.Run();

// Made with Bob
