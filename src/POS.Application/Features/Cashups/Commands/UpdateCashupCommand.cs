using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using POS.Domain.Common;
using POS.Domain.Entities;
using POS.Domain.Interfaces.Repositories;

namespace POS.Application.Features.Cashups.Commands;

public class UpdateCashupCommand : IRequest<Result<Cashup>>
{
    public int Id { get; set; }
    public DateTime OpenDate { get; set; }
    public DateTime? CloseDate { get; set; }
    public int OpenEmployeeId { get; set; }
    public int? CloseEmployeeId { get; set; }
    public decimal OpenAmount { get; set; }
    public decimal CloseAmount { get; set; }
    public decimal ClosedAmountDue { get; set; }
    public decimal ClosedAmountReceived { get; set; }
    public decimal ClosedAmountDifference { get; set; }
    public string Notes { get; set; } = string.Empty;
}

public class UpdateCashupCommandHandler : IRequestHandler<UpdateCashupCommand, Result<Cashup>>
{
    private readonly IRepository<Cashup> _repository;
    private readonly IUnitOfWork _unitOfWork;

    public UpdateCashupCommandHandler(IRepository<Cashup> repository, IUnitOfWork unitOfWork)
    {
        _repository = repository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<Cashup>> Handle(UpdateCashupCommand request, CancellationToken cancellationToken)
    {
        var entity = await _repository.GetByIdAsync(request.Id, cancellationToken);
        if (entity == null)
            return Result.Failure<Cashup>($"Cashup with ID {request.Id} not found.");

        entity.OpenDate = request.OpenDate;
        entity.CloseDate = request.CloseDate;
        entity.OpenEmployeeId = request.OpenEmployeeId;
        entity.CloseEmployeeId = request.CloseEmployeeId;
        entity.OpenAmount = request.OpenAmount;
        entity.CloseAmount = request.CloseAmount;
        entity.ClosedAmountDue = request.ClosedAmountDue;
        entity.ClosedAmountReceived = request.ClosedAmountReceived;
        entity.ClosedAmountDifference = request.ClosedAmountDifference;
        entity.Notes = request.Notes;
        
        entity.UpdatedAt = DateTime.UtcNow;

        _repository.Update(entity);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        
        return Result.Success(entity);
    }
}
