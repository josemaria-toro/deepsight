import { createBrowserRouter } from "react-router";

import { DashboardPage } from "../features/dashboard";

/** Rutas de la aplicación. Cada ruta apunta a una página de una funcionalidad. */
export const router = createBrowserRouter([{ path: "/", element: <DashboardPage /> }]);
