// plugins/router.js
import router from '@/router'
import { markRaw } from 'vue'

export function setupRouter(app, pinia) {
  // Router'ı store'lara inject et
  pinia.use(({ store }) => {
    store.router = markRaw(router)
  })

  app.use(router)
}
