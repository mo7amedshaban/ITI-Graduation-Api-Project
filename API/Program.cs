using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.EntityFrameworkCore;
using Infrastructure.Data;
using Microsoft.OpenApi.Models;
using API.Handlers;
using Core.Interfaces;
using Infrastructure.Common;
using Microsoft.AspNetCore.Identity;

var builder = WebApplication.CreateBuilder(args);

var connectionString = builder.Configuration.GetConnectionString("DataBase");
builder.Services.AddDbContext<AppDBContext>(options =>
{
    options.UseSqlServer(connectionString,b => b.MigrationsAssembly(typeof(AppDBContext).Assembly.FullName));
});

builder.Services.AddIdentityApiEndpoints<IdentityUser<Guid>>()
    .AddRoles<IdentityRole<Guid>>()
    .AddEntityFrameworkStores<AppDBContext>()
    .AddDefaultTokenProviders();
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();


// Replace AddOpenApi() with AddSwaggerGen and an OpenAPI document
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "RepositoryPattern UnitOfWork API",
        Version = "v1",
        Description = "API documentation for RepositoryPattern_UnitOfWork"
    });
});

builder.Services.AddControllers(options =>
    {
        // يمكنك إضافة خيارات أخرى هنا
    })
    .ConfigureApiBehaviorOptions(options =>
    {
        // 💥 الخطوة الحاسمة: تعطيل معالجة ModelState غير الصالح تلقائياً.
        // هذا يسمح لـ FluentValidation (من خلال Pipeline Behavior) بإطلاق ValidationException،
        // الذي سيلتقطه الـ ValidationExceptionHandler.
        options.SuppressModelStateInvalidFilter = true;
    })
    .AddFluentValidation(fv =>
    {
        // تسجيل كل الـ Validators في Assembly الذي يحتوي على الكلاس Program
        fv.RegisterValidatorsFromAssemblyContaining<Program>();
        // تعطيل التحقق من صحة Data Annotations (للاحتفاظ بـ FluentValidation فقط)
        fv.DisableDataAnnotationsValidation = true;
    });

builder.Services.AddEndpointsApiExplorer();

builder.Services.AddProblemDetails();
builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "RepositoryPattern UnitOfWork API v1");
});
app.UseExceptionHandler();

app.UseAuthentication();
app.UseAuthorization();


app.UseHttpsRedirection();

app.MapControllers();

app.UseAuthorization();
app.Run();


