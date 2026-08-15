using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using POS.Infrastructure.Persistence;
using POS.Domain.Entities;

namespace POS.WebAPI.Controllers;

[ApiController]
[Route("api/seed")]
public class SeedController : ControllerBase
{
    private readonly POSDbContext _context;

    public SeedController(POSDbContext context)
    {
        _context = context;
    }

    [HttpPost]
    public async Task<IActionResult> Seed()
    {
        await _context.Database.ExecuteSqlRawAsync("TRUNCATE TABLE \"People\" CASCADE;");
        await _context.Database.ExecuteSqlRawAsync("TRUNCATE TABLE \"ItemCategories\" CASCADE;");
        await _context.Database.ExecuteSqlRawAsync("TRUNCATE TABLE \"TaxCategories\" CASCADE;");
        await _context.Database.ExecuteSqlRawAsync("TRUNCATE TABLE \"ExpenseCategories\" CASCADE;");
        await _context.Database.ExecuteSqlRawAsync("ALTER SEQUENCE \"People_Id_seq\" RESTART WITH 1;");
        await _context.Database.ExecuteSqlRawAsync("ALTER SEQUENCE \"ItemCategories_Id_seq\" RESTART WITH 1;");
        await _context.Database.ExecuteSqlRawAsync("ALTER SEQUENCE \"TaxCategories_Id_seq\" RESTART WITH 1;");
        await _context.Database.ExecuteSqlRawAsync("ALTER SEQUENCE \"ExpenseCategories_Id_seq\" RESTART WITH 1;");

        _context.Customers.Add(new Customer { FirstName = "Seed", LastName = "Customer" });
        _context.Employees.Add(new Employee { FirstName = "Seed", LastName = "Employee" });
        _context.Employees.Add(new Employee { FirstName = "Seed", LastName = "Employee2" });
        _context.Suppliers.Add(new Supplier { FirstName = "Seed", LastName = "Supplier" });
        
        _context.ItemCategories.Add(new ItemCategory { Name = "Seed Item Category" });
        _context.TaxCategories.Add(new TaxCategory { Name = "Seed Tax Category" });
        _context.ExpenseCategories.Add(new ExpenseCategory { Name = "Seed Expense Category" });

        await _context.SaveChangesAsync();
        return Ok("Database Seeded!");
    }
}
