using System.Reflection;
using API.Handlers;
using Application;
using Application.Features.Courses.Commands.CreateCourse;
using Application.Features.Courses.Mappers;
using Core.Interfaces;
using Infrastructure;
using Infrastructure.Common;
using Infrastructure.Common.GenRepo;
using Infrastructure.Data;
using Infrastructure.Extension;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

var connectionString = builder.Configuration.GetConnectionString("DataBase");

builder.Services.AddInfrastructureDependencies().AddServiceDependencies()
           .AddServiceRegisteration(builder.Configuration);

builder.Services.AddDbContext<AppDBContext>(options =>
{
    options.UseSqlServer(connectionString, b => b.MigrationsAssembly(typeof(AppDBContext).Assembly.FullName));
    options.UseSqlServer(connectionString, b =>
        b.MigrationsAssembly(typeof(AppDBContext).Assembly.FullName));
});


builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
// builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblyContaining<Program>());

builder.Services.AddMediatR(cfg =>
    cfg.RegisterServicesFromAssemblyContaining<CreateCourseCommand>());
// builder.Services.AddAutoMapper(cfg => cfg.AddMaps(typeof(Program).Assembly));
builder.Services.AddAutoMapper(cfg => cfg.AddMaps(typeof(CourseProfile).Assembly));

// Replace AddOpenApi() with AddSwaggerGen and an OpenAPI document
#region Swagger Config

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "E-Learning API",
        Version = "v1",
        Description = "API for E-Learning Platform"
    });

    // Add JWT Bearer authentication support
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = "JWT Authorization header using the Bearer scheme. Example: \"Authorization: Bearer {token}\"",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer"
    });

    c.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            }
                        },
                        new string[] {}
                    }
                });


    try
    {
        var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
        var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
        if (File.Exists(xmlPath))
        {
            c.IncludeXmlComments(xmlPath);
        }
    }
    catch
    {
        // Ignore if XML comments aren't available
    }
});
#endregion



builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddProblemDetails();
builder.Services.AddExceptionHandler<GlobalExceptionHandler>();

var app = builder.Build();


using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    await services.SeedRolesAsync();
}
#region Swagger Middleware
app.UseSwagger();
// <<<<<<< HEAD
// app.UseSwaggerUI(c => { c.SwaggerEndpoint("/swagger/v1/swagger.json", "RepositoryPattern UnitOfWork API v1"); });
// // app.UseExceptionHandler();
// app.UseSwaggerUI();
// =======
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "E-Learning API V1");
    c.RoutePrefix = "swagger"; // This makes it available at /swagger

    // For production, you might want to hide the Swagger UI
    // but keep the JSON available for API consumers
    if (!app.Environment.IsDevelopment())
    {
        c.DocumentTitle = "API Documentation - Production";
    }
});

#endregion

// app.UseExceptionHandler();

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();