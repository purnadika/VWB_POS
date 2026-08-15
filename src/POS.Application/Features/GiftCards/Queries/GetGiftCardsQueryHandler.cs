using System.Threading;
using System.Threading.Tasks;
using MediatR;
using POS.Domain.Common;
using POS.Domain.Entities;
using POS.Domain.Interfaces.Repositories;
using POS.Application.Features.GiftCards.DTOs;
using System.Linq;

namespace POS.Application.Features.GiftCards.Queries;

public class GetGiftCardsQueryHandler : IRequestHandler<GetGiftCardsQuery, Result<System.Collections.Generic.List<GiftcardDto>>>
{
    private readonly IRepository<Giftcard> _repository;

    public GetGiftCardsQueryHandler(IRepository<Giftcard> repository)
    {
        _repository = repository;
    }

    public async Task<Result<System.Collections.Generic.List<GiftcardDto>>> Handle(GetGiftCardsQuery request, CancellationToken cancellationToken)
    {
        var entities = await _repository.GetAllAsync(cancellationToken);

        var dtos = entities.Select(e => new GiftcardDto 
        { 
            Id = e.Id,
            GiftcardNumber = e.GiftcardNumber,
            Value = e.Value,
            CustomerId = e.CustomerId
        }).ToList();
        
        return Result.Success(dtos);
    }
}

