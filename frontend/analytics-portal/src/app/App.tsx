import { RouterProvider } from "react-router";

import { router } from "./router";

/** Componente raíz: compone los proveedores globales y las rutas. */
export function App() {
  return <RouterProvider router={router} />;
}
