using POS.Domain.Common;

namespace POS.Domain.Entities;

public class DinnerTable : BaseEntity
{
    public string Name { get; set; } = string.Empty;
    public int Status { get; set; } = 0; // 0 = Free, 1 = Occupied

}

