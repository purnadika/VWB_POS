using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace POS.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddAuditTrailsAndSoftDeletes : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Deleted",
                table: "Suppliers");

            migrationBuilder.DropColumn(
                name: "Deleted",
                table: "Employees");

            migrationBuilder.DropColumn(
                name: "Deleted",
                table: "Customers");

            migrationBuilder.AddColumn<string>(
                name: "CreatedBy",
                table: "TaxRates",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DeletedAt",
                table: "TaxRates",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DeletedBy",
                table: "TaxRates",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UpdatedBy",
                table: "TaxRates",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CreatedBy",
                table: "TaxCategories",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DeletedAt",
                table: "TaxCategories",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DeletedBy",
                table: "TaxCategories",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UpdatedBy",
                table: "TaxCategories",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CreatedBy",
                table: "StockLocations",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DeletedAt",
                table: "StockLocations",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DeletedBy",
                table: "StockLocations",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UpdatedBy",
                table: "StockLocations",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CreatedBy",
                table: "SaleTaxes",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "Deleted",
                table: "SaleTaxes",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "DeletedAt",
                table: "SaleTaxes",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DeletedBy",
                table: "SaleTaxes",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UpdatedBy",
                table: "SaleTaxes",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CreatedBy",
                table: "Sales",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "Deleted",
                table: "Sales",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "DeletedAt",
                table: "Sales",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DeletedBy",
                table: "Sales",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UpdatedBy",
                table: "Sales",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CreatedBy",
                table: "SalePayments",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "Deleted",
                table: "SalePayments",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "DeletedAt",
                table: "SalePayments",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DeletedBy",
                table: "SalePayments",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UpdatedBy",
                table: "SalePayments",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CreatedBy",
                table: "SaleItems",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "Deleted",
                table: "SaleItems",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "DeletedAt",
                table: "SaleItems",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DeletedBy",
                table: "SaleItems",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UpdatedBy",
                table: "SaleItems",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CreatedBy",
                table: "Receivings",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "Deleted",
                table: "Receivings",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "DeletedAt",
                table: "Receivings",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DeletedBy",
                table: "Receivings",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UpdatedBy",
                table: "Receivings",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CreatedBy",
                table: "ReceivingItems",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "Deleted",
                table: "ReceivingItems",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "DeletedAt",
                table: "ReceivingItems",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DeletedBy",
                table: "ReceivingItems",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UpdatedBy",
                table: "ReceivingItems",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CreatedBy",
                table: "People",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "Deleted",
                table: "People",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "DeletedAt",
                table: "People",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DeletedBy",
                table: "People",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UpdatedBy",
                table: "People",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CreatedBy",
                table: "Items",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DeletedAt",
                table: "Items",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DeletedBy",
                table: "Items",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UpdatedBy",
                table: "Items",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CreatedBy",
                table: "ItemQuantities",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "Deleted",
                table: "ItemQuantities",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "DeletedAt",
                table: "ItemQuantities",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DeletedBy",
                table: "ItemQuantities",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UpdatedBy",
                table: "ItemQuantities",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CreatedBy",
                table: "ItemKits",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DeletedAt",
                table: "ItemKits",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DeletedBy",
                table: "ItemKits",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UpdatedBy",
                table: "ItemKits",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CreatedBy",
                table: "ItemKitItems",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "Deleted",
                table: "ItemKitItems",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "DeletedAt",
                table: "ItemKitItems",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DeletedBy",
                table: "ItemKitItems",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UpdatedBy",
                table: "ItemKitItems",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CreatedBy",
                table: "ItemAttributes",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DeletedAt",
                table: "ItemAttributes",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DeletedBy",
                table: "ItemAttributes",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UpdatedBy",
                table: "ItemAttributes",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CreatedBy",
                table: "ItemAttributeLinks",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "Deleted",
                table: "ItemAttributeLinks",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "DeletedAt",
                table: "ItemAttributeLinks",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DeletedBy",
                table: "ItemAttributeLinks",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UpdatedBy",
                table: "ItemAttributeLinks",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CreatedBy",
                table: "InventoryTransactions",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "Deleted",
                table: "InventoryTransactions",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "DeletedAt",
                table: "InventoryTransactions",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DeletedBy",
                table: "InventoryTransactions",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UpdatedBy",
                table: "InventoryTransactions",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CreatedBy",
                table: "Giftcards",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DeletedAt",
                table: "Giftcards",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DeletedBy",
                table: "Giftcards",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UpdatedBy",
                table: "Giftcards",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CreatedBy",
                table: "Expenses",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "Deleted",
                table: "Expenses",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "DeletedAt",
                table: "Expenses",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DeletedBy",
                table: "Expenses",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UpdatedBy",
                table: "Expenses",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CreatedBy",
                table: "ExpenseCategories",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DeletedAt",
                table: "ExpenseCategories",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DeletedBy",
                table: "ExpenseCategories",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UpdatedBy",
                table: "ExpenseCategories",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CreatedBy",
                table: "DinnerTables",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DeletedAt",
                table: "DinnerTables",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DeletedBy",
                table: "DinnerTables",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UpdatedBy",
                table: "DinnerTables",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CreatedBy",
                table: "Cashups",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "Deleted",
                table: "Cashups",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "DeletedAt",
                table: "Cashups",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DeletedBy",
                table: "Cashups",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UpdatedBy",
                table: "Cashups",
                type: "text",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CreatedBy",
                table: "TaxRates");

            migrationBuilder.DropColumn(
                name: "DeletedAt",
                table: "TaxRates");

            migrationBuilder.DropColumn(
                name: "DeletedBy",
                table: "TaxRates");

            migrationBuilder.DropColumn(
                name: "UpdatedBy",
                table: "TaxRates");

            migrationBuilder.DropColumn(
                name: "CreatedBy",
                table: "TaxCategories");

            migrationBuilder.DropColumn(
                name: "DeletedAt",
                table: "TaxCategories");

            migrationBuilder.DropColumn(
                name: "DeletedBy",
                table: "TaxCategories");

            migrationBuilder.DropColumn(
                name: "UpdatedBy",
                table: "TaxCategories");

            migrationBuilder.DropColumn(
                name: "CreatedBy",
                table: "StockLocations");

            migrationBuilder.DropColumn(
                name: "DeletedAt",
                table: "StockLocations");

            migrationBuilder.DropColumn(
                name: "DeletedBy",
                table: "StockLocations");

            migrationBuilder.DropColumn(
                name: "UpdatedBy",
                table: "StockLocations");

            migrationBuilder.DropColumn(
                name: "CreatedBy",
                table: "SaleTaxes");

            migrationBuilder.DropColumn(
                name: "Deleted",
                table: "SaleTaxes");

            migrationBuilder.DropColumn(
                name: "DeletedAt",
                table: "SaleTaxes");

            migrationBuilder.DropColumn(
                name: "DeletedBy",
                table: "SaleTaxes");

            migrationBuilder.DropColumn(
                name: "UpdatedBy",
                table: "SaleTaxes");

            migrationBuilder.DropColumn(
                name: "CreatedBy",
                table: "Sales");

            migrationBuilder.DropColumn(
                name: "Deleted",
                table: "Sales");

            migrationBuilder.DropColumn(
                name: "DeletedAt",
                table: "Sales");

            migrationBuilder.DropColumn(
                name: "DeletedBy",
                table: "Sales");

            migrationBuilder.DropColumn(
                name: "UpdatedBy",
                table: "Sales");

            migrationBuilder.DropColumn(
                name: "CreatedBy",
                table: "SalePayments");

            migrationBuilder.DropColumn(
                name: "Deleted",
                table: "SalePayments");

            migrationBuilder.DropColumn(
                name: "DeletedAt",
                table: "SalePayments");

            migrationBuilder.DropColumn(
                name: "DeletedBy",
                table: "SalePayments");

            migrationBuilder.DropColumn(
                name: "UpdatedBy",
                table: "SalePayments");

            migrationBuilder.DropColumn(
                name: "CreatedBy",
                table: "SaleItems");

            migrationBuilder.DropColumn(
                name: "Deleted",
                table: "SaleItems");

            migrationBuilder.DropColumn(
                name: "DeletedAt",
                table: "SaleItems");

            migrationBuilder.DropColumn(
                name: "DeletedBy",
                table: "SaleItems");

            migrationBuilder.DropColumn(
                name: "UpdatedBy",
                table: "SaleItems");

            migrationBuilder.DropColumn(
                name: "CreatedBy",
                table: "Receivings");

            migrationBuilder.DropColumn(
                name: "Deleted",
                table: "Receivings");

            migrationBuilder.DropColumn(
                name: "DeletedAt",
                table: "Receivings");

            migrationBuilder.DropColumn(
                name: "DeletedBy",
                table: "Receivings");

            migrationBuilder.DropColumn(
                name: "UpdatedBy",
                table: "Receivings");

            migrationBuilder.DropColumn(
                name: "CreatedBy",
                table: "ReceivingItems");

            migrationBuilder.DropColumn(
                name: "Deleted",
                table: "ReceivingItems");

            migrationBuilder.DropColumn(
                name: "DeletedAt",
                table: "ReceivingItems");

            migrationBuilder.DropColumn(
                name: "DeletedBy",
                table: "ReceivingItems");

            migrationBuilder.DropColumn(
                name: "UpdatedBy",
                table: "ReceivingItems");

            migrationBuilder.DropColumn(
                name: "CreatedBy",
                table: "People");

            migrationBuilder.DropColumn(
                name: "Deleted",
                table: "People");

            migrationBuilder.DropColumn(
                name: "DeletedAt",
                table: "People");

            migrationBuilder.DropColumn(
                name: "DeletedBy",
                table: "People");

            migrationBuilder.DropColumn(
                name: "UpdatedBy",
                table: "People");

            migrationBuilder.DropColumn(
                name: "CreatedBy",
                table: "Items");

            migrationBuilder.DropColumn(
                name: "DeletedAt",
                table: "Items");

            migrationBuilder.DropColumn(
                name: "DeletedBy",
                table: "Items");

            migrationBuilder.DropColumn(
                name: "UpdatedBy",
                table: "Items");

            migrationBuilder.DropColumn(
                name: "CreatedBy",
                table: "ItemQuantities");

            migrationBuilder.DropColumn(
                name: "Deleted",
                table: "ItemQuantities");

            migrationBuilder.DropColumn(
                name: "DeletedAt",
                table: "ItemQuantities");

            migrationBuilder.DropColumn(
                name: "DeletedBy",
                table: "ItemQuantities");

            migrationBuilder.DropColumn(
                name: "UpdatedBy",
                table: "ItemQuantities");

            migrationBuilder.DropColumn(
                name: "CreatedBy",
                table: "ItemKits");

            migrationBuilder.DropColumn(
                name: "DeletedAt",
                table: "ItemKits");

            migrationBuilder.DropColumn(
                name: "DeletedBy",
                table: "ItemKits");

            migrationBuilder.DropColumn(
                name: "UpdatedBy",
                table: "ItemKits");

            migrationBuilder.DropColumn(
                name: "CreatedBy",
                table: "ItemKitItems");

            migrationBuilder.DropColumn(
                name: "Deleted",
                table: "ItemKitItems");

            migrationBuilder.DropColumn(
                name: "DeletedAt",
                table: "ItemKitItems");

            migrationBuilder.DropColumn(
                name: "DeletedBy",
                table: "ItemKitItems");

            migrationBuilder.DropColumn(
                name: "UpdatedBy",
                table: "ItemKitItems");

            migrationBuilder.DropColumn(
                name: "CreatedBy",
                table: "ItemAttributes");

            migrationBuilder.DropColumn(
                name: "DeletedAt",
                table: "ItemAttributes");

            migrationBuilder.DropColumn(
                name: "DeletedBy",
                table: "ItemAttributes");

            migrationBuilder.DropColumn(
                name: "UpdatedBy",
                table: "ItemAttributes");

            migrationBuilder.DropColumn(
                name: "CreatedBy",
                table: "ItemAttributeLinks");

            migrationBuilder.DropColumn(
                name: "Deleted",
                table: "ItemAttributeLinks");

            migrationBuilder.DropColumn(
                name: "DeletedAt",
                table: "ItemAttributeLinks");

            migrationBuilder.DropColumn(
                name: "DeletedBy",
                table: "ItemAttributeLinks");

            migrationBuilder.DropColumn(
                name: "UpdatedBy",
                table: "ItemAttributeLinks");

            migrationBuilder.DropColumn(
                name: "CreatedBy",
                table: "InventoryTransactions");

            migrationBuilder.DropColumn(
                name: "Deleted",
                table: "InventoryTransactions");

            migrationBuilder.DropColumn(
                name: "DeletedAt",
                table: "InventoryTransactions");

            migrationBuilder.DropColumn(
                name: "DeletedBy",
                table: "InventoryTransactions");

            migrationBuilder.DropColumn(
                name: "UpdatedBy",
                table: "InventoryTransactions");

            migrationBuilder.DropColumn(
                name: "CreatedBy",
                table: "Giftcards");

            migrationBuilder.DropColumn(
                name: "DeletedAt",
                table: "Giftcards");

            migrationBuilder.DropColumn(
                name: "DeletedBy",
                table: "Giftcards");

            migrationBuilder.DropColumn(
                name: "UpdatedBy",
                table: "Giftcards");

            migrationBuilder.DropColumn(
                name: "CreatedBy",
                table: "Expenses");

            migrationBuilder.DropColumn(
                name: "Deleted",
                table: "Expenses");

            migrationBuilder.DropColumn(
                name: "DeletedAt",
                table: "Expenses");

            migrationBuilder.DropColumn(
                name: "DeletedBy",
                table: "Expenses");

            migrationBuilder.DropColumn(
                name: "UpdatedBy",
                table: "Expenses");

            migrationBuilder.DropColumn(
                name: "CreatedBy",
                table: "ExpenseCategories");

            migrationBuilder.DropColumn(
                name: "DeletedAt",
                table: "ExpenseCategories");

            migrationBuilder.DropColumn(
                name: "DeletedBy",
                table: "ExpenseCategories");

            migrationBuilder.DropColumn(
                name: "UpdatedBy",
                table: "ExpenseCategories");

            migrationBuilder.DropColumn(
                name: "CreatedBy",
                table: "DinnerTables");

            migrationBuilder.DropColumn(
                name: "DeletedAt",
                table: "DinnerTables");

            migrationBuilder.DropColumn(
                name: "DeletedBy",
                table: "DinnerTables");

            migrationBuilder.DropColumn(
                name: "UpdatedBy",
                table: "DinnerTables");

            migrationBuilder.DropColumn(
                name: "CreatedBy",
                table: "Cashups");

            migrationBuilder.DropColumn(
                name: "Deleted",
                table: "Cashups");

            migrationBuilder.DropColumn(
                name: "DeletedAt",
                table: "Cashups");

            migrationBuilder.DropColumn(
                name: "DeletedBy",
                table: "Cashups");

            migrationBuilder.DropColumn(
                name: "UpdatedBy",
                table: "Cashups");

            migrationBuilder.AddColumn<bool>(
                name: "Deleted",
                table: "Suppliers",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "Deleted",
                table: "Employees",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "Deleted",
                table: "Customers",
                type: "boolean",
                nullable: false,
                defaultValue: false);
        }
    }
}
