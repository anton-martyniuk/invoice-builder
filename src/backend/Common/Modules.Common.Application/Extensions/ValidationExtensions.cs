using FluentValidation.Results;
using Microsoft.Extensions.Logging;

namespace Modules.Common.Application.Extensions;

public static class ValidationExtensions
{
    public static List<string> ToFormattedErrorMessages(this ValidationResult validationResult)
    {
        return validationResult.Errors
            .Select(e => $"{e.PropertyName}: {e.ErrorMessage}")
            .ToList();
    }

    public static void LogValidationErrors<T>(this ILogger<T> logger, ValidationResult validationResult, string contextMessage)
    {
        var validationErrors = validationResult.ToFormattedErrorMessages();
        logger.LogWarning("{ContextMessage}: {Errors}", contextMessage, string.Join(", ", validationErrors));
    }
}
