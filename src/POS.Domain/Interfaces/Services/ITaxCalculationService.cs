using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using POS.Domain.Entities;

namespace POS.Domain.Interfaces.Services;

public interface ITaxCalculationService
{
    Task<List<SaleTax>> CalculateTaxesAsync(Sale sale, CancellationToken cancellationToken = default);
}
