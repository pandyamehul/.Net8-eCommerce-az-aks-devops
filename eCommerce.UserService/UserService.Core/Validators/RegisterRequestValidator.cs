using eCommerce.UserService.Core.DTO;
using FluentValidation;

namespace eCommerce.UserService.Core.Validators;

public class RegisterRequestValidator : AbstractValidator<RegisterRequest>
{
    public RegisterRequestValidator()
    {
        // Email
        RuleFor(temp => temp.Email)
            .NotEmpty().WithMessage("Email is required")
            .EmailAddress().WithMessage("Invalid email address format");

        // Password
        RuleFor(temp => temp.PasswordHash)
            .NotEmpty().WithMessage("Password is required")
            .MinimumLength(6).WithMessage("Password must be at least 6 characters long");

        // PersonName
        RuleFor(temp => temp.PersonName)
            .NotEmpty().WithMessage("Person name is required")
            .MaximumLength(100).WithMessage("Person name cannot exceed 100 characters");

        // Gender
        RuleFor(temp => temp.Gender)
            .IsInEnum().WithMessage("Invalid gender value");
    }
}
