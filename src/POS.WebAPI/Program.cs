using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using POS.Application;
using POS.Domain.Entities;
using POS.Infrastructure;
using POS.Infrastructure.Persistence;

AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);
var builder = WebApplication.CreateBuilder(args);

// Add layers
builder.Services.AddApplicationServices();
builder.Services.AddInfrastructureServices(builder.Configuration);

builder.Services.AddHttpContextAccessor();
builder.Services.AddScoped<POS.Application.Services.ICurrentUserService, POS.WebAPI.Services.CurrentUserService>();
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddOpenApi();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseDefaultFiles();
app.UseStaticFiles();

app.UseAuthorization();
app.MapControllers();

// Seed Database
using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<POSDbContext>();
    await SeedDataAsync(context);
}

app.Run();

async Task SeedDataAsync(POSDbContext context)
{
    // Auto apply migrations or ensure created
    await context.Database.EnsureCreatedAsync();

    if (!context.Employees.Any())
    {
        // Add default employee / admin
        var admin = new Employee
        {
            FirstName = "Admin",
            LastName = "User",
            Username = "admin",
            Email = "admin@example.com",
            PasswordHash = BCrypt.Net.BCrypt.HashPassword("password123"),
            GrantedModules = new List<string> { "items", "sales", "customers", "reports", "config" }
        };
        await context.Employees.AddAsync(admin);

        // Add a default tax category and rates
        var vatCategory = new TaxCategory { Name = "Standard VAT" };
        var standardTax = new TaxRate { Name = "VAT 10%", Rate = 10.00m, TaxCategory = vatCategory };
        vatCategory.TaxRates.Add(standardTax);
        await context.TaxCategories.AddAsync(vatCategory);

        // Add a stock location
        var storeLocation = new StockLocation { LocationName = "Main Store" };
        await context.StockLocations.AddAsync(storeLocation);

        // Add a default item category
        var electronicsCategory = new ItemCategory { Name = "Electronics", Description = "Electronic devices and accessories" };
        await context.ItemCategories.AddAsync(electronicsCategory);

        // Add some default items
        var item1 = new Item
        {
            Name = "Wireless Mouse",
            Category = electronicsCategory,
            ItemNumber = "SKU-MOUSE-100",
            Description = "Ergonomic 2.4GHz wireless mouse.",
            CostPrice = 10.00m,
            UnitPrice = 25.00m,
            ReorderLevel = 5,
            ReceivingQuantity = 10,
            TaxCategory = vatCategory
        };
        item1.ItemQuantities.Add(new ItemQuantity { Location = storeLocation, Quantity = 20 });

        var item2 = new Item
        {
            Name = "Mechanical Keyboard",
            Category = electronicsCategory,
            ItemNumber = "SKU-KEYBOARD-200",
            Description = "RGB Backlit Mechanical Keyboard.",
            CostPrice = 30.00m,
            UnitPrice = 75.00m,
            ReorderLevel = 3,
            ReceivingQuantity = 5,
            TaxCategory = vatCategory
        };
        item2.ItemQuantities.Add(new ItemQuantity { Location = storeLocation, Quantity = 10 });

        await context.Items.AddAsync(item1);
        await context.Items.AddAsync(item2);
    }
    else
    {
        // Update existing admin just in case it was seeded with plain text password and no email
        var adminEmployee = context.Employees.FirstOrDefault(e => e.Username == "admin");
        if (adminEmployee != null)
        {
            adminEmployee.Email = "admin@example.com";
            // Check if it's not already hashed by BCrypt (which starts with $2)
            if (!adminEmployee.PasswordHash.StartsWith("$2"))
            {
                adminEmployee.PasswordHash = BCrypt.Net.BCrypt.HashPassword("password123");
            }
        }
    }

    if (!context.Users.Any())
    {
        var adminUser = POS.Domain.Aggregates.UserManagement.User.Create(
            email: "admin@example.com",
            passwordHash: BCrypt.Net.BCrypt.HashPassword("password123"),
            role: POS.Domain.Enums.UserRole.Administrator,
            fullName: "Admin User",
            phoneNumber: "1234567890",
            createdBy: "SYSTEM"
        );
        await context.Users.AddAsync(adminUser);
    }
    else
    {
        // Use AsEnumerable() since Email is a Value Object not translateable by EF
        var adminUser = context.Users.AsEnumerable().FirstOrDefault(u => u.Email.Value == "admin@example.com");
        if (adminUser != null && !adminUser.PasswordHash.Hash.StartsWith("$2"))
        {
            // Password is not BCrypt-hashed — can't update Value Object directly without domain method.
        }
    }

    // Seed locale/currency/language settings into AppConfig
    var localeKeys = new[] { "locale", "currency", "language" };
    var defaultValues = new Dictionary<string, string>
    {
        { "locale", "id-ID" },
        { "currency", "IDR" },
        { "language", "id" },
    };
    foreach (var key in localeKeys)
    {
        if (!context.AppConfigs.Any(c => c.Key == key))
        {
            await context.AppConfigs.AddAsync(new POS.Domain.Entities.AppConfig
            {
                Key = key,
                Value = defaultValues[key]
            });
        }
    }

    // Save seeds
    await context.SaveChangesAsync();
}
