import { test, expect } from '@playwright/test';

test.describe('Authentication End-to-End', () => {
  test('should display login page and perform login', async ({ page }) => {
    // Navigate to the app login page
    await page.goto('/login');

    // Expect a title/tab to be visible
    await expect(page.locator('a', { hasText: 'Login' }).first()).toBeVisible();

    // Fill in the form using IDs from the HTML
    await page.fill('#identifier', 'nikhil11012');
    await page.fill('#password', 'Admin@123');

    // Click submit
    await page.click('button[type="submit"]');

    // Wait for the URL to change (meaning successful login redirect to dashboard/home)
    await page.waitForURL('**/', { timeout: 10000 }).catch(() => {});
    
    const url = page.url();
    expect(url).not.toContain('/login');
  });
});
