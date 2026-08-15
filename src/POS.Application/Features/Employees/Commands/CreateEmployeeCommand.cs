using System.Threading;
using System.Threading.Tasks;
using MediatR;
using POS.Domain.Common;
using POS.Domain.Entities;
using POS.Domain.Interfaces.Repositories;
using System.Linq;

using POS.Application.Features.Employees.DTOs;

namespace POS.Application.Features.Employees.Commands;

public class CreateEmployeeCommand : IRequest<Result<int>>
{
    public EmployeeDto Dto { get; set; } = new();
}

public class CreateEmployeeCommandHandler : IRequestHandler<CreateEmployeeCommand, Result<int>>
{
    private readonly IRepository<Employee> _repository;
    private readonly IUnitOfWork _unitOfWork;

    public CreateEmployeeCommandHandler(IRepository<Employee> repository, IUnitOfWork unitOfWork)
    {
        _repository = repository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<int>> Handle(CreateEmployeeCommand request, CancellationToken cancellationToken)
    {
        var entity = new Employee
        {
            FirstName = request.Dto.FirstName ?? "",
            LastName = request.Dto.LastName ?? "",
            Email = request.Dto.Email ?? "",
            PhoneNumber = request.Dto.PhoneNumber ?? "",
            Username = request.Dto.Username ?? "",
            PasswordHash = request.Dto.PasswordHash ?? "",
            GrantedModules = request.Dto.GrantedModules ?? new()
        };

        await _repository.AddAsync(entity, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        return Result.Success(entity.Id);
    }
}
