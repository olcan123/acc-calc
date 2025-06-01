<template>
  <!-- Üst Başlık ve Kaydet Butonu -->
  <div
    class="flex flex-col sm:flex-row justify-between items-center mb-6 gap-4"
  >
    <h2 class="text-2xl font-semibold text-gray-800 dark:text-white">
      Yeni Hesap Oluştur
    </h2>

    <button
      type="submit"
      form="accountForm"
      :disabled="submitCount > 0 && !meta.valid"
      class="flex items-center justify-center gap-2 text-white font-medium rounded-lg text-sm px-5 py-2.5 focus:outline-none transition w-full sm:w-auto bg-blue-700 hover:bg-blue-800 focus:ring-4 focus:ring-blue-300 dark:bg-blue-600 dark:hover:bg-blue-700 dark:focus:ring-blue-800 disabled:bg-blue-300 disabled:cursor-not-allowed"
    >
      <span v-if="submitCount > 0 && !meta.valid">🚫</span>
      Kaydet
    </button>
  </div>

  <!-- Form Grid -->
  <form
    @submit="onSubmit"
    id="accountForm"
    class="grid grid-cols-1 md:grid-cols-2 gap-6"
  >
    <FieldTextInput
      fieldName="code"
      labelName="Hesap Kodu"
      placeholderName="Hesap Kodu Girin"
    />

    <FieldTextInput
      fieldName="name"
      labelName="Hesap Adı"
      placeholderName="Hesap Adı Girin"
    />

    <FieldSelect
      fieldName="parentAccountId"
      labelName="Üst Hesap"
      placeholderName="Üst Hesap Seçin"
      :options="optionAccounts"
    />

    <FieldSelect
      fieldName="accountType"
      labelName="Hesap Türü"
      placeholderName="Hesap Türü Seçin"
      :options="accountTypeOptions"
    />

    <FieldSelect
      fieldName="normalBalance"
      labelName="Normal Bakiye"
      placeholderName="Normal Bakiye Seçin"
      :options="normalBalanceOptions"
    />    <div class="col-span-full">
      <div class="grid grid-cols-1 md:grid-cols-2 gap-4">
        <FieldCheckbox
          fieldName="isActive"
          labelName="Aktif"
        />
        
        <!-- isPostable seçeneğini UI'dan kaldırıldı, formda gizli olarak tutulacak -->
      </div>
    </div>

    <FieldTextarea
      fieldName="description"
      labelName="Açıklama"
      placeholderName="Açıklama Girin (Opsiyonel)"
      class="col-span-full"
    />
  </form>
</template>

<script setup>
import { useForm } from "vee-validate";
import { toTypedSchema } from "@vee-validate/zod";
import { useAccountStore } from "@/stores/account.store";
import { accountValidationSchema } from "@/services/validations/account-validation";
import FieldTextInput from "@/components/Form/FieldTextInput.vue";
import FieldSelect from "@/components/Form/FieldSelect.vue";
import FieldCheckbox from "@/components/Form/FieldCheckbox.vue";
import FieldTextarea from "@/components/Form/FieldTextArea.vue";
import { onMounted } from "vue";
import { storeToRefs } from "pinia";

const accountStore = useAccountStore();
const { accountTypeOptions, normalBalanceOptions, optionAccounts } = storeToRefs(accountStore);

// Hesap listesi yükleme
onMounted(async () => {
  await accountStore.fetchAccounts();
});

// Form setup
const { handleSubmit, meta, submitCount } = useForm({
  validationSchema: toTypedSchema(accountValidationSchema),
  initialValues: {
    code: "",
    name: "",
    parentAccountId: null,
    isActive: true,
    isPostable: true,
    normalBalance: null,
    accountType: null,
    description: "",
  },
});

// Form submit işlemi
const onSubmit = handleSubmit(async (values) => {
  // Eğer üst hesap (parentAccountId) seçildiyse isPostable değerini false yap,
  // aksi takdirde true olarak bırak
  if (values.parentAccountId) {
    values.isPostable = false;
  }
  
  await accountStore.createAccount(values);
});
</script>

<style scoped></style>
