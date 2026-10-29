# Ejemplos de uso

Ejemplo ejecutable del componente principal: monta `DashboardPage` en un enrutador en memoria. Se verifica en [`tests/integration/routing.test.tsx`](../tests/integration/routing.test.tsx).

```tsx
import { RouterProvider, createMemoryRouter } from "react-router";

import { DashboardPage } from "../src/features/dashboard";

const router = createMemoryRouter([{ path: "/", element: <DashboardPage /> }]);

export const Example = () => <RouterProvider router={router} />;
```
