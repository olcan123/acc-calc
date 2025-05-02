const DefaultLayout = () => import("@/layouts/DefaultLayout.vue");
const AuthLayout = () => import("@/layouts/AuthLayout.vue");
import homeRoutes from "./home.routes";
import companyRoutes from "./company.routes";
import warehouseRoutes from "./warehouse.routes";
import bankRoutes from "./bank.routes";

const routes = [
  {
    path: "/",
    component: DefaultLayout,
    children: [...homeRoutes, ...companyRoutes, ...warehouseRoutes, ...bankRoutes],
  },
];

export default routes;
