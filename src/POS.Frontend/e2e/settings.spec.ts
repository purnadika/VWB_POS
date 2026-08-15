import { test, expect } from '@playwright/test';

test.describe('Settings CRUD', () => {
  test.beforeEach(async ({ page }) => {
    await page.goto('/login');
    await page.getByLabel('Email').fill('admin@example.com');
    await page.getByLabel('Password').fill('password123');
    await page.getByRole('button', { name: 'Sign In' }).click();
    await page.waitForURL(/.*\/admin\/.*/);
  });
  test('should create, read, update, and delete a setting', async ({ page }) => {
    const uniqueId = Date.now();
    const key = `SETTING-${uniqueId}`;

    await page.goto('/admin/settings');
    await expect(page.getByRole('heading', { name: 'Settings' })).toBeVisible();

    await page.getByRole('button', { name: 'New Setting' }).click();
    
    await page.getByLabel('Key').fill(key);
    await page.getByLabel('Value').fill('Test Value');
    
    await page.getByRole('button', { name: 'Save' }).click();

    const row = page.locator('tr', { hasText: key }).first();
    await expect(row).toBeVisible();
    await expect(row.getByText('Test Value', { exact: true })).toBeVisible();

    const editRow = page.locator('tr', { hasText: key }).first();
    await editRow.getByRole('button', { name: 'Edit' }).click();
    
    await page.getByLabel('Value').fill('Updated Value');
    await page.getByRole('button', { name: 'Save' }).click();

    await expect(page.locator('tr', { hasText: key }).first().getByText('Updated Value', { exact: true })).toBeVisible();

    page.on('dialog', dialog => dialog.accept());
    
    await page.locator('tr', { hasText: key }).first().getByRole('button', { name: 'Delete' }).click();

    await expect(page.getByText(key)).not.toBeVisible();
  });
});

