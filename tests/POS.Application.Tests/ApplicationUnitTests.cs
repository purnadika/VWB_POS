using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Moq;
using POS.Application.Features.Items.Commands;
using POS.Domain.Common;
using POS.Domain.Entities;
using POS.Domain.Interfaces.Repositories;
using Xunit;

namespace POS.Application.Tests;

public class CreateItemCommandHandlerTests
{
    private readonly Mock<IItemRepository> _itemRepoMock;
    private readonly Mock<IUnitOfWork> _uowMock;
    private readonly CreateItemCommandHandler _handler;

    public CreateItemCommandHandlerTests()
    {
        _itemRepoMock = new Mock<IItemRepository>();
        _uowMock = new Mock<IUnitOfWork>();

        // Default SaveChangesAsync returns 1 (one row affected)
        _uowMock.Setup(uow => uow.SaveChangesAsync(It.IsAny<CancellationToken>()))
                .ReturnsAsync(1);

        _handler = new CreateItemCommandHandler(_itemRepoMock.Object, _uowMock.Object);
    }

    [Fact]
    public async Task Handle_ValidCommand_AddsItemAndReturnsSuccess()
    {
        // Arrange
        var command = new CreateItemCommand(
            Name: "Test Product",
            CategoryId: 1, // Changed from Category: "Test Category"
            ItemNumber: "SKU-TEST-001",
            Description: "A test product.",
            CostPrice: 5.00m,
            UnitPrice: 12.50m,
            ReorderLevel: 10,
            ReceivingQuantity: 20,
            IsSerialized: false,
            AllowAltDescription: false,
            SupplierId: null,
            TaxCategoryId: null
        );

        Item? capturedItem = null;
        _itemRepoMock
            .Setup(r => r.AddAsync(It.IsAny<Item>(), It.IsAny<CancellationToken>()))
            .Callback<Item, CancellationToken>((item, _) => capturedItem = item)
            .Returns(Task.CompletedTask);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.True(result.IsSuccess);
        _itemRepoMock.Verify(r => r.AddAsync(It.IsAny<Item>(), It.IsAny<CancellationToken>()), Times.Once);
        _uowMock.Verify(u => u.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
        Assert.NotNull(capturedItem);
        Assert.Equal("Test Product", capturedItem!.Name);
        Assert.Equal("SKU-TEST-001", capturedItem.ItemNumber);
        Assert.Equal(5.00m, capturedItem.CostPrice);
        Assert.Equal(12.50m, capturedItem.UnitPrice);
    }
}

public class TaxCalculationServiceTests
{
    [Fact]
    public async Task CalculateTaxes_ItemWithNoTaxCategory_UsesFallbackDefaultTax()
    {
        // Arrange
        var taxCategoryRepoMock = new Mock<IRepository<TaxCategory>>();
        var service = new Services.TaxCalculationService(taxCategoryRepoMock.Object);

        var sale = new Sale
        {
            SaleItems = new List<SaleItem>
            {
                new SaleItem
                {
                    Quantity = 1,
                    UnitPrice = 100.00m,
                    DiscountPercent = 0,
                    Item = new Item { TaxCategoryId = null } // No tax category
                }
            }
        };

        // Act
        var taxes = await service.CalculateTaxesAsync(sale, CancellationToken.None);

        // Assert: fallback 8% tax
        Assert.Single(taxes);
        Assert.Equal("Default Sales Tax", taxes[0].TaxName);
        Assert.Equal(8.00m, taxes[0].Rate);
        Assert.Equal(8.00m, taxes[0].Amount); // 8% of $100
    }
}
