using System.Threading;
using System.Threading.Tasks;
using MediatR;
using POS.Domain.Common;
using POS.Domain.Entities;
using POS.Domain.Interfaces.Repositories;
using POS.Application.Features.Receivings.DTOs;

namespace POS.Application.Features.Receivings.Commands;

public class UpdateReceivingCommand : IRequest<Result<int>>
{
    public int Id { get; set; }
    public ReceivingDto Dto { get; set; } = new();
}

public class UpdateReceivingCommandHandler : IRequestHandler<UpdateReceivingCommand, Result<int>>
{
    private readonly IRepository<Receiving> _repository;
    private readonly IUnitOfWork _unitOfWork;

    public UpdateReceivingCommandHandler(IRepository<Receiving> repository, IUnitOfWork unitOfWork)
    {
        _repository = repository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<int>> Handle(UpdateReceivingCommand request, CancellationToken cancellationToken)
    {
        var entity = await _repository.GetByIdAsync(request.Id, cancellationToken);
        if (entity == null)
            return Result.Failure<int>($"Receiving with ID {request.Id} not found.");

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

