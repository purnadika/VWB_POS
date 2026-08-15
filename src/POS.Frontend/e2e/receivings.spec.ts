import { test, expect } from '@playwright/test';

test.describe('Receivings CRUD', () => {
  test.beforeEach(async ({ page }) => {
    await page.goto('/login');
    await page.getByLabel('Email').fill('admin@example.com');
    await page.getByLabel('Password').fill('password123');
    await page.getByRole('button', { name: 'Sign In' }).click();
    await page.waitForURL(/.*\/admin\/.*/);
  });
  test('should create, read, update, and delete a receiving', async ({ page }) => {
    const uniqueId = Date.now();
    const reference = `REF-${uniqueId}`;

    await page.goto('/admin/receivings');
    await expect(page.getByRole('heading', { name: 'Receivings' })).toBeVisible();

    await page.getByRole('button', { name: 'New Receiving' }).click();
    
    await page.getByLabel('Supplier ID').fill('4');
    await page.getByLabel('Employee ID').fill('2');
    await page.getByLabel('Payment Type').fill('Cash');
    await page.getByLabel('Reference').fill(reference);
    await page.getByLabel('Receiving Time').fill('2025-10-10');
    await page.getByLabel('Total ($)').fill('500');
    
    await page.getByRole('button', { name: 'Save' }).click();

    const row = page.locator('tr', { hasText: reference }).first();
    await expect(row).toBeVisible();
    await expect(row.getByText('Cash', { exact: true })).toBeVisible();

    const editRow = page.locator('tr', { hasText: reference }).first();
    await editRow.getByRole('button', { name: 'Edit' }).click();
    
    await page.getByLabel('Payment Type').fill('Card');
    await page.getByRole('button', { name: 'Save' }).click();

    await expect(page.locator('tr', { hasText: reference }).first().getByText('Card', { exact: true })).toBeVisible();

    page.on('dialog', dialog => dialog.accept());
    
    await page.locator('tr', { hasText: reference }).first().getByRole('button', { name: 'Delete' }).click();

    await expect(page.getByText(reference)).not.toBeVisible();
  });
});
