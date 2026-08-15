using System.Threading;
using System.Threading.Tasks;
using MediatR;
using POS.Domain.Common;
using POS.Domain.Entities;
using POS.Domain.Interfaces.Repositories;
using POS.Application.Features.Suppliers.DTOs;

namespace POS.Application.Features.Suppliers.Commands;

public class UpdateSupplierCommand : IRequest<Result<int>>
{
    public int Id { get; set; }
    public SupplierDto Dto { get; set; } = new();
}

public class UpdateSupplierCommandHandler : IRequestHandler<UpdateSupplierCommand, Result<int>>
{
    private readonly IRepository<Supplier> _repository;
    private readonly IUnitOfWork _unitOfWork;

    public UpdateSupplierCommandHandler(IRepository<Supplier> repository, IUnitOfWork unitOfWork)
    {
        _repository = repository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<int>> Handle(UpdateSupplierCommand request, CancellationToken cancellationToken)
    {
        var entity = await _repository.GetByIdAsync(request.Id, cancellationToken);
        if (entity == null)
            return Result.Failure<int>($"Supplier with ID {request.Id} not found.");

                // Auto-mapping properties from DTO to Entity
        foreach (var prop in request.Dto.GetType().GetProperties()) {
            if (prop.Name == "Id") continue;
            var entityProp = entity.GetType().GetProperty(prop.Name);
            if (entityProp != null && entityProp.CanWrite) {
                var value = prop.GetValue(request.Dto);
                if (value != null) entityProp.SetValue(entity, value);
            }
        }

        _repository.Update(entity);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        
        return Result.Success(request.Id);
    }
}

