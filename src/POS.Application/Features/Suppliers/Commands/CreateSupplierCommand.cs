using System.Threading;
using System.Threading.Tasks;
using MediatR;
using POS.Domain.Common;
using POS.Domain.Entities;
using POS.Domain.Interfaces.Repositories;
using System.Linq;

using POS.Application.Features.Suppliers.DTOs;

namespace POS.Application.Features.Suppliers.Commands;

public class CreateSupplierCommand : IRequest<Result<int>>
{
    public SupplierDto Dto { get; set; } = new();
}

public class CreateSupplierCommandHandler : IRequestHandler<CreateSupplierCommand, Result<int>>
{
    private readonly IRepository<Supplier> _repository;
    private readonly IUnitOfWork _unitOfWork;

    public CreateSupplierCommandHandler(IRepository<Supplier> repository, IUnitOfWork unitOfWork)
    {
        _repository = repository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<int>> Handle(CreateSupplierCommand request, CancellationToken cancellationToken)
    {
        var entity = new Supplier
        {
            FirstName = request.Dto.FirstName ?? "",
            LastName = request.Dto.LastName ?? "",
            Email = request.Dto.Email ?? "",
            PhoneNumber = request.Dto.PhoneNumber ?? "",
            CompanyName = request.Dto.CompanyName ?? "",
            AccountNumber = request.Dto.AccountNumber ?? "",
            AgencyName = request.Dto.AgencyName ?? ""
        };

        await _repository.AddAsync(entity, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        return Result.Success(entity.Id);
    }
}
