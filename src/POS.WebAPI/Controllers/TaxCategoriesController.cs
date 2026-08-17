using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using POS.Domain.Entities;
using POS.Infrastructure.Persistence;

namespace POS.WebAPI.Controllers;

[ApiController]
[Route("api/tax-categories")]
public class TaxCategoriesController : ControllerBase
{
    private readonly POSDbContext _dbContext;

    public TaxCategoriesController(POSDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    [HttpGet]
    public async Task<IActionResult> Get()
    {
        var categories = await _dbContext.TaxCategories
            .Where(x => !x.Deleted)
            .OrderBy(x => x.Name)
            .Select(x => new { id = x.Id, taxCategoryName = x.Name })
            .ToListAsync();
            
        return Ok(categories);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] TaxCategory dto)
    {
        _dbContext.TaxCategories.Add(dto);
        await _dbContext.SaveChangesAsync();
        return Created($"/api/tax-categories/{dto.Id}", dto);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, [FromBody] TaxCategory dto)
    {
        var existing = await _dbContext.TaxCategories.FindAsync(id);
        if (existing == null || existing.Deleted) return NotFound();

        existing.Name = dto.Name;
        
        await _dbContext.SaveChangesAsync();
        return Ok(existing);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var existing = await _dbContext.TaxCategories.FindAsync(id);
        if (existing == null) return NotFound();

        existing.Deleted = true;
        await _dbContext.SaveChangesAsync();
        return Ok();
    }
}
