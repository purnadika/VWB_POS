import { test, expect } from '@playwright/test';

test.describe('Gift Cards CRUD', () => {
  test.beforeEach(async ({ page }) => {
    await page.goto('/login');
    await page.getByLabel('Email').fill('admin@example.com');
    await page.getByLabel('Password').fill('password123');
    await page.getByRole('button', { name: 'Sign In' }).click();
    await page.waitForURL(/.*\/admin\/.*/);
  });
  test('should create, read, update, and delete a gift card', async ({ page }) => {
    const uniqueId = Date.now();
    const cardNumber = `GC-${uniqueId}`;

    // 1. Read (Navigate to Gift Cards page)
    await page.goto('/admin/gift-cards');
    await expect(page.getByRole('heading', { name: 'GiftCards' })).toBeVisible();

    // 2. Create a new Gift Card
    await page.getByRole('button', { name: 'New GiftCard' }).click();
    
    // Fill the modal form
    await page.getByLabel('Card Number').fill(cardNumber);
    await page.getByLabel('Value ($)').fill('50');
    await page.getByLabel('Customer ID').fill('1');
    
    await page.getByRole('button', { name: 'Save' }).click();

    // Verify it appears in the table
    const row = page.locator('tr', { hasText: cardNumber }).first();
    await expect(row).toBeVisible();
    await expect(row.getByText('50', { exact: true })).toBeVisible();

    // 3. Update the Gift Card
    const editRow = page.locator('tr', { hasText: cardNumber }).first();
    await editRow.getByRole('button', { name: 'Edit' }).click();
    
    // Modify Value
    await page.getByLabel('Value ($)').fill('75');
    await page.getByRole('button', { name: 'Save' }).click();

    // Verify update
    await expect(page.locator('tr', { hasText: cardNumber }).first().getByText('75', { exact: true })).toBeVisible();

    // 4. Delete the Gift Card
    page.on('dialog', dialog => dialog.accept());
    
    await page.locator('tr', { hasText: cardNumber }).first().getByRole('button', { name: 'Delete' }).click();

    // Verify deletion
    await expect(page.getByText(cardNumber)).not.toBeVisible();
  });
});

