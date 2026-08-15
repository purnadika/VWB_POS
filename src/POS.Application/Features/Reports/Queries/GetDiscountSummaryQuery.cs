using System.Threading;
using System.Threading.Tasks;
using MediatR;
using POS.Domain.Common;

namespace POS.Application.Features.Reports.Queries;

public record GetDiscountSummaryQuery() : IRequest<Result<object>>;

public class GetDiscountSummaryQueryHandler : IRequestHandler<GetDiscountSummaryQuery, Result<object>>
{
    public Task<Result<object>> Handle(GetDiscountSummaryQuery request, CancellationToken cancellationToken)
    {
        return Task.FromResult(Result.Success<object>(new { discount_amount = 50.00m }));
    }
}
