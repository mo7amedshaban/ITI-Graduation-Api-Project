using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.Json;
using System.IO;



namespace Infrastructure.Data;

public class AppDBContextFactory : IDesignTimeDbContextFactory<AppDBContext>
{
    public AppDBContext CreateDbContext(string[] args)
    {
        var basePath = Path.Combine(
            Directory.GetCurrentDirectory(),  // Infrastructure
            "..",                             // فوق
            "API"                             // اسم مشروع الـ API
        );
        // 1) بناء الـ Configuration علشان نقرأ الـ appsettings.json
        var configuration = new ConfigurationBuilder()
            .SetBasePath(basePath)
            .AddJsonFile("appsettings.json", optional: false)
            .Build();
        // 2) إعداد الـ DbContextOptions
        var optionsBuilder = new DbContextOptionsBuilder<AppDBContext>();
        // 3) قراءة ConnectionString من ملف appsettings.json
        var connectionString = configuration.GetConnectionString("DataBase");
        // 4) استخدام SQL Server وحقن الـ ConnectionString
        optionsBuilder.UseSqlServer(connectionString);

        // 5) إنشاء نسخة من AppDBContext وإعطاءها للـ EF Tools
        return new AppDBContext(optionsBuilder.Options);
    }

}