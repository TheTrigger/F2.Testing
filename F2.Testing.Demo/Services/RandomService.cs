using Microsoft.Extensions.Logging;
using System;

namespace F2.Testing.Demo.Services;

/// <summary>
/// Dummy service
/// </summary>
public class RandomService
{
    private readonly Random _random = new Random();
    private readonly ILogger<RandomService> _logger;

    public RandomService(ILogger<RandomService> logger)
    {
        _logger = logger;
    }

    public int GetRandomNumber()
    {
        var number = _random.Next();

        _logger.LogInformation($"{nameof(RandomService)} generated {number} value");

        return number;
    }
}