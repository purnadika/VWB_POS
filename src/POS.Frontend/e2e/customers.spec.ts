import { test, expect } from '@playwright/test';

test.describe('Customers CRUD', () => {
  test.beforeEach(async ({ page }) => {
    await page.goto('/login');
    await page.getByLabel('Email').fill('admin@example.com');
    await page.getByLabel('Password').fill('password123');
    await page.getByRole('button', { name: 'Sign In' }).click();
    await page.waitForURL(/.*\/admin\/.*/);
  });
  test('should create, read, update, and delete a customer', async ({ page }) => {
    const uniqueId = Date.now();
    const email = `test-${uniqueId}@example.com`;

    // 1. Read (Navigate to Customers page)
    await page.goto('/admin/customers');
    await expect(page.getByRole('heading', { name: 'Customers' })).toBeVisible();

    // 2. Create a new Customer
    await page.getByRole('button', { name: 'New Customer' }).click();
    
    // Fill the modal form
    await page.getByLabel('First Name').fill('John');
    await page.getByLabel('Last Name').fill('Doe');
    await page.getByLabel('Email').fill(email);
    await page.getByLabel('Phone Number').fill('1234567890');
    await page.getByLabel('Company Name').fill('Acme Corp');
    
    await page.getByRole('button', { name: 'Save' }).click();

    // Verify it appears in the table
    const row = page.locator('tr', { hasText: email }).first();
    await expect(row).toBeVisible();
    await expect(row.getByText('Doe', { exact: true })).toBeVisible();

    // 3. Update the Customer
    const editRow = page.locator('tr', { hasText: email }).first();
    await editRow.getByRole('button', { name: 'Edit' }).click();
    
    // Modify Last Name
    await page.getByLabel('Last Name').fill('Doe Updated');
    await page.getByRole('button', { name: 'Save' }).click();

    // Verify update
    await expect(page.locator('tr', { hasText: email }).first().getByText('Doe Updated', { exact: true })).toBeVisible();

    // 4. Delete the Customer
    // Confirm delete in the prompt/dialog if any, assuming there is a confirmation
    page.on('dialog', dialog => dialog.accept());
    
    await page.locator('tr', { hasText: email }).first().getByRole('button', { name: 'Delete' }).click();

    // Verify deletion
    await expect(page.getByText(email)).not.toBeVisible();
  });
});

