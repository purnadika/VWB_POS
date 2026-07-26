namespace POS.Domain.Enums;

/// <summary>
/// Defines all user roles in the POS system.
/// These are constants to avoid magic strings throughout the application.
/// </summary>
public enum UserRole
{
    Administrator = 1,
    Manager = 2,
    Cashier = 3,
    WarehouseStaff = 4,
    Supervisor = 5
}
