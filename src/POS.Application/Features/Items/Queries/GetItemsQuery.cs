using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using POS.Domain.Common;
using POS.Domain.Entities;
using POS.Domain.Interfaces.Repositories;
using System.Linq;

namespace POS.Application.Features.Items.Queries;

public record GetItemsQuery() : IRequest<Result<List<Item>>>;

public class GetItemsQueryHandler : IRequestHandler<GetItemsQuery, Result<List<Item>>>
{
    private readonly IRepository<Item> _repository;

    public GetItemsQueryHandler(IRepository<Item> repository)
    {
        _repository = repository;
    }

    public async Task<Result<List<Item>>> Handle(GetItemsQuery request, CancellationToken cancellationToken)
    {
        var items = await _repository.GetAllAsync(cancellationToken);
        return Result.Success(items.Where(i => !i.Deleted).ToList());
    }
}
