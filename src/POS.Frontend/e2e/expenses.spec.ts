import { test, expect } from '@playwright/test';

test.describe('Expenses CRUD', () => {
  test.beforeEach(async ({ page }) => {
    await page.goto('/login');
    await page.getByLabel('Email').fill('admin@example.com');
    await page.getByLabel('Password').fill('password123');
    await page.getByRole('button', { name: 'Sign In' }).click();
    await page.waitForURL(/.*\/admin\/.*/);
  });
  test('should create, read, update, and delete an expense', async ({ page }) => {
    const uniqueId = Date.now();
    const desc = `EXP-${uniqueId}`;

    await page.goto('/admin/expenses');
    await expect(page.getByRole('heading', { name: 'Expenses' })).toBeVisible();

    await page.getByRole('button', { name: 'New Expense' }).click();
    
    await page.getByLabel('Category ID').fill('1');
    await page.getByLabel('Amount ($)').fill('100');
    await page.getByLabel('Payment Type').fill('Cash');
    await page.getByLabel('Description').fill(desc);
    await page.getByLabel('Employee ID').fill('2');
    await page.getByLabel('Date').fill('2025-10-10');
    
    await page.getByRole('button', { name: 'Save' }).click();

    const row = page.locator('tr', { hasText: desc }).first();
    await expect(row).toBeVisible();
    await expect(row.getByText('100', { exact: true })).toBeVisible();

    const editRow = page.locator('tr', { hasText: desc }).first();
    await editRow.getByRole('button', { name: 'Edit' }).click();
    
    await page.getByLabel('Amount ($)').fill('200');
    await page.getByRole('button', { name: 'Save' }).click();

    await expect(page.locator('tr', { hasText: desc }).first().getByText('200', { exact: true })).toBeVisible();

    page.on('dialog', dialog => dialog.accept());
    
    await page.locator('tr', { hasText: desc }).first().getByRole('button', { name: 'Delete' }).click();

    await expect(page.getByText(desc)).not.toBeVisible();
  });
});
