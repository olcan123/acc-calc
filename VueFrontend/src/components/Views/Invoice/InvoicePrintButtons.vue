<template>
  <!-- Yazdırma düğmesi (yazdırma sırasında gizlenir) -->
  <div class="print:hidden text-center py-4">
    <button 
      @click="handlePrint" 
      class="bg-blue-600 hover:bg-blue-700 text-white px-6 py-2 rounded-lg mr-4 transition-colors duration-200"
    >
      🖨️ Yazdır
    </button>
    <button 
      @click="handleGoBack"
      class="bg-gray-600 hover:bg-gray-700 text-white px-6 py-2 rounded-lg transition-colors duration-200"
    >
      ← Geri Dön
    </button>
  </div>
</template>

<script setup>
import { useRouter } from 'vue-router'

const router = useRouter()

// Emits
const emit = defineEmits(['print', 'goBack'])

// Methods
const handlePrint = () => {
  emit('print')
  window.print()
}

const handleGoBack = () => {
  emit('goBack')
  router.go(-1)
}
</script>