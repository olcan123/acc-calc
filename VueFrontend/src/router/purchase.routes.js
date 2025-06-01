const CreatePurchaseView = () => import("@/views/Purchases/CreatePurchaseView.vue");


export default [
  {
    path: "/purchases",
    name: "Purchases",
    children: [
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
      }
    ]
  }
];
