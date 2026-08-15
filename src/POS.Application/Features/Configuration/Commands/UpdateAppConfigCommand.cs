using System.Threading;
using System.Threading.Tasks;
using MediatR;
using POS.Domain.Common;
using POS.Domain.Entities;
using POS.Domain.Interfaces.Repositories;
using POS.Application.Features.Configuration.DTOs;

namespace POS.Application.Features.Configuration.Commands;

public class UpdateAppConfigCommand : IRequest<Result<string>>
{
    public string Id { get; set; }
    public AppConfigDto Dto { get; set; } = new();
}

public class UpdateAppConfigCommandHandler : IRequestHandler<UpdateAppConfigCommand, Result<string>>
{
    private readonly IAppConfigRepository _repository;
    private readonly IUnitOfWork _unitOfWork;

    public UpdateAppConfigCommandHandler(IAppConfigRepository repository, IUnitOfWork unitOfWork)
    {
        _repository = repository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<string>> Handle(UpdateAppConfigCommand request, CancellationToken cancellationToken)
    {
        var existingValue = await _repository.GetValueAsync(request.Id, cancellationToken);
        if (existingValue == null)
            return Result.Failure<string>($"AppConfig with ID {request.Id} not found.");

        await _repository.SetValueAsync(request.Id, request.Dto.Value ?? string.Empty, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        
        return Result.Success(request.Id);
    }
}
