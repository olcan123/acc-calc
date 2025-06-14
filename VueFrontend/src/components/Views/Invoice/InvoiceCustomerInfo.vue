<template>
  <div class="mb-6">
    <div class="bg-blue-50 p-4 rounded-lg border-l-4 border-blue-400">
      <h3 class="text-sm font-semibold text-gray-800 mb-2 border-b border-gray-200 pb-1">
        Fatura Edilecek Adres
      </h3>
      <div class="text-xs space-y-1">
        <p class="font-semibold">{{ client.name }}</p>
        <p>{{ client.address }}</p>
        <p>{{ client.city }} {{ client.postalCode }}</p>
        <p>Tel: {{ client.phone }}</p>
        <p>Email: {{ client.email }}</p>
        <p v-if="client.taxNumber">Vergi No: {{ client.taxNumber }}</p>
      </div>
    </div>
  </div>
</template>

<script setup>
// Props
defineProps({
  client: {
    type: Object,
    required: true
  }
})
</script>