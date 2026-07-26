namespace POS.Application.Constants;

/// <summary>
/// All localization keys used in the application.
/// NO MAGIC STRINGS - all user-facing text is referenced by constant keys.
/// These keys are looked up via ILocalizationService to get translated strings.
/// </summary>
public static class LocalizationKeys
{
    // =============== User Management ===============
    public const string UserCreatedSuccessfully = "user.created.successfully";
    public const string UserUpdatedSuccessfully = "user.updated.successfully";
    public const string UserDeactivatedSuccessfully = "user.deactivated.successfully";
    public const string UserActivatedSuccessfully = "user.activated.successfully";
    public const string UserDeletedSuccessfully = "user.deleted.successfully";
    public const string RoleAssignedSuccessfully = "user.role.assigned.successfully";
    public const string PermissionGrantedSuccessfully = "user.permission.granted.successfully";
    public const string PermissionRevokedSuccessfully = "user.permission.revoked.successfully";

    // =============== Error Messages ===============
    public const string UserNotFound = "error.user.not.found";
    public const string EmailAlreadyExists = "error.email.already.exists";
    public const string InvalidEmailFormat = "error.invalid.email.format";
    public const string InvalidPassword = "error.invalid.password";
    public const string UserDeactivated = "error.user.deactivated";
    public const string UnauthorizedAccess = "error.unauthorized.access";
    public const string InsufficientPermissions = "error.insufficient.permissions";
    public const string InvalidCredentials = "error.invalid.credentials";
    public const string OperationFailed = "error.operation.failed";
    public const string DatabaseError = "error.database.error";
    public const string ValidationError = "error.validation.error";

    // =============== Validation Messages ===============
    public const string EmailRequired = "validation.email.required";
    public const string EmailInvalid = "validation.email.invalid";
    public const string PasswordRequired = "validation.password.required";
    public const string PasswordTooShort = "validation.password.too.short";
    public const string PasswordMustContainSpecialChar = "validation.password.special.char";
    public const string PasswordMustContainUppercase = "validation.password.uppercase";
    public const string PasswordMustContainNumber = "validation.password.number";
    public const string FullNameRequired = "validation.fullname.required";
    public const string FullNameTooShort = "validation.fullname.too.short";
    public const string FullNameTooLong = "validation.fullname.too.long";
    public const string RoleRequired = "validation.role.required";
    public const string InvalidRole = "validation.invalid.role";
    public const string PhoneNumberInvalid = "validation.phone.invalid";
    public const string UserIdRequired = "validation.userid.required";
    public const string PermissionRequired = "validation.permission.required";

    // =============== Generic ===============
    public const string OperationSuccessful = "operation.successful";
    public const string PleaseCheckYourInput = "please.check.your.input";
    public const string ConfirmDeleteTitle = "confirm.delete.title";
    public const string ConfirmDeleteMessage = "confirm.delete.message";
    public const string Yes = "button.yes";
    public const string No = "button.no";
    public const string Cancel = "button.cancel";
    public const string Save = "button.save";
    public const string Delete = "button.delete";
    public const string Edit = "button.edit";
    public const string Close = "button.close";
    public const string Loading = "loading";
    public const string NoDataAvailable = "no.data.available";
}
