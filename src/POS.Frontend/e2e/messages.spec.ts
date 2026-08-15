import { test, expect } from '@playwright/test';

test.describe('Messages CRUD', () => {
  test.beforeEach(async ({ page }) => {
    await page.goto('/login');
    await page.getByLabel('Email').fill('admin@example.com');
    await page.getByLabel('Password').fill('password123');
    await page.getByRole('button', { name: 'Sign In' }).click();
    await page.waitForURL(/.*\/admin\/.*/);
  });
  test('should create, read, update, and delete a message', async ({ page }) => {
    const uniqueId = Date.now();
    const subject = `MSG-${uniqueId}`;

    await page.goto('/admin/messages');
    await expect(page.getByRole('heading', { name: 'Messages' })).toBeVisible();

    await page.getByRole('button', { name: 'New Message' }).click();
    
    await page.getByLabel('Sender ID').fill('2');
    await page.getByLabel('Receiver ID').fill('3');
    await page.getByLabel('Subject').fill(subject);
    await page.getByLabel('Body').fill('This is a test message');
    await page.getByLabel('Sent At').fill('2025-10-10');
    
    await page.getByRole('button', { name: 'Save' }).click();

    const row = page.locator('tr', { hasText: subject }).first();
    await expect(row).toBeVisible();
    await expect(row.getByText('This is a test message', { exact: true })).toBeVisible();

    const editRow = page.locator('tr', { hasText: subject }).first();
    await editRow.getByRole('button', { name: 'Edit' }).click();
    
    await page.getByLabel('Body').fill('Updated message');
    await page.getByRole('button', { name: 'Save' }).click();

    await expect(page.locator('tr', { hasText: subject }).first().getByText('Updated message', { exact: true })).toBeVisible();

    page.on('dialog', dialog => dialog.accept());
    
    await page.locator('tr', { hasText: subject }).first().getByRole('button', { name: 'Delete' }).click();

    await expect(page.getByText(subject)).not.toBeVisible();
  });
});
