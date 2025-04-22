// plugins/index.js

import { pinia, setupPinia } from './pinia'
import { setupRouter } from './router'
import { setupToast } from './toast'
import { setupPrimeVue } from './primevue'

export function setupPlugins(app) {
  setupPrimeVue(app)
  setupToast(app)
  setupPinia(app)
  setupRouter(app, pinia)
}

export { pinia } // dışa aktarıyoruz çünkü bazı durumlarda lazım olabilir
