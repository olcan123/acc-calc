const TableUnitOfMeasureView = () => import("@/views/Products/UnitOfMeasures/TableUnitOfMeasureView.vue");
const CreateUnitOfMeasureView = () => import("@/views/Products/UnitOfMeasures/CreateUnitOfMeasureView.vue");
const UpdateUnitOfMeasureView = () => import("@/views/Products/UnitOfMeasures/UpdateUnitOfMeasureView.vue");

export default [
  {
    path: "unitofmeasures",
    name: "table-unit-of-measure",
    meta: { requiresAuth: false, breadcrumb: "unit-of-measures" },
    component: TableUnitOfMeasureView,
  },
  {
    path: "unitofmeasures/create",
    name: "create-unit-of-measure",
    meta: { requiresAuth: false, breadcrumb: "create-unit-of-measure" },
    component: CreateUnitOfMeasureView,
  },
  {
    path: "unitofmeasures/update/:id",
    name: "update-unit-of-measure",
    meta: { requiresAuth: false, breadcrumb: "update-unit-of-measure" },
    component: UpdateUnitOfMeasureView,
  },
];
