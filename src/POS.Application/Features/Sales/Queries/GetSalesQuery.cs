using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using POS.Domain.Common;
using POS.Domain.Interfaces.Repositories;

namespace POS.Application.Features.Sales.Queries;

public record GetSalesQuery() : IRequest<Result<List<SaleSummaryDto>>>;

public record SaleSummaryDto(
    int Id,
    string SaleTime,
    string CustomerName,
    string EmployeeName,
    string Comment
);

public class GetSalesQueryHandler : IRequestHandler<GetSalesQuery, Result<List<SaleSummaryDto>>>
{
    private readonly ISaleRepository _saleRepository;

    public GetSalesQueryHandler(ISaleRepository saleRepository)
    {
        _saleRepository = saleRepository;
    }

    public async Task<Result<List<SaleSummaryDto>>> Handle(GetSalesQuery request, CancellationToken cancellationToken)
    {
        var sales = await _saleRepository.GetAllWithDetailsAsync(cancellationToken);
        
        var dtos = sales.Select(sale => new SaleSummaryDto(
            sale.Id,
            sale.SaleTime.ToString("yyyy-MM-dd HH:mm:ss"),
            sale.Customer?.FullName ?? "Walk-in Customer",
            sale.Employee?.FullName ?? "System",
            sale.Comment
        )).OrderByDescending(x => x.Id).ToList();

        return Result.Success(dtos);
    }
}
