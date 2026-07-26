using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using POS.Domain.Common;
using POS.Domain.Enums;
using POS.Domain.Interfaces.Repositories;

namespace POS.Application.Features.Sales.Queries;

public record GetSaleQuery(int Id) : IRequest<Result<SaleDetailsDto>>;

public record SaleDetailsDto(
    int Id,
    string InvoiceNumber,
    DateTime SaleTime,
    SaleStatus Status,
    string CustomerName,
    string EmployeeName,
    decimal SubTotal,
    decimal TaxTotal,
    decimal Total,
    List<SaleItemDetailsDto> Items,
    List<SalePaymentDetailsDto> Payments
);

public record SaleItemDetailsDto(
    int ItemId,
    string ItemName,
    decimal Quantity,
    decimal UnitPrice,
    decimal DiscountPercent,
    decimal LineTotal
);

public record SalePaymentDetailsDto(
    PaymentMethod PaymentMethod,
    decimal Amount
);

public class GetSaleQueryHandler : IRequestHandler<GetSaleQuery, Result<SaleDetailsDto>>
{
    private readonly ISaleRepository _saleRepository;

    public GetSaleQueryHandler(ISaleRepository saleRepository)
    {
        _saleRepository = saleRepository;
    }

    public async Task<Result<SaleDetailsDto>> Handle(GetSaleQuery request, CancellationToken cancellationToken)
    {
        var sale = await _saleRepository.GetByIdAsync(request.Id, cancellationToken);
        if (sale == null)
        {
            return Result.Failure<SaleDetailsDto>($"Sale with ID {request.Id} was not found.");
        }

        var customerName = sale.Customer != null ? sale.Customer.FullName : "Walk-in Customer";
        var employeeName = sale.Employee != null ? sale.Employee.FullName : "System";

        var dto = new SaleDetailsDto(
            sale.Id,
            sale.InvoiceNumber,
            sale.SaleTime,
            sale.Status,
            customerName,
            employeeName,
            sale.SubTotal,
            sale.TaxTotal,
            sale.Total,
            sale.SaleItems.Select(si => new SaleItemDetailsDto(
                si.ItemId,
                si.Item?.Name ?? "Unknown Item",
                si.Quantity,
                si.UnitPrice,
                si.DiscountPercent,
                si.LineTotal
            )).ToList(),
            sale.Payments.Select(p => new SalePaymentDetailsDto(
                p.PaymentMethod,
                p.Amount
            )).ToList()
        );

        return Result.Success(dto);
    }
}
