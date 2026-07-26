using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using POS.Domain.Entities;
using POS.Domain.Interfaces.Repositories;
using POS.Domain.Interfaces.Services;

namespace POS.Application.Services;

public class TaxCalculationService : ITaxCalculationService
{
    private readonly IRepository<TaxCategory> _taxCategoryRepository;

    public TaxCalculationService(IRepository<TaxCategory> taxCategoryRepository)
    {
        _taxCategoryRepository = taxCategoryRepository;
    }

    public async Task<List<SaleTax>> CalculateTaxesAsync(Sale sale, CancellationToken cancellationToken = default)
    {
        var saleTaxes = new List<SaleTax>();

        foreach (var saleItem in sale.SaleItems)
        {
            if (saleItem.Item == null) continue;

            // If item has a tax category, apply its tax rates
            if (saleItem.Item.TaxCategoryId.HasValue)
            {
                var taxCategory = await _taxCategoryRepository.GetByIdAsync(saleItem.Item.TaxCategoryId.Value, cancellationToken);
                if (taxCategory != null)
                {
                    foreach (var taxRate in taxCategory.TaxRates)
                    {
                        if (taxRate.Deleted) continue;

                        var taxAmount = saleItem.LineTotal * (taxRate.Rate / 100m);

                        saleTaxes.Add(new SaleTax
                        {
                            TaxName = taxRate.Name,
                            Rate = taxRate.Rate,
                            Amount = taxAmount
                        });
                    }
                }
            }
            else
            {
                // Fallback: Default flat 8% sales tax if no category exists
                var defaultTaxAmount = saleItem.LineTotal * 0.08m;
                saleTaxes.Add(new SaleTax
                {
                    TaxName = "Default Sales Tax",
                    Rate = 8.00m,
                    Amount = defaultTaxAmount
                });
            }
        }

        return saleTaxes;
    }
}
