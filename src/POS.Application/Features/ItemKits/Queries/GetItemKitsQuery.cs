using MediatR;
using POS.Domain.Common;
using System.Collections.Generic;

namespace POS.Application.Features.ItemKits.Queries;

public record GetItemKitsQuery : IRequest<Result<List<DTOs.ItemKitDto>>>;
