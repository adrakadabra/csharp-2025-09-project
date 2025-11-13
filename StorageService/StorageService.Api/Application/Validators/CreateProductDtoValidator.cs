using FluentValidation;
using StorageService.Api.Application.DTOs;

namespace StorageService.Api.Application.Validators;

public class CreateProductDtoValidator : AbstractValidator<CreateProductDto>
{
    public CreateProductDtoValidator()
    {
        RuleFor(x => x.Name).NotEmpty().Length(1, 200);
        RuleFor(x => x.Quantity).GreaterThanOrEqualTo(0);
    }
}
