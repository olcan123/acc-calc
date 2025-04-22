// plugins/pinia.js
import { createPinia } from 'pinia'

export const pinia = createPinia()

export function setupPinia(app) {
  app.use(pinia)
}
