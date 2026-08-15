import { test, expect } from '@playwright/test';

test.describe('Taxes CRUD', () => {
  test.beforeEach(async ({ page }) => {
    await page.goto('/login');
    await page.getByLabel('Email').fill('admin@example.com');
    await page.getByLabel('Password').fill('password123');
    await page.getByRole('button', { name: 'Sign In' }).click();
    await page.waitForURL(/.*\/admin\/.*/);
  });
  test('should create, read, update, and delete a tax rate', async ({ page }) => {
    const uniqueId = Date.now();
    const name = `TAX-${uniqueId}`;

    await page.goto('/admin/taxes');
    await expect(page.getByRole('heading', { name: 'Taxes' })).toBeVisible();

    await page.getByRole('button', { name: 'New Taxe' }).click();
    
    await page.getByLabel('Tax Name').fill(name);
    await page.getByLabel('Rate (%)').fill('10');
    await page.getByLabel('Tax Category ID').fill('1');
    
    await page.getByRole('button', { name: 'Save' }).click();

    const row = page.locator('tr', { hasText: name }).first();
    await expect(row).toBeVisible();
    await expect(row.getByText('10', { exact: true })).toBeVisible();

    const editRow = page.locator('tr', { hasText: name }).first();
    await editRow.getByRole('button', { name: 'Edit' }).click();
    
    await page.getByLabel('Rate (%)').fill('15');
    await page.getByRole('button', { name: 'Save' }).click();

    await expect(page.locator('tr', { hasText: name }).first().getByText('15', { exact: true })).toBeVisible();

    page.on('dialog', dialog => dialog.accept());
    
    await page.locator('tr', { hasText: name }).first().getByRole('button', { name: 'Delete' }).click();

    await expect(page.getByText(name)).not.toBeVisible();
  });
});

