<template>
  <FieldTextInput
    fieldName="name"
    labelName="Bilgi Adı"
    placeholderName="Örn. Telefon, Email"
  />
  <FieldTextInput
    fieldName="value"
    labelName="Değer"
    placeholderName="Örn: +90 555 555 55 55"
  />
  <FieldTextInput
    fieldName="description"
    labelName="Açıklama"
    placeholderName="Opsiyonel açıklama"
  />
  <div class="flex items-center gap-4">
    <FieldCheckbox fieldName="isActive" labelName="Aktif" />
    <FieldCheckbox fieldName="isPrimary" labelName="Ana" />
  </div>
</template>

<script setup>
import FieldTextInput from "@/components/Form/FieldTextInput.vue";
import FieldCheckbox from "@/components/Form/FieldCheckbox.vue";
</script>

<style scoped>
/* Your styles here */
</style>
