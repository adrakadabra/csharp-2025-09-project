using FluentValidation;

namespace OrderPickingService.Api.Controllers.Picker.Actions.GetPickerById;

public sealed class GetPickerByIdValidator : AbstractValidator<long>
{
    public GetPickerByIdValidator()
    {
        RuleFor(id => id)
            .GreaterThan(0);
    }
}