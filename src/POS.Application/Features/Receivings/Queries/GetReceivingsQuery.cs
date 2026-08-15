using MediatR;
using POS.Domain.Common;
using System.Collections.Generic;

namespace POS.Application.Features.Receivings.Queries;

public record GetReceivingsQuery : IRequest<Result<List<DTOs.ReceivingDto>>>;
