<template>
  <div>
    <label
      v-if="showLabel && labelName"
      :for="fieldName"
      class="block mb-1 text-xs font-medium text-gray-700 dark:text-gray-300"
    >
      {{ labelName }}
    </label>
    <select
      :id="fieldName"
      v-model="value"
      @change="handleChange"
      :disabled="disabled"
      class="block w-full p-1.5 text-xs text-gray-900 border border-gray-300 rounded-md focus:ring-blue-500 focus:border-blue-500 focus:outline-none dark:bg-gray-700 dark:border-gray-600 dark:text-white dark:focus:ring-blue-500 dark:focus:border-blue-500 disabled:bg-gray-100 disabled:text-gray-500 disabled:cursor-not-allowed dark:disabled:bg-gray-600 dark:disabled:text-gray-400"
    >
      <option disabled value="">{{ placeholderName || "Seçiniz" }}</option>
      <option
        v-for="option in options"
        :key="option.value"
        :value="option.value"
      >
        {{ option.label }}
      </option>
    </select>
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
  showLabel: {
    // Tablodaki :show-label="false" kullanımını desteklemek için eklendi
    type: Boolean,
    default: true,
  },
  placeholderName: {
    type: String,
    default: "Seçiniz", // Daha iyi bir varsayılan
  },
  options: {
    type: Array,
    default: () => [],
    // Seçeneklerin { value: ..., label: ... } formatında olması beklenir
  },
  disabled: {
    type: Boolean,
    default: false,
  },
});

const { fieldName } = toRefs(props);
// errorMessage burada kullanılmasa da VeeValidate tarafından yönetilmeye devam eder.
const { value, errorMessage } = useField(fieldName.value);

// Emit tanımı
const emit = defineEmits(["on-change"]);

// Change handler
const handleChange = (event) => {
  emit("on-change", event.target.value);
};
</script>
