using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using POS.Domain.Entities;
using POS.Infrastructure.Persistence;

namespace POS.WebAPI.Controllers;

[ApiController]
[Route("api/expense-categories")]
public class ExpenseCategoriesController : ControllerBase
{
    private readonly POSDbContext _dbContext;

    public ExpenseCategoriesController(POSDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    [HttpGet]
    public async Task<IActionResult> Get()
    {
        var categories = await _dbContext.ExpenseCategories
            .Where(x => !x.Deleted)
            .OrderBy(x => x.Name)
            .Select(x => new { id = x.Id, categoryName = x.Name, categoryDescription = x.Description })
            .ToListAsync();
            
        return Ok(categories);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] ExpenseCategory dto)
    {
        _dbContext.ExpenseCategories.Add(dto);
        await _dbContext.SaveChangesAsync();
        return Created($"/api/expense-categories/{dto.Id}", dto);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, [FromBody] ExpenseCategory dto)
    {
        var existing = await _dbContext.ExpenseCategories.FindAsync(id);
        if (existing == null || existing.Deleted) return NotFound();

        existing.Name = dto.Name;
        existing.Description = dto.Description;
        
        await _dbContext.SaveChangesAsync();
        return Ok(existing);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var existing = await _dbContext.ExpenseCategories.FindAsync(id);
        if (existing == null) return NotFound();

        existing.Deleted = true;
        await _dbContext.SaveChangesAsync();
        return Ok();
    }
}
