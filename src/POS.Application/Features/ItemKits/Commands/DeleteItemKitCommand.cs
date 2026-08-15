using System.Threading;
using System.Threading.Tasks;
using MediatR;
using POS.Domain.Common;
using POS.Domain.Entities;
using POS.Domain.Interfaces.Repositories;

namespace POS.Application.Features.ItemKits.Commands;

public class DeleteItemKitCommand : IRequest<Result<bool>>
{
    public int Id { get; set; }
}

public class DeleteItemKitCommandHandler : IRequestHandler<DeleteItemKitCommand, Result<bool>>
{
    private readonly IRepository<ItemKit> _repository;
    private readonly IUnitOfWork _unitOfWork;

    public DeleteItemKitCommandHandler(IRepository<ItemKit> repository, IUnitOfWork unitOfWork)
    {
        _repository = repository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<bool>> Handle(DeleteItemKitCommand request, CancellationToken cancellationToken)
    {
        var entity = await _repository.GetByIdAsync(request.Id, cancellationToken);
        if (entity == null)
            return Result.Failure<bool>($"ItemKit with ID {request.Id} not found.");

        entity.Deleted = true;
        entity.DeletedAt = System.DateTime.UtcNow;
        _repository.Update(entity);
        
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        
        return Result.Success(true);
    }
}

