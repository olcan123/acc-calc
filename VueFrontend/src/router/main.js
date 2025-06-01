const DefaultLayout = () => import("@/layouts/DefaultLayout.vue");
const AuthLayout = () => import("@/layouts/AuthLayout.vue");
import homeRoutes from "./home.routes";
import companyRoutes from "./company.routes";
import warehouseRoutes from "./warehouse.routes";
import bankRoutes from "./bank.routes";
import partnerRoutes from "./partner.routes";
import vatRoutes from "./vat.routes";
import unitOfMeasureRoutes from "./unit-of-measure.routes";
import categoryRoutes from "./category.routes";
import productRoutes from "./product.routes";
import currencyRoutes from "./currency.routes";
import accountRoutes from "./account.routes";
import purchaseRoutes from "./purchase.routes";

const routes = [  {
    path: "/",
    component: DefaultLayout,    children: [
      ...homeRoutes, 
      ...companyRoutes, 
      ...warehouseRoutes, 
      ...bankRoutes, 
      ...partnerRoutes,
      ...vatRoutes,
      ...unitOfMeasureRoutes,
      ...categoryRoutes,
      ...productRoutes,
      ...currencyRoutes,
      ...accountRoutes,
      ...purchaseRoutes
    ],
  },
];

export default routes;
