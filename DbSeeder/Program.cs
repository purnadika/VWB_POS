using System;
using Microsoft.EntityFrameworkCore;
using POS.Infrastructure.Persistence;

class Program
{
    static async System.Threading.Tasks.Task Main()
    {
        var optionsBuilder = new DbContextOptionsBuilder<POSDbContext>();
        optionsBuilder.UseNpgsql("Host=localhost;Port=5432;Database=vwb_pos;Username=postgres;Password=postgres");
        
        using var context = new POSDbContext(optionsBuilder.Options, null);
        
        await context.Database.ExecuteSqlRawAsync("TRUNCATE TABLE \"People\" CASCADE;");
        await context.Database.ExecuteSqlRawAsync("TRUNCATE TABLE \"ItemCategories\" CASCADE;");
        await context.Database.ExecuteSqlRawAsync("TRUNCATE TABLE \"TaxCategories\" CASCADE;");
        await context.Database.ExecuteSqlRawAsync("TRUNCATE TABLE \"ExpenseCategories\" CASCADE;");
        
        await context.Database.ExecuteSqlRawAsync("INSERT INTO \"People\" (\"Id\", \"FirstName\", \"LastName\", \"Email\", \"PhoneNumber\", \"CreatedAt\", \"Deleted\", \"Address_Street\", \"Address_City\", \"Address_State\", \"Address_ZipCode\", \"Address_Country\", \"Comments\") VALUES (1, 'Seed', 'Customer', 'c@c.com', '123', NOW(), false, '', '', '', '', '', '');");
        await context.Database.ExecuteSqlRawAsync("INSERT INTO \"Customers\" (\"Id\", \"CompanyName\", \"AccountNumber\", \"Taxable\", \"DiscountPercent\", \"RewardPoints\") VALUES (1, '', '', true, 0, 0);");
        
        await context.Database.ExecuteSqlRawAsync("INSERT INTO \"People\" (\"Id\", \"FirstName\", \"LastName\", \"Email\", \"PhoneNumber\", \"CreatedAt\", \"Deleted\", \"Address_Street\", \"Address_City\", \"Address_State\", \"Address_ZipCode\", \"Address_Country\", \"Comments\") VALUES (2, 'Seed', 'Employee', 'e@e.com', '123', NOW(), false, '', '', '', '', '', '');");
        await context.Database.ExecuteSqlRawAsync("INSERT INTO \"Employees\" (\"Id\", \"Role\", \"HireDate\", \"Username\", \"PasswordHash\") VALUES (2, 'Admin', NOW(), '', '');");
        
        await context.Database.ExecuteSqlRawAsync("INSERT INTO \"People\" (\"Id\", \"FirstName\", \"LastName\", \"Email\", \"PhoneNumber\", \"CreatedAt\", \"Deleted\", \"Address_Street\", \"Address_City\", \"Address_State\", \"Address_ZipCode\", \"Address_Country\", \"Comments\") VALUES (3, 'Seed', 'Supplier', 's@s.com', '123', NOW(), false, '', '', '', '', '', '');");
        await context.Database.ExecuteSqlRawAsync("INSERT INTO \"Suppliers\" (\"Id\", \"CompanyName\", \"AccountNumber\", \"AgencyName\") VALUES (3, '', '', '');");
        
        await context.Database.ExecuteSqlRawAsync("INSERT INTO \"ItemCategories\" (\"Id\", \"Name\", \"Description\", \"CreatedAt\", \"Deleted\") VALUES (1, 'Seed Item Category', 'Desc', NOW(), false);");
        await context.Database.ExecuteSqlRawAsync("INSERT INTO \"TaxCategories\" (\"Id\", \"Name\", \"CreatedAt\", \"Deleted\") VALUES (1, 'Seed Tax Category', NOW(), false);");
        await context.Database.ExecuteSqlRawAsync("INSERT INTO \"ExpenseCategories\" (\"Id\", \"Name\", \"CreatedAt\", \"Deleted\") VALUES (1, 'Seed Expense Category', NOW(), false);");
        
        await context.Database.ExecuteSqlRawAsync("SELECT setval('\"People_Id_seq\"', 4);");
        await context.Database.ExecuteSqlRawAsync("SELECT setval('\"ItemCategories_Id_seq\"', 2);");
        await context.Database.ExecuteSqlRawAsync("SELECT setval('\"TaxCategories_Id_seq\"', 2);");
        await context.Database.ExecuteSqlRawAsync("SELECT setval('\"ExpenseCategories_Id_seq\"', 2);");
        
        Console.WriteLine("Done seeding!");
    }
}
