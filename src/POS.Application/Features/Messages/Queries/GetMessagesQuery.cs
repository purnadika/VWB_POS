using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using POS.Domain.Common;
using POS.Domain.Entities;
using POS.Domain.Interfaces.Repositories;

namespace POS.Application.Features.Messages.Queries;

public class GetMessagesQuery : IRequest<Result<IEnumerable<Message>>>
{
}

public class GetMessagesQueryHandler : IRequestHandler<GetMessagesQuery, Result<IEnumerable<Message>>>
{
    private readonly IRepository<Message> _repository;

    public GetMessagesQueryHandler(IRepository<Message> repository)
    {
        _repository = repository;
    }

    public async Task<Result<IEnumerable<Message>>> Handle(GetMessagesQuery request, CancellationToken cancellationToken)
    {
        var entities = await _repository.GetAllAsync(cancellationToken);
        return Result.Success<IEnumerable<Message>>(entities);
    }
}
