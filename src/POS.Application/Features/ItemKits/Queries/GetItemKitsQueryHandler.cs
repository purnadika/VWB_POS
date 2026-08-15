using System.Threading;
using System.Threading.Tasks;
using MediatR;
using POS.Domain.Common;
using POS.Domain.Entities;
using POS.Domain.Interfaces.Repositories;
using POS.Application.Features.ItemKits.DTOs;
using System.Linq;

namespace POS.Application.Features.ItemKits.Queries;

public class GetItemKitsQueryHandler : IRequestHandler<GetItemKitsQuery, Result<System.Collections.Generic.List<ItemKitDto>>>
{
    private readonly IRepository<ItemKit> _repository;

    public GetItemKitsQueryHandler(IRepository<ItemKit> repository)
    {
        _repository = repository;
    }

    public async Task<Result<System.Collections.Generic.List<ItemKitDto>>> Handle(GetItemKitsQuery request, CancellationToken cancellationToken)
    {
        var entities = await _repository.GetAllAsync(cancellationToken);

        var dtos = entities.Select(e => new ItemKitDto 
        { 
            Id = e.Id,
            Name = e.Name,
            ItemKitNumber = e.ItemKitNumber,
            Description = e.Description,
            UnitPrice = e.UnitPrice,
            CostPrice = e.CostPrice
        }).ToList();
        
        return Result.Success(dtos);
    }
}

