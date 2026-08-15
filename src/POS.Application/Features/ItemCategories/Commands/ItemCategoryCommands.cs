using System.Threading;
using System.Threading.Tasks;
using MediatR;
using POS.Application.Features.ItemCategories.DTOs;
using POS.Domain.Common;
using POS.Domain.Entities;
using POS.Domain.Interfaces.Repositories;

namespace POS.Application.Features.ItemCategories.Commands;

// ── Create ──────────────────────────────────────────────────────────────────
public record CreateItemCategoryCommand(string Name, string Description) : IRequest<Result<int>>;

public class CreateItemCategoryCommandHandler : IRequestHandler<CreateItemCategoryCommand, Result<int>>
{
    private readonly IRepository<ItemCategory> _repository;
    private readonly IUnitOfWork _unitOfWork;

    public CreateItemCategoryCommandHandler(IRepository<ItemCategory> repository, IUnitOfWork unitOfWork)
    {
        _repository = repository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<int>> Handle(CreateItemCategoryCommand request, CancellationToken cancellationToken)
    {
        var entity = new ItemCategory { Name = request.Name, Description = request.Description };
        await _repository.AddAsync(entity, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        return Result.Success(entity.Id);
    }
}

// ── Update ──────────────────────────────────────────────────────────────────
public record UpdateItemCategoryCommand(int Id, ItemCategoryDto Dto) : IRequest<Result<ItemCategoryDto>>;

public class UpdateItemCategoryCommandHandler : IRequestHandler<UpdateItemCategoryCommand, Result<ItemCategoryDto>>
{
    private readonly IRepository<ItemCategory> _repository;
    private readonly IUnitOfWork _unitOfWork;

    public UpdateItemCategoryCommandHandler(IRepository<ItemCategory> repository, IUnitOfWork unitOfWork)
    {
        _repository = repository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<ItemCategoryDto>> Handle(UpdateItemCategoryCommand request, CancellationToken cancellationToken)
    {
        var entity = await _repository.GetByIdAsync(request.Id, cancellationToken);
        if (entity is null) return Result.Failure<ItemCategoryDto>("Category not found.");
        entity.Name = request.Dto.Name;
        entity.Description = request.Dto.Description;
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        return Result.Success(new ItemCategoryDto { Id = entity.Id, Name = entity.Name, Description = entity.Description });
    }
}

// ── Delete ──────────────────────────────────────────────────────────────────
public record DeleteItemCategoryCommand(int Id) : IRequest<Result<bool>>;

public class DeleteItemCategoryCommandHandler : IRequestHandler<DeleteItemCategoryCommand, Result<bool>>
{
    private readonly IRepository<ItemCategory> _repository;
    private readonly IUnitOfWork _unitOfWork;

    public DeleteItemCategoryCommandHandler(IRepository<ItemCategory> repository, IUnitOfWork unitOfWork)
    {
        _repository = repository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<bool>> Handle(DeleteItemCategoryCommand request, CancellationToken cancellationToken)
    {
        var entity = await _repository.GetByIdAsync(request.Id, cancellationToken);
        if (entity is null) return Result.Failure<bool>("Category not found.");
        _repository.Delete(entity);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        return Result.Success(true);
    }
}
