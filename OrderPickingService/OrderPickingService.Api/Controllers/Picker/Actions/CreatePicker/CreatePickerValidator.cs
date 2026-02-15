using FluentValidation;
using OrderPickingService.Services.Picker.Dtos;

namespace OrderPickingService.Api.Controllers.Picker.Actions;

public sealed class CreatePickerValidator : AbstractValidator<CreatePickerDto>
{
    public CreatePickerValidator()
    {
        RuleFor(picker => picker.FirstName).NotEmpty();
        RuleFor(picker => picker.LastName).NotEmpty();
    }
}