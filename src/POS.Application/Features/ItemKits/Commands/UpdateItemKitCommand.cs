using System.Threading;
using System.Threading.Tasks;
using MediatR;
using POS.Domain.Common;
using POS.Domain.Entities;
using POS.Domain.Interfaces.Repositories;
using POS.Application.Features.ItemKits.DTOs;

namespace POS.Application.Features.ItemKits.Commands;

public class UpdateItemKitCommand : IRequest<Result<int>>
{
    public int Id { get; set; }
    public ItemKitDto Dto { get; set; } = new();
}

public class UpdateItemKitCommandHandler : IRequestHandler<UpdateItemKitCommand, Result<int>>
{
    private readonly IRepository<ItemKit> _repository;
    private readonly IUnitOfWork _unitOfWork;

    public UpdateItemKitCommandHandler(IRepository<ItemKit> repository, IUnitOfWork unitOfWork)
    {
        _repository = repository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<int>> Handle(UpdateItemKitCommand request, CancellationToken cancellationToken)
    {
        var entity = await _repository.GetByIdAsync(request.Id, cancellationToken);
        if (entity == null)
            return Result.Failure<int>($"ItemKit with ID {request.Id} not found.");

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

