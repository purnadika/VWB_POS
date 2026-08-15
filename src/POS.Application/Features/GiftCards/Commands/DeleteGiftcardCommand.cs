using System.Threading;
using System.Threading.Tasks;
using MediatR;
using POS.Domain.Common;
using POS.Domain.Entities;
using POS.Domain.Interfaces.Repositories;

namespace POS.Application.Features.GiftCards.Commands;

public class DeleteGiftcardCommand : IRequest<Result<bool>>
{
    public int Id { get; set; }
}

public class DeleteGiftcardCommandHandler : IRequestHandler<DeleteGiftcardCommand, Result<bool>>
{
    private readonly IRepository<Giftcard> _repository;
    private readonly IUnitOfWork _unitOfWork;

    public DeleteGiftcardCommandHandler(IRepository<Giftcard> repository, IUnitOfWork unitOfWork)
    {
        _repository = repository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<bool>> Handle(DeleteGiftcardCommand request, CancellationToken cancellationToken)
    {
        var entity = await _repository.GetByIdAsync(request.Id, cancellationToken);
        if (entity == null)
            return Result.Failure<bool>($"Giftcard with ID {request.Id} not found.");

        entity.Deleted = true;
        entity.DeletedAt = System.DateTime.UtcNow;
        _repository.Update(entity);
        
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        
        return Result.Success(true);
    }
}

