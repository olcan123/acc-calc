const CreatePurchaseView = () => import("@/views/Purchases/CreatePurchaseView.vue");
const UpdatePurchaseView = () => import("@/views/Purchases/UpdatePurchaseView.vue");
const TablePurchaseView = () => import("@/views/Purchases/TablePurchaseView.vue");


export default [
  {
    path: "/purchases",
    name: "Purchases",
    children: [
      {
        path: "",
        name: "TablePurchases",
        component: TablePurchaseView,
        meta: {
          title: "Satın Alma Faturaları"
        }
      },
      {
        path: "local/create",
        name: "CreateLocalPurchase",
        component: CreatePurchaseView,
        meta: {
          title: "Lokal Satın Alma Faturası Oluştur"
        }
      },
      {
        path: 'import/create',
        name: 'CreateImportPurchase',
        component: CreatePurchaseView,
        meta: {
          title: "İthalat Satın Alma Faturası Oluştur"
        }
      },
      {
        path: "local/edit/:id",
        name: "UpdateLocalPurchase",
        component: UpdatePurchaseView,
        meta: {
          title: "Lokal Satın Alma Faturası Düzenle"
        }
      },
      {
        path: 'import/edit/:id',
        name: 'UpdateImportPurchase',
        component: UpdatePurchaseView,
        meta: {
          title: "İthalat Satın Alma Faturası Düzenle"
        }
      }
    ]
  }
];
