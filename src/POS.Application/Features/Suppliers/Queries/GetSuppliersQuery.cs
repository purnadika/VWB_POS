using MediatR;
using POS.Domain.Common;
using System.Collections.Generic;

namespace POS.Application.Features.Suppliers.Queries;

public record GetSuppliersQuery : IRequest<Result<List<DTOs.SupplierDto>>>;
