using System.Threading;
using System.Threading.Tasks;
using MediatR;
using POS.Domain.Common;
using POS.Domain.Entities;
using POS.Domain.Interfaces.Repositories;
using POS.Application.Features.Employees.DTOs;
using System.Linq;

namespace POS.Application.Features.Employees.Queries;

public class GetEmployeesQueryHandler : IRequestHandler<GetEmployeesQuery, Result<System.Collections.Generic.List<EmployeeDto>>>
{
    private readonly IRepository<Employee> _repository;

    public GetEmployeesQueryHandler(IRepository<Employee> repository)
    {
        _repository = repository;
    }

    public async Task<Result<System.Collections.Generic.List<EmployeeDto>>> Handle(GetEmployeesQuery request, CancellationToken cancellationToken)
    {
        var entities = await _repository.GetAllAsync(cancellationToken);

        var dtos = entities.Select(e => new EmployeeDto 
        { 
            Id = e.Id,
            FirstName = e.FirstName,
            LastName = e.LastName,
            Email = e.Email,
            PhoneNumber = e.PhoneNumber,
            Username = e.Username,
            PasswordHash = e.PasswordHash,
            GrantedModules = e.GrantedModules
        }).ToList();
        
        return Result.Success(dtos);
    }
}

