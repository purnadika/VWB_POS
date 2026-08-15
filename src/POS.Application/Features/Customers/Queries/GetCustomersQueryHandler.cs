using System.Threading;
using System.Threading.Tasks;
using MediatR;
using POS.Domain.Common;
using POS.Domain.Entities;
using POS.Domain.Interfaces.Repositories;
using POS.Application.Features.Customers.DTOs;
using System.Linq;

namespace POS.Application.Features.Customers.Queries;

public class GetCustomersQueryHandler : IRequestHandler<GetCustomersQuery, Result<System.Collections.Generic.List<CustomerDto>>>
{
    private readonly ICustomerRepository _repository;

    public GetCustomersQueryHandler(ICustomerRepository repository)
    {
        _repository = repository;
    }

    public async Task<Result<System.Collections.Generic.List<CustomerDto>>> Handle(GetCustomersQuery request, CancellationToken cancellationToken)
    {
        var customers = await _repository.GetAllAsync(cancellationToken);

        var dtos = customers.Select(c => new CustomerDto 
        { 
            Id = c.Id, 
            FirstName = c.FirstName, 
            LastName = c.LastName, 
            Email = c.Email, 
            CompanyName = c.CompanyName 
        }).ToList();
        
        return Result.Success(dtos);
    }
}

