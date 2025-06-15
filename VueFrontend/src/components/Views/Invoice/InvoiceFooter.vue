<template>  <div class="legal-info text-center text-sm text-gray-500 pt-2">
    <p>Kjo faturë është krijuar në mjedis elektronik dhe ka vlefshmëri ligjore.</p>
  </div>
</template>

<script setup>
// No props needed for this component
</script>

<style scoped>
.legal-info {
  margin-top: 10px;
}

/* Print stilleri */
@media print {  .legal-info {
    margin-top: 5px !important;
    font-size: 10px !important;
  }
}
</style>