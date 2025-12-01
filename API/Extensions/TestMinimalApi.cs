using System.Diagnostics;
using System.Text.Json;
using Application.Features.Exam.DTOs;
using Application.Features.Exam.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using Scalar.AspNetCore;

namespace API.Extensions;

public static class TestMinimalApi
{
    public static WebApplication test(this WebApplication app)
    {
        app.MapGet("/test",
            async ([FromServices] ISender sender, [FromServices] IDistributedCache _cache,
                [FromServices] ILogger<Program> _logger) =>
            {
                var watch = Stopwatch.StartNew();
                _logger.LogInformation($"Test Start {watch.ElapsedMilliseconds} ms ");
                var cacheKey = "all_Question";
                var questions = new List<QuestionDto>();
                var cachedData = await _cache.GetStringAsync(cacheKey);
                if (!string.IsNullOrEmpty(cachedData))
                {
                    questions = JsonSerializer.Deserialize<List<QuestionDto>>(cachedData)!;
                    _logger.LogInformation($"Data retrieved from cache in {watch.ElapsedMilliseconds} ms ");
                }
                else
                {
                    questions = await sender.Send(new GetAllQuestionQuery());
                    var serializedData = JsonSerializer.Serialize(questions);
                    await _cache.SetStringAsync(cacheKey, serializedData, new DistributedCacheEntryOptions
                    {
                        AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(10),
                        SlidingExpiration = TimeSpan.FromMinutes(2)
                    });
                    _logger.LogInformation($"Data retrieved from database in {watch.ElapsedMilliseconds} ms ");
                }

                return Results.Ok(questions);
            }).WithTags("Shaban");
        return app;
    }
}