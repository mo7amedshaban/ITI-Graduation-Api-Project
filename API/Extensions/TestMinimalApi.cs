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
using Microsoft.Extensions.Caching.Memory;

namespace API.Extensions;

public static class TestMinimalApi
{
    public static WebApplication test(this WebApplication app)
    {
        app.MapGet("/test", async ([FromServices]ISender sender, [FromServices]ILogger<Program> _logger, [FromServices]IMemoryCache _cache) =>
        {
            var watch = Stopwatch.StartNew();
            _logger.LogInformation($"Test Start {watch.ElapsedMilliseconds} ms ");
            var cacheKey = "all_Question";
            if (!_cache.TryGetValue(cacheKey, out List<QuestionDto> question))
            {
                try
                {
                    var data = await sender.Send(new GetAllQuestionQuery());
                    if (data is not null)
                    {
                        var entryOptions = new MemoryCacheEntryOptions()
                            .SetSlidingExpiration(TimeSpan.FromMinutes(2))
                            .SetPriority(CacheItemPriority.Normal);
                        _cache.Set(cacheKey, data, entryOptions);
                        question = data;
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error retrieving Quesitons");
                }

            }
            watch.Stop();
            _logger.LogInformation($"Test End {watch.ElapsedMilliseconds} ms ");
            return Results.Ok(question??new List<QuestionDto>());

        }).WithTags("Shaban");
        return app;
    }
    
    
    
}