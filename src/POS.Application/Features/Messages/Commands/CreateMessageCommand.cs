using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using POS.Domain.Common;
using POS.Domain.Entities;
using POS.Domain.Interfaces.Repositories;

namespace POS.Application.Features.Messages.Commands;

public class CreateMessageCommand : IRequest<Result<int>>
{
    public int SenderId { get; set; }
    public int ReceiverId { get; set; }
    public string Subject { get; set; } = string.Empty;
    public string Body { get; set; } = string.Empty;
    public DateTime SentAt { get; set; }
}

public class CreateMessageCommandHandler : IRequestHandler<CreateMessageCommand, Result<int>>
{
    private readonly IRepository<Message> _repository;
    private readonly IUnitOfWork _unitOfWork;

    public CreateMessageCommandHandler(IRepository<Message> repository, IUnitOfWork unitOfWork)
    {
        _repository = repository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<int>> Handle(CreateMessageCommand request, CancellationToken cancellationToken)
    {
        var entity = new Message
        {
            SenderId = request.SenderId,
            ReceiverId = request.ReceiverId,
            Subject = request.Subject,
            Body = request.Body,
            SentAt = request.SentAt
        };

        await _repository.AddAsync(entity, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        return Result.Success(entity.Id);
    }
}
