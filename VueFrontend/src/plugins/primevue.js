// plugins/primevue.js
import PrimeVue from 'primevue/config'
import Lara from '@primeuix/themes/lara'

export function setupPrimeVue(app) {
  app.use(PrimeVue, {
    theme: {
      preset: Lara,
      options: {
        cssLayer: {
          name: 'primevue',
          order: 'theme, base, primevue',
        },
      },
    },
  })
}
