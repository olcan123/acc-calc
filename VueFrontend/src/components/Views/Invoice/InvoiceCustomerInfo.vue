<template>
  <div class="mb-6">
    <div class="bg-blue-50 p-4 rounded-lg border-l-4 border-blue-400">      <h3
        class="text-base font-semibold text-gray-800 mb-2 border-b border-gray-200 pb-1"
      >
        Adresa e Faturimit
      </h3>
      <div class="text-sm space-y-1">
        <p class="font-semibold">{{ partner.name }}</p>
        <p v-if="partner.identityNumber">UID No: {{ partner.identityNumber }}</p>
        <p v-if="partner.vatNumber">Nr. TVSH: {{ partner.vatNumber }}</p>
        <p>{{partner.addressPartners[0].address.street }}</p>
        <p>
          {{ partner.addressPartners[0].address.city }},
          {{ partner.addressPartners[0].address.country }},
          {{ partner.addressPartners[0].address.zipCode }}
        </p>
      </div>
    </div>
  </div>
</template>

<script setup>
import { usePartnerStore } from "@/stores/partner.store";
import { storeToRefs } from "pinia";

// Store
const partnerStore = usePartnerStore();
const { partner } = storeToRefs(partnerStore);

</script>
