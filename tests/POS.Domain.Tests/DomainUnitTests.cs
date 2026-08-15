using POS.Domain.Entities;
using POS.Domain.Enums;
using Xunit;

namespace POS.Domain.Tests;

public class MoneyTests
{
    [Fact]
    public void Add_SameCurrency_ReturnsCorrectAmount()
    {
        var a = new ValueObjects.Money(10.00m, "USD");
        var b = new ValueObjects.Money(5.50m, "USD");
        var result = a.Add(b);
        Assert.Equal(15.50m, result.Amount);
    }

    [Fact]
    public void Add_DifferentCurrency_ThrowsInvalidOperationException()
    {
        var a = new ValueObjects.Money(10.00m, "USD");
        var b = new ValueObjects.Money(5.50m, "EUR");
        Assert.Throws<InvalidOperationException>(() => a.Add(b));
    }

    [Fact]
    public void Multiply_ReturnsScaledAmount()
    {
        var money = new ValueObjects.Money(20.00m, "USD");
        var result = money.Multiply(3);
        Assert.Equal(60.00m, result.Amount);
    }
}

public class SaleItemLineTotalTests
{
    [Fact]
    public void LineTotal_WithDiscount_CalculatesCorrectly()
    {
        // Arrange: 2 units at $100 each, with 10% discount
        var saleItem = new SaleItem
        {
            Quantity = 2,
            UnitPrice = 100.00m,
            DiscountPercent = 10.00m
        };

        // Act
        var lineTotal = saleItem.LineTotal;

        // Assert: 2 * 100 * (1 - 0.10) = 180.00
        Assert.Equal(180.00m, lineTotal);
    }

    [Fact]
    public void LineTotal_NoDiscount_CalculatesCorrectly()
    {
        var saleItem = new SaleItem
        {
            Quantity = 3,
            UnitPrice = 50.00m,
            DiscountPercent = 0.00m
        };

        Assert.Equal(150.00m, saleItem.LineTotal);
    }
}

public class ItemPermissionsTests
{
    [Fact]
    public void Employee_HasPermission_ReturnsTrueForGrantedModule()
    {
        var employee = new Employee
        {
            GrantedModules = new System.Collections.Generic.List<string> { "sales", "items" }
        };

        Assert.True(employee.HasPermission("sales"));
        Assert.True(employee.HasPermission("items"));
        Assert.False(employee.HasPermission("config"));
    }
}

public class ResultTests
{
    [Fact]
    public void Success_IsSuccessTrue_ErrorEmpty()
    {
        var result = Common.Result.Success();
        Assert.True(result.IsSuccess);
        Assert.False(result.IsFailure);
    }

    [Fact]
    public void Failure_IsFailureTrue_ErrorSet()
    {
        var result = Common.Result.Failure("Something went wrong");
        Assert.True(result.IsFailure);
        Assert.Equal("Something went wrong", result.Error);
    }

    [Fact]
    public void SuccessOfT_ValueAccessible()
    {
        var result = Common.Result.Success(42);
        Assert.True(result.IsSuccess);
        Assert.Equal(42, result.Value);
    }

    [Fact]
    public void FailureOfT_AccessingValue_ThrowsException()
    {
        var result = Common.Result.Failure<int>("Error");
        Assert.Throws<InvalidOperationException>(() => result.Value);
    }
}
