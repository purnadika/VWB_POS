using System.Threading;
using System.Threading.Tasks;
using MediatR;
using POS.Domain.Common;
using POS.Domain.Entities;
using POS.Domain.Interfaces.Repositories;
using POS.Application.Features.Receivings.DTOs;
using System.Linq;

namespace POS.Application.Features.Receivings.Queries;

public class GetReceivingsQueryHandler : IRequestHandler<GetReceivingsQuery, Result<System.Collections.Generic.List<ReceivingDto>>>
{
    private readonly IRepository<Receiving> _repository;

    public GetReceivingsQueryHandler(IRepository<Receiving> repository)
    {
        _repository = repository;
    }

    public async Task<Result<System.Collections.Generic.List<ReceivingDto>>> Handle(GetReceivingsQuery request, CancellationToken cancellationToken)
    {
        var entities = await _repository.GetAllAsync(cancellationToken);
        var dtos = entities.Select(e => new ReceivingDto 
        { 
            Id = e.Id,
            SupplierId = e.SupplierId,
            EmployeeId = e.EmployeeId,
            ReceivingTime = e.ReceivingTime,
            Comment = e.Comment,
            Reference = e.Reference,
            PaymentType = e.PaymentType,
            Total = e.Total
        }).ToList();
        
        return Result.Success(dtos);
    }
}
