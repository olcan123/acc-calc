// plugins/primevue.js
import PrimeVue from "primevue/config";
import Lara from "@primeuix/themes/lara";
import ConfirmationService from 'primevue/confirmationservice'

export function setupPrimeVue(app) {
  app.use(PrimeVue, {
    theme: {
      preset: Lara,
      options: {
        cssLayer: {
          name: "primevue",
          order: "theme, base, primevue",
        },
      },
    },
  });

  app.use(ConfirmationService);

}
