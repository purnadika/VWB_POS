using System.Threading;
using System.Threading.Tasks;
using MediatR;
using POS.Domain.Common;
using POS.Domain.Entities;
using POS.Domain.Interfaces.Repositories;

namespace POS.Application.Features.Taxes.Commands;

public class DeleteTaxRateCommand : IRequest<Result<bool>>
{
    public int Id { get; set; }
}

public class DeleteTaxRateCommandHandler : IRequestHandler<DeleteTaxRateCommand, Result<bool>>
{
    private readonly IRepository<TaxRate> _repository;
    private readonly IUnitOfWork _unitOfWork;

    public DeleteTaxRateCommandHandler(IRepository<TaxRate> repository, IUnitOfWork unitOfWork)
    {
        _repository = repository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<bool>> Handle(DeleteTaxRateCommand request, CancellationToken cancellationToken)
    {
        var entity = await _repository.GetByIdAsync(request.Id, cancellationToken);
        if (entity == null)
            return Result.Failure<bool>($"TaxRate with ID {request.Id} not found.");

        entity.Deleted = true;
        entity.DeletedAt = System.DateTime.UtcNow;
        _repository.Update(entity);
        
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        
        return Result.Success(true);
    }
}

