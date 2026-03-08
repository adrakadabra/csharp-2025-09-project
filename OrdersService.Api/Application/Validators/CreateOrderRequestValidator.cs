using FluentValidation;
using OrdersService.Api.Application.DTOs;

namespace OrdersService.Api.Application.Validators;

public class CreateOrderRequestValidator : AbstractValidator<CreateOrderRequest>
{
    public CreateOrderRequestValidator()
    {
        RuleFor(x => x.Items)
            .NotEmpty()
            .WithMessage("Заказ должен содержать хотя бы один товар.");

        RuleForEach(x => x.Items).ChildRules(item =>
        {
            item.RuleFor(i => i.ProductId)
                .NotEmpty()
                .WithMessage("ProductId должен быть заполнен.");

            item.RuleFor(i => i.Quantity)
                .GreaterThan(0)
                .WithMessage("Количество по каждому товару должно быть больше нуля.");
        });
    }
}