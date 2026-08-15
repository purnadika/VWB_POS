using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using POS.Application.Features.ItemCategories.DTOs;
using POS.Domain.Common;
using POS.Domain.Interfaces.Repositories;

namespace POS.Application.Features.ItemCategories.Queries;

public record GetItemCategoriesQuery : IRequest<Result<List<ItemCategoryDto>>>;

public class GetItemCategoriesQueryHandler : IRequestHandler<GetItemCategoriesQuery, Result<List<ItemCategoryDto>>>
{
    private readonly IRepository<POS.Domain.Entities.ItemCategory> _repository;

    public GetItemCategoriesQueryHandler(IRepository<POS.Domain.Entities.ItemCategory> repository)
    {
        _repository = repository;
    }

    public async Task<Result<List<ItemCategoryDto>>> Handle(GetItemCategoriesQuery request, CancellationToken cancellationToken)
    {
        var all = await _repository.GetAllAsync(cancellationToken);
        var dtos = all.Select(c => new ItemCategoryDto
        {
            Id = c.Id,
            Name = c.Name,
            Description = c.Description
        }).ToList();
        return Result.Success(dtos);
    }
}
