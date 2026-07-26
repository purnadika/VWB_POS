using FluentValidation;
using POS.Application.Constants;

namespace POS.Application.Features.UserManagement.Commands.Validators;

/// <summary>
/// Validator for CreateUserCommand.
/// Enforces all business rules for user creation.
/// NO MAGIC VALUES - all constants used.
/// </summary>
public class CreateUserCommandValidator : AbstractValidator<CreateUserCommand>
{
    public CreateUserCommandValidator()
    {
        // Email validation
        RuleFor(x => x.Email)
            .NotEmpty()
            .WithName("Email")
            .WithErrorCode(LocalizationKeys.EmailRequired);

        RuleFor(x => x.Email)
            .EmailAddress()
            .WithName("Email")
            .WithErrorCode(LocalizationKeys.EmailInvalid)
            .When(x => !string.IsNullOrWhiteSpace(x.Email));

        RuleFor(x => x.Email)
            .MaximumLength(DefaultValues.MaxEmailLength)
            .WithName("Email")
            .When(x => !string.IsNullOrWhiteSpace(x.Email));

        // Password validation
        RuleFor(x => x.Password)
            .NotEmpty()
            .WithName("Password")
            .WithErrorCode(LocalizationKeys.PasswordRequired);

        RuleFor(x => x.Password)
            .MinimumLength(DefaultValues.MinPasswordLength)
            .WithName("Password")
            .WithErrorCode(LocalizationKeys.PasswordTooShort)
            .When(x => !string.IsNullOrWhiteSpace(x.Password));

        RuleFor(x => x.Password)
            .Matches(@"[A-Z]")
            .WithName("Password")
            .WithErrorCode(LocalizationKeys.PasswordMustContainUppercase)
            .When(x => !string.IsNullOrWhiteSpace(x.Password));

        RuleFor(x => x.Password)
            .Matches(@"[0-9]")
            .WithName("Password")
            .WithErrorCode(LocalizationKeys.PasswordMustContainNumber)
            .When(x => !string.IsNullOrWhiteSpace(x.Password));

        RuleFor(x => x.Password)
            .Matches(@"[!@#$%^&*(),.?]")
            .WithName("Password")
            .WithErrorCode(LocalizationKeys.PasswordMustContainSpecialChar)
            .When(x => !string.IsNullOrWhiteSpace(x.Password));

        // Password confirmation
        RuleFor(x => x.ConfirmPassword)
            .Equal(x => x.Password)
            .WithName("ConfirmPassword")
            .WithMessage("Passwords do not match");

        // Full name validation
        RuleFor(x => x.FullName)
            .NotEmpty()
            .WithName("FullName")
            .WithErrorCode(LocalizationKeys.FullNameRequired);

        RuleFor(x => x.FullName)
            .MinimumLength(DefaultValues.MinFullNameLength)
            .WithName("FullName")
            .WithErrorCode(LocalizationKeys.FullNameTooShort)
            .When(x => !string.IsNullOrWhiteSpace(x.FullName));

        RuleFor(x => x.FullName)
            .MaximumLength(DefaultValues.MaxFullNameLength)
            .WithName("FullName")
            .WithErrorCode(LocalizationKeys.FullNameTooLong)
            .When(x => !string.IsNullOrWhiteSpace(x.FullName));

        // Phone number validation (optional but if provided, must be valid)
        RuleFor(x => x.PhoneNumber)
            .Matches(@"^\+?[1-9]\d{1,14}$")
            .WithName("PhoneNumber")
            .WithErrorCode(LocalizationKeys.PhoneNumberInvalid)
            .When(x => !string.IsNullOrWhiteSpace(x.PhoneNumber));

        // Role validation
        RuleFor(x => x.Role)
            .IsInEnum()
            .WithName("Role")
            .WithErrorCode(LocalizationKeys.InvalidRole);
    }
}

/// <summary>
/// Validator for AssignRoleCommand.
/// </summary>
public class AssignRoleCommandValidator : AbstractValidator<AssignRoleCommand>
{
    public AssignRoleCommandValidator()
    {
        RuleFor(x => x.UserId)
            .NotEmpty()
            .WithName("UserId")
            .WithErrorCode(LocalizationKeys.UserIdRequired);

        RuleFor(x => x.NewRole)
            .IsInEnum()
            .WithName("Role")
            .WithErrorCode(LocalizationKeys.InvalidRole);
    }
}

/// <summary>
/// Validator for GrantPermissionCommand.
/// </summary>
public class GrantPermissionCommandValidator : AbstractValidator<GrantPermissionCommand>
{
    public GrantPermissionCommandValidator()
    {
        RuleFor(x => x.UserId)
            .NotEmpty()
            .WithName("UserId")
            .WithErrorCode(LocalizationKeys.UserIdRequired);

        RuleFor(x => x.Permission)
            .IsInEnum()
            .WithName("Permission")
            .WithErrorCode(LocalizationKeys.PermissionRequired);
    }
}

/// <summary>
/// Validator for DeactivateUserCommand.
/// </summary>
public class DeactivateUserCommandValidator : AbstractValidator<DeactivateUserCommand>
{
    public DeactivateUserCommandValidator()
    {
        RuleFor(x => x.UserId)
            .NotEmpty()
            .WithName("UserId")
            .WithErrorCode(LocalizationKeys.UserIdRequired);
    }
}

/// <summary>
/// Validator for UpdateUserProfileCommand.
/// </summary>
public class UpdateUserProfileCommandValidator : AbstractValidator<UpdateUserProfileCommand>
{
    public UpdateUserProfileCommandValidator()
    {
        RuleFor(x => x.UserId)
            .NotEmpty()
            .WithName("UserId")
            .WithErrorCode(LocalizationKeys.UserIdRequired);

        RuleFor(x => x.FullName)
            .NotEmpty()
            .WithName("FullName")
            .WithErrorCode(LocalizationKeys.FullNameRequired);

        RuleFor(x => x.FullName)
            .MinimumLength(DefaultValues.MinFullNameLength)
            .WithName("FullName")
            .WithErrorCode(LocalizationKeys.FullNameTooShort)
            .When(x => !string.IsNullOrWhiteSpace(x.FullName));

        RuleFor(x => x.FullName)
            .MaximumLength(DefaultValues.MaxFullNameLength)
            .WithName("FullName")
            .WithErrorCode(LocalizationKeys.FullNameTooLong)
            .When(x => !string.IsNullOrWhiteSpace(x.FullName));

        RuleFor(x => x.PhoneNumber)
            .Matches(@"^\+?[1-9]\d{1,14}$")
            .WithName("PhoneNumber")
            .WithErrorCode(LocalizationKeys.PhoneNumberInvalid)
            .When(x => !string.IsNullOrWhiteSpace(x.PhoneNumber));
    }
}
