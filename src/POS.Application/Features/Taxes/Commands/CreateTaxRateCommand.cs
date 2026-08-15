using System.Threading;
using System.Threading.Tasks;
using MediatR;
using POS.Domain.Common;
using POS.Domain.Entities;
using POS.Domain.Interfaces.Repositories;
using System.Linq;

namespace POS.Application.Features.Taxes.Commands;

public class CreateTaxRateCommand : IRequest<Result<int>>
{
    public string Name { get; set; } = string.Empty;
    public decimal Rate { get; set; }
    public int TaxCategoryId { get; set; }
}

public class CreateTaxRateCommandHandler : IRequestHandler<CreateTaxRateCommand, Result<int>>
{
    private readonly IRepository<TaxRate> _repository;
    private readonly IUnitOfWork _unitOfWork;

    public CreateTaxRateCommandHandler(IRepository<TaxRate> repository, IUnitOfWork unitOfWork)
    {
        _repository = repository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<int>> Handle(CreateTaxRateCommand request, CancellationToken cancellationToken)
    {
        var entity = new TaxRate
        {
            Name = request.Name,
            Rate = request.Rate,
            TaxCategoryId = request.TaxCategoryId
        };

        await _repository.AddAsync(entity, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        return Result.Success(entity.Id);
    }
}
