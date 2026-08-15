using MediatR;
using POS.Domain.Common;
using System.Collections.Generic;

namespace POS.Application.Features.Employees.Queries;

public record GetEmployeesQuery : IRequest<Result<List<DTOs.EmployeeDto>>>;
