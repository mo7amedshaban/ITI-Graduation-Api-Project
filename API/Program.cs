using API.Handlers;
using Application.Features.Courses.Commands.CreateCourse;
using Application.Features.Courses.Mappers;
using Core.Interfaces;
using Infrastructure.Common;
using Infrastructure.Common.GenRepo;
using Infrastructure.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

var connectionString = builder.Configuration.GetConnectionString("DataBase");

builder.Services.AddDbContext<AppDBContext>(options =>
{
    options.UseSqlServer(connectionString, b => b.MigrationsAssembly(typeof(AppDBContext).Assembly.FullName));
    options.UseSqlServer(connectionString, b =>
        b.MigrationsAssembly(typeof(AppDBContext).Assembly.FullName));
});

builder.Services.AddIdentityApiEndpoints<IdentityUser<Guid>>()
    .AddRoles<IdentityRole<Guid>>()
    .AddEntityFrameworkStores<AppDBContext>()
    .AddDefaultTokenProviders();

builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
// builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblyContaining<Program>());

builder.Services.AddMediatR(cfg =>
    cfg.RegisterServicesFromAssemblyContaining<CreateCourseCommand>());
// builder.Services.AddAutoMapper(cfg => cfg.AddMaps(typeof(Program).Assembly));
builder.Services.AddAutoMapper(cfg => cfg.AddMaps(typeof(CourseProfile).Assembly));

// Replace AddOpenApi() with AddSwaggerGen and an OpenAPI document
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "RepositoryPattern UnitOfWork API",
        Version = "v1"
    });
});

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddProblemDetails();
builder.Services.AddExceptionHandler<GlobalExceptionHandler>();

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI(c => { c.SwaggerEndpoint("/swagger/v1/swagger.json", "RepositoryPattern UnitOfWork API v1"); });
// app.UseExceptionHandler();
app.UseSwaggerUI();

// app.UseExceptionHandler();

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();