using System.Threading;
using System.Threading.Tasks;
using MediatR;
using POS.Domain.Common;
using POS.Domain.Entities;
using POS.Domain.Interfaces.Repositories;
using POS.Application.Features.Configuration.DTOs;
using System.Linq;

namespace POS.Application.Features.Configuration.Queries;

public class GetConfigurationQueryHandler : IRequestHandler<GetConfigurationQuery, Result<System.Collections.Generic.List<AppConfigDto>>>
{
    private readonly IRepository<AppConfig> _repository;

    public GetConfigurationQueryHandler(IRepository<AppConfig> repository)
    {
        _repository = repository;
    }

    public async Task<Result<System.Collections.Generic.List<AppConfigDto>>> Handle(GetConfigurationQuery request, CancellationToken cancellationToken)
    {
        var entities = await _repository.GetAllAsync(cancellationToken);
        var dtos = entities.Select(e => new AppConfigDto 
        { 
            Key = e.Key,
            Value = e.Value
        }).ToList();
        
        return Result.Success(dtos);
    }
}
