using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using POS.Domain.Entities;
using POS.Domain.Interfaces.Repositories;

namespace POS.Infrastructure.Persistence.Repositories;

public class AppConfigRepository : IAppConfigRepository
{
    private readonly POSDbContext _dbContext;

    public AppConfigRepository(POSDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<string?> GetValueAsync(string key, CancellationToken cancellationToken = default)
    {
        var config = await _dbContext.AppConfigs.FindAsync(new object[] { key }, cancellationToken);
        return config?.Value;
    }

    public async Task<IReadOnlyDictionary<string, string>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        var configs = await _dbContext.AppConfigs.ToListAsync(cancellationToken);
        return configs.ToDictionary(c => c.Key, c => c.Value);
    }

    public async Task SetValueAsync(string key, string value, CancellationToken cancellationToken = default)
    {
        var config = await _dbContext.AppConfigs.FindAsync(new object[] { key }, cancellationToken);
        if (config != null)
        {
            config.Value = value;
        }
        else
        {
            config = new AppConfig { Key = key, Value = value };
            await _dbContext.AppConfigs.AddAsync(config, cancellationToken);
        }
    }

    public async Task DeleteAsync(string key, CancellationToken cancellationToken = default)
    {
        var config = await _dbContext.AppConfigs.FindAsync(new object[] { key }, cancellationToken);
        if (config != null)
        {
            _dbContext.AppConfigs.Remove(config);
        }
    }
}
