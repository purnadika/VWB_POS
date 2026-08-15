using System.IO;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using POS.Domain.Common;
using POS.Domain.Entities;
using POS.Domain.Interfaces.Repositories;

namespace POS.Application.Features.Customers.Commands;

public record ImportCustomersCommand(Stream CsvStream) : IRequest<Result<int>>;

public class ImportCustomersCommandHandler : IRequestHandler<ImportCustomersCommand, Result<int>>
{
    private readonly ICustomerRepository _repository;
    private readonly IUnitOfWork _unitOfWork;

    public ImportCustomersCommandHandler(ICustomerRepository repository, IUnitOfWork unitOfWork)
    {
        _repository = repository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<int>> Handle(ImportCustomersCommand request, CancellationToken cancellationToken)
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
            if (values.Length >= 3)
            {
                var customer = new Customer
                {
                    FirstName = values[0],
                    LastName = values[1],
                    Email = values[2],
                    CompanyName = ""
                };
                await _repository.AddAsync(customer, cancellationToken);
                importedCount++;
            }
        }
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        return Result.Success(importedCount);
    }
}
