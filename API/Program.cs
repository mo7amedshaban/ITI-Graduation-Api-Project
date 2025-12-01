using System.Reflection;
using API.Configurations.Scalar;
using API.Extensions;
using API.Handlers;
using Application;
using Application.Features.Courses.Commands.CreateCourse;
using Application.Features.Courses.Mappers;
using Application.Features.Exam.Mappers;
using Core.Interfaces;
using Core.Interfaces.Services;
using Infrastructure;
using Infrastructure.Common;
using Infrastructure.Common.GenRepo;
using Infrastructure.Data;
using Infrastructure.Extension;
using Infrastructure.services;
using Infrastructure.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);
//builder.WebHost.UseWebRoot("wwwroot");

var connectionString = builder.Configuration.GetConnectionString("DataBase");

builder.Services.AddInfrastructureDependencies().AddServiceDependencies()
    .AddServiceRegisteration(builder.Configuration);

builder.Services.AddDbContext<AppDBContext>(options =>
{
    options.UseSqlServer(connectionString, b => b.MigrationsAssembly(typeof(AppDBContext).Assembly.FullName));
    options.UseSqlServer(connectionString, b =>
        b.MigrationsAssembly(typeof(AppDBContext).Assembly.FullName));
});
builder.Services.AddStackExchangeRedisCache(options =>
{
    options.Configuration = builder.Configuration.GetConnectionString("Redis");
});

builder.Services.AddResponseCaching();


builder.Services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
builder.Services.AddScoped<IFileStorageService, FileStorageService>();
builder.Services.AddScoped<IStudentRepository, StudentRepository>();
builder.Services.AddScoped<IRefreshTokenRepository, RefreshTokenRepository>();
builder.Services.AddScoped<ITokenService, TokenService>();
builder.Services.AddHttpContextAccessor();
builder.Services.AddScoped<IUserContextService, UserContextService>();
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();


builder.Services.AddMediatR(cfg =>
    cfg.RegisterServicesFromAssemblyContaining<CreateCourseCommand>());
builder.Services.AddOpenApi();

builder.Services.AddAutoMapper(cfg =>
{
    cfg.AddMaps(typeof(CourseProfile).Assembly);
    cfg.AddMaps(typeof(QuestionsProfile).Assembly);
});

// Replace AddOpenApi() with AddSwaggerGen and an OpenAPI document
builder.Services.AddEndpointsApiExplorer();

#region Swagger Config

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
            new string[] { }
        }
    });


    try
    {
        var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
        var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
        if (File.Exists(xmlPath)) c.IncludeXmlComments(xmlPath);
    }
    catch
    {
        // Ignore if XML comments aren't available
    }
});

#endregion


builder.Services.AddControllers();
// Scalar API Reference Configuration with Bearer Security Scheme
builder.Services.AddOpenApi("v1", options => { options.AddDocumentTransformer<BearerSecuritySchemeTransformer>(); });


// builder.Services.AddResultExceptionHandler();

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
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "E-Learning API V1");
    c.RoutePrefix = string.Empty; //"swagger"; // This makes it available at /swagger

    if (!app.Environment.IsDevelopment()) c.DocumentTitle = "API Documentation - Production";
});

#endregion

// Scalar API Reference Middleware
app.MapOpenApi();
app.MapScalarApiReference(options =>
{
    options.Title = "E-Learning API Reference";
    options.Theme = ScalarTheme.BluePlanet;
    options.DefaultHttpClient =
        new KeyValuePair<ScalarTarget, ScalarClient>(ScalarTarget.CSharp, ScalarClient.HttpClient);
    options.HideDarkModeToggle = true;
    options.ShowSidebar = true;
    options.Favicon = "/favicon.ico"; //add image
});

app.UseExceptionHandler();
app.test();
app.UseResponseCaching();
app.UseStaticFiles();
app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();