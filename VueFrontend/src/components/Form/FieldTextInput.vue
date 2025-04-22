<template>
  <div>
    <label
      v-if="labelName"
      :for="fieldName"
      :class="[
        'block mb-2 text-sm font-medium',
        errorMessage
          ? 'text-red-700 dark:text-red-500'
          : value
          ? 'text-green-700 dark:text-green-500'
          : 'text-gray-700 dark:text-gray-200',
      ]"
    >
      {{ labelName }}
    </label>

    <input
      type="text"
      :id="fieldName"
      :placeholder="placeholderName"
      v-model="value"
      :class="[
        'block w-full p-2.5 text-sm rounded-lg focus:ring-2 focus:outline-none',
        errorMessage
          ? 'bg-red-50 border border-red-500 text-red-900 placeholder-red-700 dark:bg-gray-700 focus:ring-red-500 focus:border-red-500 dark:text-red-500 dark:placeholder-red-500 dark:border-red-500'
          : value
          ? 'bg-green-50 border border-green-500 text-green-900 placeholder-green-700 dark:bg-gray-700 focus:ring-green-500 focus:border-green-500 dark:text-green-400 dark:placeholder-green-500 dark:border-green-500'
          : 'bg-gray-50 border border-gray-300 text-gray-900 placeholder-gray-400 dark:bg-gray-800 focus:ring-blue-500 focus:border-blue-500 dark:text-white dark:placeholder-gray-400 dark:border-gray-600',
      ]"
    />

    <p v-if="errorMessage" class="mt-2 text-sm text-red-600 dark:text-red-500">
      <span class="font-medium">Oh, snapp!</span> {{ errorMessage }}
    </p>
    <p v-else-if="value" class="mt-2 text-sm text-green-600 dark:text-green-500">
      <span class="font-medium">Well done!</span> Looks good.
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
  placeholderName: {
    type: String,
    default: "",
  },
});

const { fieldName } = toRefs(props);
const { value, errorMessage } = useField(fieldName.value);
</script>
