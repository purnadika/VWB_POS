using System.Threading;
using System.Threading.Tasks;
using MediatR;
using POS.Domain.Common;
using POS.Domain.Entities;
using POS.Domain.Interfaces.Repositories;

namespace POS.Application.Features.Messages.Commands;

public class DeleteMessageCommand : IRequest<Result<bool>>
{
    public int Id { get; set; }
}

public class DeleteMessageCommandHandler : IRequestHandler<DeleteMessageCommand, Result<bool>>
{
    private readonly IRepository<Message> _repository;
    private readonly IUnitOfWork _unitOfWork;

    public DeleteMessageCommandHandler(IRepository<Message> repository, IUnitOfWork unitOfWork)
    {
        _repository = repository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<bool>> Handle(DeleteMessageCommand request, CancellationToken cancellationToken)
    {
        var entity = await _repository.GetByIdAsync(request.Id, cancellationToken);
        if (entity == null)
            return Result.Failure<bool>($"Message with ID {request.Id} not found.");

        _repository.Delete(entity);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        
        return Result.Success(true);
    }
}
