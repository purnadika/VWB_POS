using System.Threading;
using System.Threading.Tasks;
using MediatR;
using POS.Domain.Common;
using POS.Domain.Entities;
using POS.Domain.Interfaces.Repositories;
using System.Linq;

namespace POS.Application.Features.GiftCards.Commands;

public class CreateGiftcardCommand : IRequest<Result<int>>
{
    public string GiftcardNumber { get; set; } = string.Empty;
    public decimal Value { get; set; }
    public int? CustomerId { get; set; }
}

public class CreateGiftcardCommandHandler : IRequestHandler<CreateGiftcardCommand, Result<int>>
{
    private readonly IRepository<Giftcard> _repository;
    private readonly IUnitOfWork _unitOfWork;

    public CreateGiftcardCommandHandler(IRepository<Giftcard> repository, IUnitOfWork unitOfWork)
    {
        _repository = repository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<int>> Handle(CreateGiftcardCommand request, CancellationToken cancellationToken)
    {
        var entity = new Giftcard
        {
            GiftcardNumber = request.GiftcardNumber,
            Value = request.Value,
            CustomerId = request.CustomerId
        };

        await _repository.AddAsync(entity, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        return Result.Success(entity.Id);
    }
}
