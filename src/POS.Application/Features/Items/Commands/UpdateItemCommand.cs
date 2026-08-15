using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using POS.Domain.Common;
using POS.Domain.Entities;
using POS.Domain.Interfaces.Repositories;

namespace POS.Application.Features.Items.Commands;

public class UpdateItemCommand : IRequest<Result<Item>>
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public int? CategoryId { get; set; }
    public decimal CostPrice { get; set; }
    public decimal UnitPrice { get; set; }
    public string? ItemNumber { get; set; }
    public string? Description { get; set; }
    public decimal ReorderLevel { get; set; }
    public bool AllowAltDescription { get; set; }
    public bool IsSerialized { get; set; }
    public int? SupplierId { get; set; }
}

public class UpdateItemCommandHandler : IRequestHandler<UpdateItemCommand, Result<Item>>
{
    private readonly IItemRepository _repository;
    private readonly IUnitOfWork _unitOfWork;

    public UpdateItemCommandHandler(IItemRepository repository, IUnitOfWork unitOfWork)
    {
        _repository = repository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<Item>> Handle(UpdateItemCommand request, CancellationToken cancellationToken)
    {
        var item = await _repository.GetByIdAsync(request.Id, cancellationToken);
        if (item == null)
            return Result.Failure<Item>($"Item with ID {request.Id} not found.");

        item.Name = request.Name;
        item.CategoryId = request.CategoryId;
        item.CostPrice = request.CostPrice;
        item.UnitPrice = request.UnitPrice;
        item.ItemNumber = request.ItemNumber ?? string.Empty;
        item.Description = request.Description ?? string.Empty;
        item.ReorderLevel = request.ReorderLevel;
        item.AllowAltDescription = request.AllowAltDescription;
        item.IsSerialized = request.IsSerialized;
        item.SupplierId = request.SupplierId;
        
        item.UpdatedAt = DateTime.UtcNow;

        _repository.Update(item);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        
        return Result.Success(item);
    }
}
