using MediatR;
using POS.Domain.Common;
using System.Collections.Generic;

namespace POS.Application.Features.Customers.Queries;

public record GetCustomersQuery : IRequest<Result<List<DTOs.CustomerDto>>>;
