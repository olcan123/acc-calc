<template>
  <div>
    <label
      v-if="labelName"
      :for="fieldName"
      class="block mb-1 text-xs font-medium text-gray-700 dark:text-gray-300"
    >
      {{ labelName }}
    </label>
    <input
      type="number"
      :id="fieldName"
      :placeholder="placeholderName"
      v-model="value"
      :min="min"
      :max="max"
      :step="step"
      class="block w-full p-1.5 text-xs text-gray-900 border border-gray-300 rounded-md focus:ring-blue-500 focus:border-blue-500 focus:outline-none dark:bg-gray-700 dark:border-gray-600 dark:placeholder-gray-400 dark:text-white dark:focus:ring-blue-500 dark:focus:border-blue-500"
    />
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
  min: {
    type: Number,
    default: undefined,
  },
  max: {
    type: Number,
    default: undefined,
  },
  step: {
    type: [String, Number], // Hem 'any' hem de sayısal değerleri kabul etmesi için
    default: "any",
  },
});

const { fieldName } = toRefs(props);
// errorMessage burada kullanılmasa da VeeValidate tarafından yönetilmeye devam eder.
const { value, errorMessage } = useField(fieldName.value);
</script>
