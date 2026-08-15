using System.Threading;
using System.Threading.Tasks;
using MediatR;
using POS.Domain.Common;
using POS.Domain.Entities;
using POS.Domain.Interfaces.Repositories;
using POS.Application.Features.Taxes.DTOs;
using System.Linq;

namespace POS.Application.Features.Taxes.Queries;

public class GetTaxesQueryHandler : IRequestHandler<GetTaxesQuery, Result<System.Collections.Generic.List<TaxRateDto>>>
{
    private readonly IRepository<TaxRate> _repository;

    public GetTaxesQueryHandler(IRepository<TaxRate> repository)
    {
        _repository = repository;
    }

    public async Task<Result<System.Collections.Generic.List<TaxRateDto>>> Handle(GetTaxesQuery request, CancellationToken cancellationToken)
    {
        var entities = await _repository.GetAllAsync(cancellationToken);

        var dtos = entities.Select(e => new TaxRateDto 
        { 
            Id = e.Id,
            Name = e.Name,
            Rate = e.Rate,
            TaxCategoryId = e.TaxCategoryId
        }).ToList();
        
        return Result.Success(dtos);
    }
}

