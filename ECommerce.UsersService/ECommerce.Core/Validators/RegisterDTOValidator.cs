using ECommerce.Core.DTOs;
using ECommerce.Core.Enums;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Core.Validators
{
    public class RegisterDTOValidator : AbstractValidator<RegisterDTO>
    {
        public RegisterDTOValidator()
        {
            RuleFor(dto => dto.Name)
                .NotEmpty().WithMessage("Name is required")
                .MaximumLength(100).WithMessage("Name must not exceed 100 characters");
            RuleFor(dto => dto.Email)
                .NotEmpty().WithMessage("Email is required")
                .EmailAddress().WithMessage("Invalid email address")
                .MaximumLength(100).WithMessage("Email must not exceed 100 characters");
            RuleFor(dto => dto.Password)
                .NotEmpty().WithMessage("Password is required")
                .MaximumLength(100).WithMessage("Password must not exceed 100 characters");
            RuleFor(dto => dto.Gender)
                .NotEmpty().WithMessage("Gender is required")
                .Must(gender => Enum.IsDefined(typeof(Gender), gender))
                .WithMessage("Invalid Gender value");
        }
    }
}
