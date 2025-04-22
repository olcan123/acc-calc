// plugins/toast.js
import Toast from 'vue-toastification'
import 'vue-toastification/dist/index.css'

const toastOptions = {
  timeout: 1500,
}

export function setupToast(app) {
  app.use(Toast, toastOptions)
}
