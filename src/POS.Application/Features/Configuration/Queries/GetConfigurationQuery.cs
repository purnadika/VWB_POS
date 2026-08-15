using MediatR;
using POS.Domain.Common;
using System.Collections.Generic;

namespace POS.Application.Features.Configuration.Queries;

public record GetConfigurationQuery : IRequest<Result<List<DTOs.AppConfigDto>>>;
