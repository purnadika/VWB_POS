using MediatR;
using POS.Domain.Common;
using System.Collections.Generic;

namespace POS.Application.Features.Taxes.Queries;

public record GetTaxesQuery : IRequest<Result<List<DTOs.TaxRateDto>>>;
