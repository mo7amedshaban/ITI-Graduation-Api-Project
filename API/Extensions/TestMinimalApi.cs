using System.Diagnostics;
using Application.Features.Courses.DTOs;
using Application.Features.Courses.Queries.GetAllCourses;
using Application.Features.Exam.Commands.Questions.UpdateQuestion;
using Application.Features.Exam.DTOs;
using Application.Features.Exam.Queries;
using Core.Common.Results;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Caching.Memory;

using System.Text.Json;

namespace API.Extensions;

public static class TestMinimalApi
{
    public static WebApplication test(this WebApplication app)
    {
        app.MapGet("/test", async ([FromServices]ISender sender,[FromServices]IDistributedCache _cache, [FromServices]ILogger<Program> _logger) =>
        {
            var watch = Stopwatch.StartNew();
            _logger.LogInformation($"Test Start {watch.ElapsedMilliseconds} ms ");
            var cacheKey = "all_Question";
            List<QuestionDto> questions = new List<QuestionDto>();
            string? cachedData =await _cache.GetStringAsync(cacheKey);
            if (!String.IsNullOrEmpty(cachedData))
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