using FluentValidation;
using Mango.UserService.Application.DTOs;

namespace Mango.UserService.Application.Validators;

public class RegisterRequestValidator : AbstractValidator<RegisterRequest>
{
  public RegisterRequestValidator()
  {
    RuleFor(x => x.FullName)
        .NotEmpty().WithMessage("Full name is required.")
        .MaximumLength(100);

    RuleFor(x => x.Email)
        .NotEmpty().WithMessage("Email is required.")
        .EmailAddress().WithMessage("Invalid email format.");

    RuleFor(x => x.Password)
        .NotEmpty().WithMessage("Password is required.")
        .MinimumLength(6).WithMessage("Password must be at least 6 characters.");

    RuleFor(x => x.Role)
        .NotEmpty().WithMessage("Role is required.")
        .Must(BeAValidRole)
        .WithMessage("Role must be one of: Farmer, Reseller, Wholesaler.");
  }

  private bool BeAValidRole(string role)
  {
    return new[] { "Farmer", "Reseller", "Wholesaler" }.Contains(role, StringComparer.OrdinalIgnoreCase);
  }
}
