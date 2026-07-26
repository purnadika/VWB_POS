namespace POS.Domain.Enums;

/// <summary>
/// Defines all permission types that can be assigned to users.
/// These are constants to avoid magic strings throughout the application.
/// </summary>
public enum PermissionType
{
    // User Management Permissions
    CreateUser = 1,
    EditUser = 2,
    DeleteUser = 3,
    ViewUserList = 4,
    ManageRoles = 5,

    // Sales Permissions
    ProcessSales = 10,
    ProcessRefunds = 11,
    ProcessPayments = 12,
    ViewSalesReport = 13,

    // Inventory Permissions
    ManageInventory = 20,
    ViewInventory = 21,
    ReceiveStock = 22,
    AdjustStock = 23,

    // Reporting Permissions
    ViewReports = 30,
    ExportReports = 31,
    ViewAuditLog = 32,

    // Configuration Permissions
    ManageConfiguration = 40,
    ManageTaxes = 41,
    ManageProducts = 42
}
