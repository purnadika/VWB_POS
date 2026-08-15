using System.Threading;
using System.Threading.Tasks;
using MediatR;
using POS.Domain.Common;
using POS.Domain.Entities;
using POS.Domain.Interfaces.Repositories;
using POS.Application.Features.Suppliers.DTOs;
using System.Linq;

namespace POS.Application.Features.Suppliers.Queries;

public class GetSuppliersQueryHandler : IRequestHandler<GetSuppliersQuery, Result<System.Collections.Generic.List<SupplierDto>>>
{
    private readonly IRepository<Supplier> _repository;

    public GetSuppliersQueryHandler(IRepository<Supplier> repository)
    {
        _repository = repository;
    }

    public async Task<Result<System.Collections.Generic.List<SupplierDto>>> Handle(GetSuppliersQuery request, CancellationToken cancellationToken)
    {
        var entities = await _repository.GetAllAsync(cancellationToken);

        var dtos = entities.Select(e => new SupplierDto 
        { 
            Id = e.Id,
            FirstName = e.FirstName,
            LastName = e.LastName,
            Email = e.Email,
            PhoneNumber = e.PhoneNumber,
            CompanyName = e.CompanyName,
            AccountNumber = e.AccountNumber,
            AgencyName = e.AgencyName
        }).ToList();
        
        return Result.Success(dtos);
    }
}

