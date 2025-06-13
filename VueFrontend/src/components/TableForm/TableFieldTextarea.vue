<template>
  <div>
    <label
      v-if="labelName"
      :for="fieldName"
      class="block mb-1 text-xs font-medium text-gray-700 dark:text-gray-300"
    >
      {{ labelName }}
    </label>    <textarea
      :id="fieldName"
      :placeholder="placeholderName"
      v-model="value"
      :rows="rows"
      class="block w-full p-1.5 text-xs text-gray-900 border border-gray-300 rounded-md focus:ring-blue-500 focus:border-blue-500 focus:outline-none dark:bg-gray-700 dark:border-gray-600 dark:placeholder-gray-400 dark:text-white dark:focus:ring-blue-500 dark:focus:border-blue-500 resize-y min-h-[2rem] max-h-[8rem]"
    ></textarea>
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
  placeholderName: {
    type: String,
    default: "",
  },
  rows: {
    type: Number,
    default: 2,
  },
});

const { fieldName } = toRefs(props);

// 'useField' hook'u alanın değerini ('value') bağlamak için kullanılır.
// 'errorMessage' bu bileşende doğrudan kullanılmasa da, VeeValidate'in genel hata yönetimi için hala mevcuttur.
const { value, errorMessage } = useField(fieldName.value);
</script>
