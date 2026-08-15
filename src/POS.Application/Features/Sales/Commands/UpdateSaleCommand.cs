using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using POS.Domain.Common;
using POS.Domain.Entities;
using POS.Domain.Interfaces.Repositories;

namespace POS.Application.Features.Sales.Commands;

public class UpdateSaleCommand : IRequest<Result<Sale>>
{
    public int Id { get; set; }
    public int? CustomerId { get; set; }
    public string Comment { get; set; } = string.Empty;
    public int? DinnerTableId { get; set; }
}

public class UpdateSaleCommandHandler : IRequestHandler<UpdateSaleCommand, Result<Sale>>
{
    private readonly ISaleRepository _repository;
    private readonly IUnitOfWork _unitOfWork;

    public UpdateSaleCommandHandler(ISaleRepository repository, IUnitOfWork unitOfWork)
    {
        _repository = repository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<Sale>> Handle(UpdateSaleCommand request, CancellationToken cancellationToken)
    {
        var sale = await _repository.GetByIdAsync(request.Id, cancellationToken);
        if (sale == null)
            return Result.Failure<Sale>($"Sale with ID {request.Id} not found.");

        sale.CustomerId = request.CustomerId;
        sale.Comment = request.Comment;
        sale.DinnerTableId = request.DinnerTableId;
        
        sale.UpdatedAt = DateTime.UtcNow;

        _repository.Update(sale);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        
        return Result.Success(sale);
    }
}
