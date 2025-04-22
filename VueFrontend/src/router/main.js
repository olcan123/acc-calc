const DefaultLayout = () => import("@/layouts/DefaultLayout.vue");
const AuthLayout = () => import("@/layouts/AuthLayout.vue");
import homeRoutes from "./home.routes";
import companyRoutes from "./company.routes";
import bankAccountRoutes from "./bank-account.routes";
import warehouseRoutes from "./warehouse.routes";

const routes = [
  {
    path: "/",
    component: DefaultLayout,
    children: [...homeRoutes, ...companyRoutes, ...bankAccountRoutes, ...warehouseRoutes],
  },
];

export default routes;
