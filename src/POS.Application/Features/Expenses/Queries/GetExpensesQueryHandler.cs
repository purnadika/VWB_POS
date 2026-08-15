using System.Threading;
using System.Threading.Tasks;
using MediatR;
using POS.Domain.Common;
using POS.Domain.Entities;
using POS.Domain.Interfaces.Repositories;
using POS.Application.Features.Expenses.DTOs;
using System.Linq;

namespace POS.Application.Features.Expenses.Queries;

public class GetExpensesQueryHandler : IRequestHandler<GetExpensesQuery, Result<System.Collections.Generic.List<ExpenseDto>>>
{
    private readonly IRepository<Expense> _repository;

    public GetExpensesQueryHandler(IRepository<Expense> repository)
    {
        _repository = repository;
    }

    public async Task<Result<System.Collections.Generic.List<ExpenseDto>>> Handle(GetExpensesQuery request, CancellationToken cancellationToken)
    {
        var entities = await _repository.GetAllAsync(cancellationToken);
        var dtos = entities.Select(e => new ExpenseDto 
        { 
            Id = e.Id,
            CategoryId = e.CategoryId,
            Amount = e.Amount,
            PaymentType = e.PaymentType,
            Description = e.Description,
            EmployeeId = e.EmployeeId,
            Date = e.Date
        }).ToList();
        
        return Result.Success(dtos);
    }
}
