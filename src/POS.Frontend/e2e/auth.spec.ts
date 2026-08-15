import { test, expect } from '@playwright/test';

test.describe('Authentication', () => {
  test('should login successfully and redirect to admin', async ({ page }) => {
    await page.goto('/login');
    
    // Check elements
    await expect(page.getByRole('heading', { name: 'NETPOS' })).toBeVisible();
    await expect(page.getByLabel('Email')).toBeVisible();
    
    // Fill credentials
    await page.getByLabel('Email').fill('admin@example.com');
    await page.getByLabel('Password').fill('password123'); // Fake password, backend logic hardcoded fake JWT for any correct creds
    
    // Submit
    await page.getByRole('button', { name: 'Sign In' }).click();
    
    // Verify redirect to admin
    await expect(page).toHaveURL(/.*\/admin\/items/);
    await expect(page.getByRole('heading', { name: 'Items' })).toBeVisible();
  });

  test('should show error for invalid credentials', async ({ page }) => {
    await page.goto('/login');
    
    // Fill wrong credentials
    await page.getByLabel('Email').fill('admin@example.com');
    await page.getByLabel('Password').fill('wrongpassword'); // Specifically triggers failure in backend
    
    await page.getByRole('button', { name: 'Sign In' }).click();
    
    // Verify error
    await expect(page.getByText('Invalid credentials')).toBeVisible();
  });
});
