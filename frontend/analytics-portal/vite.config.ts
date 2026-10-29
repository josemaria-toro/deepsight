import { defineConfig } from "vitest/config";

// El JSX se transforma con el transformador integrado de Vite (según `jsx` de tsconfig.json), sin plugins no admitidos.
export default defineConfig({
  server: { port: 5173 },
  test: {
    environment: "jsdom",
    include: ["src/**/*.test.{ts,tsx}", "tests/integration/**/*.test.{ts,tsx}"],
    coverage: {
      provider: "v8",
      include: ["src/**/*.{ts,tsx}"],
      exclude: ["src/**/*.test.{ts,tsx}", "src/app/main.tsx", "src/vite-env.d.ts"],
      thresholds: { lines: 80, functions: 80, branches: 80, statements: 80 },
    },
  },
});
