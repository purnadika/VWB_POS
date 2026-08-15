using System.Threading;
using System.Threading.Tasks;
using MediatR;
using POS.Domain.Common;
using POS.Domain.Entities;
using POS.Domain.Interfaces.Repositories;
using System.Linq;

namespace POS.Application.Features.Configuration.Commands;

public class CreateAppConfigCommand : IRequest<Result<string>>
{
    public string Key { get; set; } = string.Empty;
    public string Value { get; set; } = string.Empty;
}

public class CreateAppConfigCommandHandler : IRequestHandler<CreateAppConfigCommand, Result<string>>
{
    private readonly IAppConfigRepository _repository;
    private readonly IUnitOfWork _unitOfWork;

    public CreateAppConfigCommandHandler(IAppConfigRepository repository, IUnitOfWork unitOfWork)
    {
        _repository = repository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<string>> Handle(CreateAppConfigCommand request, CancellationToken cancellationToken)
    {
        await _repository.SetValueAsync(request.Key, request.Value, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        return Result.Success(request.Key);
    }
}

