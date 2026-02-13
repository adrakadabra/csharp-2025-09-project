using FluentValidation.Results;

namespace OrderPickingService.Api.Extensions;

internal static class ValidationExtensions
{
    public static string GetErrors(this ValidationResult validationResult)
    {
        return string.Join(";", validationResult.Errors.Select(x => x.ErrorMessage));
    }
}