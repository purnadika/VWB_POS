using System.Threading;
using System.Threading.Tasks;
using MediatR;
using POS.Domain.Common;
using POS.Domain.Entities;
using POS.Domain.Interfaces.Repositories;
using System.Linq;

using POS.Application.Features.ItemKits.DTOs;

namespace POS.Application.Features.ItemKits.Commands;

public class CreateItemKitCommand : IRequest<Result<int>>
{
    public ItemKitDto Dto { get; set; } = new();
}

public class CreateItemKitCommandHandler : IRequestHandler<CreateItemKitCommand, Result<int>>
{
    private readonly IRepository<ItemKit> _repository;
    private readonly IUnitOfWork _unitOfWork;

    public CreateItemKitCommandHandler(IRepository<ItemKit> repository, IUnitOfWork unitOfWork)
    {
        _repository = repository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<int>> Handle(CreateItemKitCommand request, CancellationToken cancellationToken)
    {
        var entity = new ItemKit
        {
            Name = request.Dto.Name ?? "",
            ItemKitNumber = request.Dto.ItemKitNumber ?? "",
            Description = request.Dto.Description ?? "",
            UnitPrice = request.Dto.UnitPrice,
            CostPrice = request.Dto.CostPrice
        };

        await _repository.AddAsync(entity, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        return Result.Success(entity.Id);
    }
}
