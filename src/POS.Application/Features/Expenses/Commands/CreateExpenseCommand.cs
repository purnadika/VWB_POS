using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using POS.Domain.Common;
using POS.Domain.Entities;
using POS.Domain.Interfaces.Repositories;
using System.Linq;

namespace POS.Application.Features.Expenses.Commands;

public class CreateExpenseCommand : IRequest<Result<int>>
{
    public int CategoryId { get; set; }
    public decimal Amount { get; set; }
    public string PaymentType { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public int EmployeeId { get; set; }
    public DateTime Date { get; set; } = DateTime.UtcNow;
}

public class CreateExpenseCommandHandler : IRequestHandler<CreateExpenseCommand, Result<int>>
{
    private readonly IRepository<Expense> _repository;
    private readonly IUnitOfWork _unitOfWork;

    public CreateExpenseCommandHandler(IRepository<Expense> repository, IUnitOfWork unitOfWork)
    {
        _repository = repository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<int>> Handle(CreateExpenseCommand request, CancellationToken cancellationToken)
    {
        var entity = new Expense
        {
            CategoryId = request.CategoryId,
            Amount = request.Amount,
            PaymentType = request.PaymentType,
            Description = request.Description,
            EmployeeId = request.EmployeeId,
            Date = request.Date
        };

        await _repository.AddAsync(entity, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        return Result.Success(entity.Id);
    }
}
