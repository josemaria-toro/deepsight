import { expect, test } from "@playwright/test";

test("the home page shows the portal title", async ({ page }) => {
  await page.goto("/");

  await expect(page.getByRole("heading", { name: "DeepSight" })).toBeVisible();
});
