using System.Threading;
using System.Threading.Tasks;
using MediatR;
using POS.Domain.Common;
using POS.Domain.Entities;
using POS.Domain.Interfaces.Repositories;

namespace POS.Application.Features.Configuration.Commands;

public class DeleteAppConfigCommand : IRequest<Result<bool>>
{
    public string Id { get; set; }
}

public class DeleteAppConfigCommandHandler : IRequestHandler<DeleteAppConfigCommand, Result<bool>>
{
    private readonly IAppConfigRepository _repository;
    private readonly IUnitOfWork _unitOfWork;

    public DeleteAppConfigCommandHandler(IAppConfigRepository repository, IUnitOfWork unitOfWork)
    {
        _repository = repository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<bool>> Handle(DeleteAppConfigCommand request, CancellationToken cancellationToken)
    {
        var existingValue = await _repository.GetValueAsync(request.Id, cancellationToken);
        if (existingValue == null)
            return Result.Failure<bool>($"AppConfig with ID {request.Id} not found.");

        await _repository.DeleteAsync(request.Id, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        
        return Result.Success(true);
    }
}

