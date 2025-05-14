<template>
  <!-- Üst Başlık ve Kaydet Butonu -->
  <div
    class="flex flex-col sm:flex-row justify-between items-center mb-6 gap-4"
  >
    <h2 class="text-2xl font-semibold text-gray-800 dark:text-white">
      Banka Hesabını Güncelle
    </h2>

    <button
      type="submit"
      form="bankAccountForm"
      :disabled="submitCount > 0 && !meta.valid"
      class="flex items-center justify-center gap-2 text-white font-medium rounded-lg text-sm px-5 py-2.5 focus:outline-none transition w-full sm:w-auto bg-blue-700 hover:bg-blue-800 focus:ring-4 focus:ring-blue-300 dark:bg-blue-600 dark:hover:bg-blue-700 dark:focus:ring-blue-800 disabled:bg-blue-300 disabled:cursor-not-allowed"
    >
      <span v-if="submitCount > 0 && !meta.valid">🚫</span>
      Düzenle
    </button>
  </div>

  <!-- Form -->
  <form
    @submit="onSubmit"
    id="bankAccountForm"
    class="grid grid-cols-1 md:grid-cols-2 gap-6"
  >
    <FieldSelect fieldName="bankId" labelName="Banka" :options="optionBanks" />
    <FieldTextInput fieldName="branchName" labelName="Şube Adı" />
    <FieldTextInput fieldName="accountNumber" labelName="Hesap No" />
    <FieldTextInput fieldName="iban" labelName="IBAN" />
    <FieldTextInput field-name="swiftCode" label-name="Swift Kodu" />
    <FieldTextInput fieldName="currency" labelName="Para Birimi" />
  </form>
</template>

<script setup>
import { useForm } from "vee-validate";
import { toTypedSchema } from "@vee-validate/zod";
import { useRoute } from "vue-router";
import { storeToRefs } from "pinia";
import { useBankAccountCompanyStore } from "@/stores/bank-account-company.store";
import { useBankStore } from "@/stores/bank.store";
import FieldTextInput from "@/components/Form/FieldTextInput.vue";
import FieldSelect from "@/components/Form/FieldSelect.vue";
import { bankAccountSchema } from "@/services/validations/bank-account-validation";

const route = useRoute();
const { companyId } = route.params;
const bankAccountId = route.params.bankAccountId;

const bankStore = useBankStore();
const { optionBanks } = storeToRefs(bankStore);
await bankStore.fetchBanks();
const bankAccountStore = useBankAccountCompanyStore();
const { bankAccount } = storeToRefs(bankAccountStore);
await bankAccountStore.fetchBankAccount(companyId, bankAccountId);

const { handleSubmit, setValues, meta, submitCount } = useForm({
  validationSchema: toTypedSchema(bankAccountSchema),
  initialValues: {
    bankId: "",
    branchName: "",
    accountNumber: "",
    swiftCode: "",
    iban: "",
    currency: "",
  },
});

setValues(bankAccount.value);

const onSubmit = handleSubmit(async (values) => {
  await bankAccountStore.updateBankAccount(companyId, bankAccountId, values);
});
</script>
