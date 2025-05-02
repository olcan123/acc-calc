<template>
    <div class="flex items-start gap-2">
      <input
        type="checkbox"
        :id="fieldName"
        v-model="value"
        :class="[
          'mt-1 w-4 h-4 rounded border',
          errorMessage
            ? 'text-red-600 bg-red-100 border-red-500 focus:ring-red-500 focus:border-red-500 dark:bg-gray-700 dark:border-red-500'
            : value
            ? 'text-green-600 bg-green-100 border-green-500 focus:ring-green-500 focus:border-green-500 dark:bg-gray-700 dark:border-green-500'
            : 'text-gray-600 bg-gray-50 border-gray-300 focus:ring-blue-500 focus:border-blue-500 dark:bg-gray-700 dark:border-gray-600',
        ]"
      />
      <label
        :for="fieldName"
        :class="[
          'text-sm select-none',
          errorMessage
            ? 'text-red-700 dark:text-red-500'
            : value
            ? 'text-green-700 dark:text-green-500'
            : 'text-gray-700 dark:text-gray-200',
        ]"
      >
        {{ labelName }}
      </label>
  
      <p v-if="errorMessage" class="ml-6 text-sm text-red-600 dark:text-red-500">
        <span class="font-medium">Oops!</span> {{ errorMessage }}
      </p>
      <p v-else-if="value" class="ml-6 text-sm text-green-600 dark:text-green-500">
        <span class="font-medium">Good!</span> Checked.
      </p>
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
  const { value, errorMessage } = useField(fieldName.value);
  </script>
  