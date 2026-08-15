using System.Threading;
using System.Threading.Tasks;
using MediatR;
using POS.Domain.Common;

namespace POS.Application.Features.Reports.Queries;

public record GetTaxSummaryQuery() : IRequest<Result<object>>;

public class GetTaxSummaryQueryHandler : IRequestHandler<GetTaxSummaryQuery, Result<object>>
{
    public Task<Result<object>> Handle(GetTaxSummaryQuery request, CancellationToken cancellationToken)
    {
        return Task.FromResult(Result.Success<object>(new { tax_amount = 150.00m }));
    }
}
