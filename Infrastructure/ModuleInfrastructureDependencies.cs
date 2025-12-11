using Application.Common.Interfaces;
using Core.Interfaces;
using Core.Interfaces.Services;
using Infrastructure.Common;
using Infrastructure.Common.GenRepo;
using Infrastructure.Interface;
using Infrastructure.services;
using Infrastructure.Services;
using Infrastructure.ZoomServices;
using Microsoft.Extensions.Caching.Hybrid;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure;

public static class ModuleInfrastructureDependencies
{
    public static IServiceCollection AddInfrastructureDependencies(this IServiceCollection services)
    {
        services.AddHttpClient();

        services.AddScoped<IUserContextService, UserContextService>();
        services.AddScoped<ITokenService, TokenService>();
        services.AddScoped<IRefreshTokenRepository, RefreshTokenRepository>();

        services.AddScoped<IZoomService, ZoomService>();
        services.AddScoped<IZoomAuthService, ZoomAuthService>();
        services.AddScoped<IZoomMeetingRepository, ZoomMeetingRepository>();

        services.AddScoped<IStudentRepository, StudentRepository>();
        services.AddScoped<IStudentAnswerRepository, StudentAnswerRepository>();

        services.AddScoped<ICourseRepository, CourseRepository>();
        services.AddScoped<IEnrollmentRepository, EnrollmentRepository>();
        services.AddScoped<ILectureRespository, LectureRespository>();

        services.AddScoped<IExamRepository, ExamRepository>();
        services.AddScoped<IExamResultRepository, ExamResultRepository>();
        services.AddScoped<IAnswerOptionRepository, AnswerOptionRepository>();

        services.AddScoped<IFileStorageService, FileStorageService>();
        services.AddScoped<IFawaterakPaymentService, FawaterakPaymentService>();





        services.AddHybridCache(options => options.DefaultEntryOptions = new HybridCacheEntryOptions
        {
            Expiration = TimeSpan.FromMinutes(10), // Distributed( L2, L3)
            LocalCacheExpiration = TimeSpan.FromSeconds(30), // Local Memory L1
        });

     
        services.AddScoped<IUnitOfWork, UnitOfWork>();
        services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));

        return services;
    }
}