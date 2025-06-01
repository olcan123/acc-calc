<template>
  <div class="flex items-center gap-2">
    <input
      type="checkbox"
      :id="fieldName"
      v-model="value"
      class="mt-0.5 w-4 h-4 text-blue-600 bg-gray-100 border-gray-300 rounded focus:ring-blue-500 dark:focus:ring-blue-600 dark:ring-offset-gray-800 focus:ring-2 dark:bg-gray-700 dark:border-gray-600"
    />
    <label
      :for="fieldName"
      class="text-sm text-gray-700 dark:text-gray-300 select-none"
    >
      {{ labelName }}
    </label>
    </div>
</template>

<script setup>
import { toRefs } from "vue";
import { useField } from "vee-validate";

const props = defineProps({
  fieldName: {
    type: String,
    required: true,
  },
  labelName: {
    type: String,
    default: "",
  },
});

const { fieldName } = toRefs(props);

// VeeValidate'den gelen 'value' onay kutusunun durumunu (true/false) tutar.
// 'errorMessage' bu bileşende kullanılmasa da VeeValidate tarafından yönetilmeye devam eder.
const { value, errorMessage } = useField(fieldName.value, undefined, {
  // Onay kutuları için varsayılan olarak bir 'value' prop'u beklenir,
  // bu nedenle boolean bir değerle çalışmak için initialValue ayarlamak gerekebilir
  // veya VeeValidate yapılandırmanızda onay kutularını nasıl ele aldığınıza bağlıdır.
  // Genellikle v-model doğrudan boolean bir ref ile iyi çalışır.
});
</script>