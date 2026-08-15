using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using POS.Domain.Common;
using POS.Domain.Entities;
using POS.Domain.Interfaces.Repositories;
using System.Linq;

namespace POS.Application.Features.Receivings.Commands;

public class CreateReceivingCommand : IRequest<Result<int>>
{
    public int? SupplierId { get; set; }
    public int EmployeeId { get; set; }
    public DateTime ReceivingTime { get; set; }
    public string Comment { get; set; } = string.Empty;
    public string Reference { get; set; } = string.Empty;
    public string PaymentType { get; set; } = string.Empty;
    public decimal Total { get; set; }
}

public class CreateReceivingCommandHandler : IRequestHandler<CreateReceivingCommand, Result<int>>
{
    private readonly IRepository<Receiving> _repository;
    private readonly IUnitOfWork _unitOfWork;

    public CreateReceivingCommandHandler(IRepository<Receiving> repository, IUnitOfWork unitOfWork)
    {
        _repository = repository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<int>> Handle(CreateReceivingCommand request, CancellationToken cancellationToken)
    {
        var entity = new Receiving
        {
            SupplierId = request.SupplierId,
            EmployeeId = request.EmployeeId,
            ReceivingTime = request.ReceivingTime,
            Comment = request.Comment,
            Reference = request.Reference,
            PaymentType = request.PaymentType,
            Total = request.Total
        };

        await _repository.AddAsync(entity, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        return Result.Success(entity.Id);
    }
}
