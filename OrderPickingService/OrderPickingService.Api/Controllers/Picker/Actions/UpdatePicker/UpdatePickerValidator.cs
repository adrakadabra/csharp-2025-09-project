using FluentValidation;
using OrderPickingService.Services.Picker.Dtos;

namespace OrderPickingService.Api.Controllers.Picker.Actions.UpdatePicker;

public sealed class UpdatePickerValidator : AbstractValidator<UpdatePickerDto>
{
    public UpdatePickerValidator()
    {
        RuleFor(picker => picker.id).GreaterThan(0);
        RuleFor(picker => picker)
            .Must(HaveAtLeastOneName)
            .WithMessage(picker => "Either FirstName or LastName must be provided");
    }
    
    private bool HaveAtLeastOneName(UpdatePickerDto picker)
    {
        return !string.IsNullOrWhiteSpace(picker.FirstName) 
               || !string.IsNullOrWhiteSpace(picker.LastName);
    }
}