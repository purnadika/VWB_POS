using System.Collections.Generic;
using POS.Domain.Common;

namespace POS.Domain.Entities;

public class TaxCategory : BaseEntity
{
    public string Name { get; set; } = string.Empty;

    public List<TaxRate> TaxRates { get; set; } = new();
}

