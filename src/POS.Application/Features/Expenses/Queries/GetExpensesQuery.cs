using MediatR;
using POS.Domain.Common;
using System.Collections.Generic;

namespace POS.Application.Features.Expenses.Queries;

public record GetExpensesQuery : IRequest<Result<List<DTOs.ExpenseDto>>>;
