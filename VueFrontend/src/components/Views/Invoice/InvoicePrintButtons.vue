<template>
  <!-- Yazdırma düğmesi (yazdırma sırasında gizlenir) -->
  <div class="print:hidden text-center py-4">    <button
      @click="handlePrint"
      :disabled="isDisabled"
      class="bg-blue-600 hover:bg-blue-700 text-white px-6 py-3 rounded-lg mr-4 transition-colors duration-200 disabled:bg-blue-300 disabled:cursor-not-allowed text-base"
    >
      🖨️ {{ isDisabled ? "Duke u Printuar..." : "Printo" }}
    </button>
    <button
      @click="handleGoBack"
      class="bg-gray-600 hover:bg-gray-700 text-white px-6 py-3 rounded-lg transition-colors duration-200 text-base"
    >
      ← Kthehu Prapa
    </button>
  </div>
</template>

<script setup>
import { ref } from "vue";
import { useRouter } from "vue-router";

const router = useRouter();
const isDisabled = ref(false);

// Emits
const emit = defineEmits(["print", "goBack"]);

// Methods
const handlePrint = () => {
  if (isDisabled.value) return;

  isDisabled.value = true;
  emit("print");

  // Re-enable button after 3 seconds
  setTimeout(() => {
    isDisabled.value = false;
  }, 3000);
};

const handleGoBack = () => {
  emit("goBack");
  router.go(-1);
};
</script>
