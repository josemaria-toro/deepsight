import { render, screen } from "@testing-library/react";
import { describe, expect, it } from "vitest";

import { DashboardPage } from "./index";

describe("DashboardPage", () => {
  it("shows the portal title", () => {
    render(<DashboardPage />);

    expect(screen.getByRole("heading", { name: "DeepSight" })).toBeTruthy();
  });
});
