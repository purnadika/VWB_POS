import { test, expect } from '@playwright/test';

test.describe('Employees CRUD', () => {
  test.beforeEach(async ({ page }) => {
    await page.goto('/login');
    await page.getByLabel('Email').fill('admin@example.com');
    await page.getByLabel('Password').fill('password123');
    await page.getByRole('button', { name: 'Sign In' }).click();
    await page.waitForURL(/.*\/admin\/.*/);
  });
  test('should create, read, update, and delete an employee', async ({ page }) => {
    const uniqueId = Date.now();
    const email = `test-${uniqueId}@example.com`;
    const username = `testuser_${uniqueId}`;

    // 1. Read (Navigate to Employees page)
    await page.goto('/admin/employees');
    await expect(page.getByRole('heading', { name: 'Employees' })).toBeVisible();

    // 2. Create a new Employee
    await page.getByRole('button', { name: 'New Employee' }).click();
    
    // Fill the modal form
    await page.getByLabel('First Name').fill('Jane');
    await page.getByLabel('Last Name').fill('Smith');
    await page.getByLabel('Email').fill(email);
    await page.getByLabel('Phone Number').fill('0987654321');
    await page.getByLabel('Username').fill(username);
    await page.getByLabel('Password').fill('password123');
    
    await page.getByRole('button', { name: 'Save' }).click();

    // Verify it appears in the table
    const row = page.locator('tr', { hasText: email }).first();
    await expect(row).toBeVisible();
    await expect(row.getByText('Smith', { exact: true })).toBeVisible();
    await expect(row.getByText(email)).toBeVisible();

    // 3. Update the Employee
    const editRow = page.locator('tr', { hasText: email }).first();
    await editRow.getByRole('button', { name: 'Edit' }).click();
    
    // Modify Last Name
    await page.getByLabel('Last Name').fill('Smith Updated');
    await page.getByRole('button', { name: 'Save' }).click();

    // Verify update
    await expect(page.locator('tr', { hasText: email }).first().getByText('Smith Updated', { exact: true })).toBeVisible();

    // 4. Delete the Employee
    // Confirm delete in the prompt/dialog
    page.on('dialog', dialog => dialog.accept());
    
    await page.locator('tr', { hasText: email }).first().getByRole('button', { name: 'Delete' }).click();

    // Verify deletion
    await expect(page.getByText(email)).not.toBeVisible();
  });
});

