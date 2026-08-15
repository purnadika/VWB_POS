import { test, expect } from '@playwright/test';

test.describe('Items CRUD', () => {
  test.beforeEach(async ({ page }) => {
    await page.goto('/login');
    await page.getByLabel('Email').fill('admin@example.com');
    await page.getByLabel('Password').fill('password123');
    await page.getByRole('button', { name: 'Sign In' }).click();
    await page.waitForURL(/.*\/admin\/.*/);
  });
  test('should create, read, update, and delete an item', async ({ page }) => {
    const uniqueId = Date.now();
    const itemName = `Test Product ${uniqueId}`;
    const itemNameUpdated = `${itemName} V2`;

    // 1. Read (Navigate to Items page)
    await page.goto('/admin/items');
    await expect(page.getByRole('heading', { name: 'Items' })).toBeVisible();

    // 2. Create a new Item
    await page.getByRole('button', { name: 'New Item' }).click();
    
    // Fill the modal form
    await page.getByLabel('Item Name').fill(itemName);
    await page.getByLabel('Category ID').fill('1');
    await page.getByLabel('Cost Price').fill('50.00');
    await page.getByLabel('Unit Price').fill('99.99');
    
    await page.getByRole('button', { name: 'Save' }).click();

    // Verify it appears in the table
    const row = page.locator('tr', { hasText: itemName }).first();
    await expect(row).toBeVisible();
    await expect(row.getByText('1', { exact: true })).toBeVisible();

    // 3. Update the Item
    await row.getByRole('button', { name: 'Edit' }).click();
    
    // Modify Item Name
    await page.getByLabel('Item Name').fill(itemNameUpdated);
    await page.getByRole('button', { name: 'Save' }).click();

    // Verify update
    const updatedRow = page.locator('tr', { hasText: itemNameUpdated }).first();
    await expect(updatedRow).toBeVisible();

    // 4. Delete the Item
    // Confirm delete in the prompt/dialog if any, assuming there is a confirmation
    page.on('dialog', dialog => dialog.accept());
    
    await updatedRow.getByRole('button', { name: 'Delete' }).click();

    // Verify deletion
    await expect(updatedRow).not.toBeVisible();
  });
});

