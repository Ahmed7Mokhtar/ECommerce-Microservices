using FluentValidation;
using OrdersService.BusinessLogicLayer.DTOs;

namespace OrdersService.BusinessLogicLayer.Validators
{
    public class UpdateOrderDTOValidator : AbstractValidator<UpdateOrderDTO>
    {
        public UpdateOrderDTOValidator()
        {
            RuleFor(dto => dto.OrderId)
                .NotEmpty().WithMessage("Order Id can't be blanked!");

            RuleFor(dto => dto.UserId)
                .NotEmpty().WithMessage("User Id can't be blanked!");

            RuleFor(dto => dto.OrderDate)
                .NotEmpty().WithMessage("Order Date can't be blanked!");

            RuleFor(dto => dto.Items)
                .NotEmpty().WithMessage("Order Items can't be blanked!");
        }
    }

    public class UpdateOrderItemDTOValidator : AbstractValidator<UpdateOrderItemDTO>
    {
        public UpdateOrderItemDTOValidator()
        {
            RuleFor(dto => dto.ProductId)
                .NotEmpty().WithMessage("Product Id can't be blanked!");

            RuleFor(dto => dto.UnitPrice)
                .NotEmpty().WithMessage("Unit Price can't be blanked!")
                .GreaterThan(0).WithMessage("Unit Price must be greater than zero!");

            RuleFor(dto => dto.Quantity)
                .NotEmpty().WithMessage("Quantity can't be blanked!")
                .GreaterThan(0).WithMessage("Quantity must be greater than zero!");
        }
    }
}
