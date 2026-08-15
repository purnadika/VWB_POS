using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using POS.Domain.Common;
using POS.Domain.Entities;
using POS.Domain.Enums;
using POS.Domain.Interfaces.Repositories;

namespace POS.Application.Features.Sales.Commands;

public class DeleteSaleCommand : IRequest<Result<bool>>
{
    public int Id { get; set; }
}

public class DeleteSaleCommandHandler : IRequestHandler<DeleteSaleCommand, Result<bool>>
{
    private readonly ISaleRepository _repository;
    private readonly IUnitOfWork _unitOfWork;

    public DeleteSaleCommandHandler(ISaleRepository repository, IUnitOfWork unitOfWork)
    {
        _repository = repository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<bool>> Handle(DeleteSaleCommand request, CancellationToken cancellationToken)
    {
        var entity = await _repository.GetByIdAsync(request.Id, cancellationToken);
        if (entity == null)
            return Result.Failure<bool>($"Sale with ID {request.Id} not found.");

        entity.Deleted = true;
        entity.DeletedAt = DateTime.UtcNow;
        entity.Status = SaleStatus.Cancelled;
        
        _repository.Update(entity);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        
        return Result.Success(true);
    }
}
