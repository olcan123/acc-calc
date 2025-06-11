const CreateSaleView = () => import("@/views/Sales/CreateSaleView.vue");
const UpdateSaleView = () => import("@/views/Sales/UpdateSaleView.vue");
const TableSaleView = () => import("@/views/Sales/TableSaleView.vue");

export default [
  {
    path: "/sales",
    name: "Sales",
    children: [
      {
        path: "",
        name: "table-sale",
        component: TableSaleView,
        meta: {
          title: "Satış Faturaları"
        }
      },
      {
        path: "local/create",
        name: "create-local-sale",
        component: CreateSaleView,
        meta: {
          title: "Lokal Satış Faturası Oluştur"
        }
      },
      {
        path: 'export/create',
        name: 'create-export-sale',
        component: CreateSaleView,
        meta: {
          title: "İhracat Satış Faturası Oluştur"
        }
      },
      {
        path: "local/edit/:id",
        name: "update-local-sale",
        component: UpdateSaleView,
        meta: {
          title: "Lokal Satış Faturası Düzenle"
        }
      },
      {
        path: 'export/edit/:id',
        name: 'update-export-sale',
        component: UpdateSaleView,
        meta: {
          title: "İhracat Satış Faturası Düzenle"
        }
      }
    ]
  }
];
