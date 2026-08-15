using System.Threading;
using System.Threading.Tasks;
using MediatR;
using POS.Domain.Common;
using POS.Domain.Entities;
using POS.Domain.Interfaces.Repositories;
using POS.Application.Features.Customers.DTOs;
using System.Linq;

namespace POS.Application.Features.Customers.Commands;

public class CreateCustomerCommand : IRequest<Result<int>>
{
    public CustomerDto Dto { get; set; } = new();
}

public class CreateCustomerCommandHandler : IRequestHandler<CreateCustomerCommand, Result<int>>
{
    private readonly ICustomerRepository _repository;
    private readonly IUnitOfWork _unitOfWork;

    public CreateCustomerCommandHandler(ICustomerRepository repository, IUnitOfWork unitOfWork)
    {
        _repository = repository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<int>> Handle(CreateCustomerCommand request, CancellationToken cancellationToken)
    {
        var customer = new Customer
        {
            FirstName = request.Dto.FirstName ?? "",
            LastName = request.Dto.LastName ?? "",
            Email = request.Dto.Email ?? "",
            CompanyName = request.Dto.CompanyName ?? ""
        };

        await _repository.AddAsync(customer, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        return Result.Success(customer.Id);
    }
}
