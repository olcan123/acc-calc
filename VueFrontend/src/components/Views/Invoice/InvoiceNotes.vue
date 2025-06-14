<template>
  <div v-if="notes" class="mb-4">
    <h3 class="text-sm font-semibold text-gray-800 mb-2 border-b border-gray-200 pb-1">
      Notlar ve Koşullar
    </h3>
    <p class="text-xs text-gray-600 leading-relaxed">{{ notes }}</p>
  </div>
</template>

<script setup>
// Props
defineProps({
  notes: {
    type: String,
    default: ''
  }
})
</script>