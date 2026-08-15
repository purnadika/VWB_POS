using System.IO;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using POS.Domain.Common;
using POS.Domain.Entities;
using POS.Domain.Interfaces.Repositories;

namespace POS.Application.Features.Items.Commands;

public record ImportItemsCommand(Stream CsvStream) : IRequest<Result<int>>;

public class ImportItemsCommandHandler : IRequestHandler<ImportItemsCommand, Result<int>>
{
    private readonly IRepository<Item> _repository;
    private readonly IUnitOfWork _unitOfWork;

    public ImportItemsCommandHandler(IRepository<Item> repository, IUnitOfWork unitOfWork)
    {
        _repository = repository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<int>> Handle(ImportItemsCommand request, CancellationToken cancellationToken)
    {
        using var reader = new StreamReader(request.CsvStream);
        var headerLine = await reader.ReadLineAsync();
        if (headerLine == null) return Result.Failure<int>("File is empty");
        
        int importedCount = 0;
        while (!reader.EndOfStream)
        {
            var line = await reader.ReadLineAsync();
            if (string.IsNullOrWhiteSpace(line)) continue;
            
            var values = line.Split(',');
            if (values.Length >= 4)
            {
                var item = new Item
                {
                    Name = values[0],
                    CategoryId = null, // CSV import doesn't map to category FK
                    CostPrice = decimal.TryParse(values[2], out var cost) ? cost : 0,
                    UnitPrice = decimal.TryParse(values[3], out var price) ? price : 0
                };
                await _repository.AddAsync(item, cancellationToken);
                importedCount++;
            }
        }
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        return Result.Success(importedCount);
    }
}
