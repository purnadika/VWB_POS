using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using FluentValidation;
using MediatR;
using POS.Domain.Common;
using POS.Domain.Entities;
using POS.Domain.Enums;
using POS.Domain.Interfaces.Repositories;
using POS.Domain.Interfaces.Services;

namespace POS.Application.Features.Sales.Commands;

public record CreateSaleCommand(
    int? CustomerId,
    int EmployeeId,
    string Comment,
    int? DinnerTableId,
    List<SaleItemDto> SaleItems,
    List<SalePaymentDto> Payments
) : IRequest<Result<int>>;

public record SaleItemDto(
    int ItemId,
    decimal Quantity,
    decimal DiscountPercent,
    decimal? UnitPriceOverride,
    string SerialNumber,
    int LocationId
);

public record SalePaymentDto(
    PaymentMethod PaymentMethod,
    decimal Amount
);

public class CreateSaleCommandValidator : AbstractValidator<CreateSaleCommand>
{
    public CreateSaleCommandValidator()
    {
        RuleFor(x => x.EmployeeId).NotEmpty();
        RuleFor(x => x.SaleItems).NotEmpty().WithMessage("At least one sale item is required.");
        RuleFor(x => x.Payments).NotEmpty().WithMessage("At least one payment method is required.");
    }
}

public class CreateSaleCommandHandler : IRequestHandler<CreateSaleCommand, Result<int>>
{
    private readonly ISaleRepository _saleRepository;
    private readonly IItemRepository _itemRepository;
    private readonly ITaxCalculationService _taxCalculationService;
    private readonly IUnitOfWork _unitOfWork;

    public CreateSaleCommandHandler(
        ISaleRepository saleRepository,
        IItemRepository itemRepository,
        ITaxCalculationService taxCalculationService,
        IUnitOfWork unitOfWork)
    {
        _saleRepository = saleRepository;
        _itemRepository = itemRepository;
        _taxCalculationService = taxCalculationService;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<int>> Handle(CreateSaleCommand request, CancellationToken cancellationToken)
    {
        var sale = new Sale
        {
            CustomerId = request.CustomerId,
            EmployeeId = request.EmployeeId,
            Comment = request.Comment,
            DinnerTableId = request.DinnerTableId,
            SaleTime = DateTime.UtcNow,
            Status = SaleStatus.Completed
        };

        decimal subTotal = 0;

        foreach (var itemDto in request.SaleItems)
        {
            var item = await _itemRepository.GetByIdAsync(itemDto.ItemId, cancellationToken);
            if (item == null)
            {
                return Result.Failure<int>($"Item with ID {itemDto.ItemId} was not found.");
            }

            var saleItem = new SaleItem
            {
                ItemId = itemDto.ItemId,
                Quantity = itemDto.Quantity,
                UnitPrice = itemDto.UnitPriceOverride ?? item.UnitPrice,
                CostPrice = item.CostPrice,
                DiscountPercent = itemDto.DiscountPercent,
                SerialNumber = itemDto.SerialNumber
            };

            sale.SaleItems.Add(saleItem);
            subTotal += saleItem.LineTotal;

            // Subtract stock level and add inventory transaction log
            await _itemRepository.UpdateStockLevelAsync(itemDto.ItemId, itemDto.LocationId, -itemDto.Quantity, cancellationToken);
        }

        sale.SubTotal = subTotal;

        // Calculate taxes using Domain Service
        var taxes = await _taxCalculationService.CalculateTaxesAsync(sale, cancellationToken);
        sale.SaleTaxes = taxes;
        sale.TaxTotal = taxes.Sum(t => t.Amount);
        sale.Total = sale.SubTotal + sale.TaxTotal;

        // Apply payments
        foreach (var paymentDto in request.Payments)
        {
            sale.Payments.Add(new SalePayment
            {
                PaymentMethod = paymentDto.PaymentMethod,
                Amount = paymentDto.Amount
            });
        }

        // Generate a simple unique invoice number
        sale.InvoiceNumber = $"INV-{DateTime.UtcNow:yyyyMMddHHmmss}-{new Random().Next(1000, 9999)}";

        await _saleRepository.AddAsync(sale, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success(sale.Id);
    }
}
