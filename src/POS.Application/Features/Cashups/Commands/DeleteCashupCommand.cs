using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using POS.Domain.Common;
using POS.Domain.Entities;
using POS.Domain.Interfaces.Repositories;

namespace POS.Application.Features.Cashups.Commands;

public class DeleteCashupCommand : IRequest<Result<bool>>
{
    public int Id { get; set; }
}

public class DeleteCashupCommandHandler : IRequestHandler<DeleteCashupCommand, Result<bool>>
{
    private readonly IRepository<Cashup> _repository;
    private readonly IUnitOfWork _unitOfWork;

    public DeleteCashupCommandHandler(IRepository<Cashup> repository, IUnitOfWork unitOfWork)
    {
        _repository = repository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<bool>> Handle(DeleteCashupCommand request, CancellationToken cancellationToken)
    {
        var entity = await _repository.GetByIdAsync(request.Id, cancellationToken);
        if (entity == null)
            return Result.Failure<bool>($"Cashup with ID {request.Id} not found.");

        entity.Deleted = true;
        entity.DeletedAt = DateTime.UtcNow;
        
        _repository.Update(entity);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        
        return Result.Success(true);
    }
}
