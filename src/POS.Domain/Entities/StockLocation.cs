using POS.Domain.Common;

namespace POS.Domain.Entities;

public class StockLocation : BaseEntity
{
    public string LocationName { get; set; } = string.Empty;

}

