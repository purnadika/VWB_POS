using System.Threading;
using System.Threading.Tasks;
using FluentValidation;
using MediatR;
using POS.Domain.Common;
using POS.Domain.Entities;
using POS.Domain.Interfaces.Repositories;

namespace POS.Application.Features.Items.Commands;

public record CreateItemCommand(
    string Name,
    int? CategoryId,
    string ItemNumber,
    string Description,
    decimal CostPrice,
    decimal UnitPrice,
    decimal ReorderLevel,
    decimal ReceivingQuantity,
    bool IsSerialized,
    bool AllowAltDescription,
    int? SupplierId,
    int? TaxCategoryId
) : IRequest<Result<int>>;

public class CreateItemCommandValidator : AbstractValidator<CreateItemCommand>
{
    public CreateItemCommandValidator()
    {
        RuleFor(x => x.Name).NotEmpty().MaximumLength(255);
        RuleFor(x => x.CostPrice).GreaterThanOrEqualTo(0);
        RuleFor(x => x.UnitPrice).GreaterThanOrEqualTo(0);
    }
}

public class CreateItemCommandHandler : IRequestHandler<CreateItemCommand, Result<int>>
{
    private readonly IItemRepository _itemRepository;
    private readonly IUnitOfWork _unitOfWork;

    public CreateItemCommandHandler(IItemRepository itemRepository, IUnitOfWork unitOfWork)
    {
        _itemRepository = itemRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<int>> Handle(CreateItemCommand request, CancellationToken cancellationToken)
    {
        var item = new Item
        {
            Name = request.Name,
            CategoryId = request.CategoryId,
            ItemNumber = request.ItemNumber,
            Description = request.Description,
            CostPrice = request.CostPrice,
            UnitPrice = request.UnitPrice,
            ReorderLevel = request.ReorderLevel,
            ReceivingQuantity = request.ReceivingQuantity,
            IsSerialized = request.IsSerialized,
            AllowAltDescription = request.AllowAltDescription,
            SupplierId = request.SupplierId,
            TaxCategoryId = request.TaxCategoryId
        };

        await _itemRepository.AddAsync(item, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success(item.Id);
    }
}
