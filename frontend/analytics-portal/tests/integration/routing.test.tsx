import { render, screen } from "@testing-library/react";
import { RouterProvider, createMemoryRouter } from "react-router";
import { describe, expect, it } from "vitest";

import { DashboardPage } from "../../src/features/dashboard";

describe("routing", () => {
  it("renders the dashboard page for the root path", () => {
    const router = createMemoryRouter([{ path: "/", element: <DashboardPage /> }], {
      initialEntries: ["/"],
    });

    render(<RouterProvider router={router} />);

    expect(screen.getByText("Portal analítico")).toBeTruthy();
  });
});
