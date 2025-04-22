<template>
  <h1 class="text-3xl font-bold text-gray-900 dark:text-white">Home</h1>

  <form class="w-full min-h-screen mx-auto bg-white dark:bg-gray-900 space-y-6" @submit="onSubmit">
    <FieldTextInput fieldName="first_name" labelName="First Name" placeholderName="John" />

    <div>
      <button type="submit">Kelimeyi al</button>
    </div>
  </form>
</template>

<script lang="ts" setup>
import { useForm } from "vee-validate";
import { toTypedSchema } from "@vee-validate/zod";
import { z } from "zod";
import FieldTextInput from "@/components/Form/FieldTextInput.vue";

const schema = toTypedSchema(
  z.object({
    first_name: z.string().min(1, "First name is required"),
  })
);

const { handleSubmit } = useForm({
  validationSchema: schema,
  initialValues: {
    first_name: "",
  },
});

const onSubmit = handleSubmit((values) => {
  console.log(values);
});
</script>

<style></style>
