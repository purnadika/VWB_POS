using MediatR;
using POS.Domain.Common;
using System.Collections.Generic;

namespace POS.Application.Features.GiftCards.Queries;

public record GetGiftCardsQuery : IRequest<Result<List<DTOs.GiftcardDto>>>;
