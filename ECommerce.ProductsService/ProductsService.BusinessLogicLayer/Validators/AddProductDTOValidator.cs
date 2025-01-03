using FluentValidation;
using ProductsService.BusinessLogicLayer.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductsService.BusinessLogicLayer.Validators
{
    public class AddProductDTOValidator : AbstractValidator<AddProductDTO>
    {
        public AddProductDTOValidator()
        {
            RuleFor(dto => dto.ProductName)
                .NotEmpty().WithMessage("Product Name is required!");

            RuleFor(dto => dto.Category)
                .IsInEnum().WithMessage("Invalid Product Category");

            RuleFor(dto => dto.UnitPrice)
                .InclusiveBetween(0, decimal.MaxValue).WithMessage($"Unit Price Must be betweeb 0 and {decimal.MaxValue}");

            RuleFor(dto => dto.QuantityInStock)
                .InclusiveBetween(0, int.MaxValue).WithMessage($"Quantity in Stock Must be betweeb 0 and {int.MaxValue}");
        }
    }
}
