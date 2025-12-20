using FluentValidation;
using Mango.UserService.Application.DTOs;
using Mango.UserService.Domain.Enums;

namespace Mango.UserService.Application.Validators;

public class RegisterRequestValidator : AbstractValidator<RegisterRequest>
{
  public RegisterRequestValidator()
  {
    RuleFor(x => x.FirstName)
        .NotEmpty().WithMessage("First name is required.")
        .MaximumLength(50).WithMessage("First name must not exceed 50 characters.");

    RuleFor(x => x.LastName)
        .NotEmpty().WithMessage("Last name is required.")
        .MaximumLength(50).WithMessage("Last name must not exceed 50 characters.");

    RuleFor(x => x.Email)
        .NotEmpty().WithMessage("Email is required.")
        .EmailAddress().WithMessage("Invalid email format.");

    RuleFor(x => x.Password)
        .NotEmpty().WithMessage("Password is required.")
        .MinimumLength(8).WithMessage("Password must be at least 8 characters.");

    RuleFor(x => x.Role)
        .Must(BeAValidRole)
        .When(x => !string.IsNullOrWhiteSpace(x.Role))
        .WithMessage($"Role must be one of: {string.Join(", ", Enum.GetNames<UserRole>())}.");
  }

  private bool BeAValidRole(string? role)
  {
    if (string.IsNullOrWhiteSpace(role))
      return true; // Role is optional
    return Enum.GetNames<UserRole>().Contains(role, StringComparer.OrdinalIgnoreCase);
  }
}
