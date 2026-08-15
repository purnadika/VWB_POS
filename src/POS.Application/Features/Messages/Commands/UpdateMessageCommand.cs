using System.Threading;
using System.Threading.Tasks;
using MediatR;
using POS.Domain.Common;
using POS.Domain.Entities;
using POS.Domain.Interfaces.Repositories;
using POS.Application.Features.Messages.DTOs;

namespace POS.Application.Features.Messages.Commands;

public class UpdateMessageCommand : IRequest<Result<int>>
{
    public int Id { get; set; }
    public MessageDto Dto { get; set; } = new();
}

public class UpdateMessageCommandHandler : IRequestHandler<UpdateMessageCommand, Result<int>>
{
    private readonly IRepository<Message> _repository;
    private readonly IUnitOfWork _unitOfWork;

    public UpdateMessageCommandHandler(IRepository<Message> repository, IUnitOfWork unitOfWork)
    {
        _repository = repository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<int>> Handle(UpdateMessageCommand request, CancellationToken cancellationToken)
    {
        var entity = await _repository.GetByIdAsync(request.Id, cancellationToken);
        if (entity == null)
            return Result.Failure<int>($"Message with ID {request.Id} not found.");

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
