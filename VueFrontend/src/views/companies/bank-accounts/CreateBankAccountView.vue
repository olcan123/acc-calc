<template>
  <form class="w-full min-h-screen mx-auto bg-white dark:bg-gray-900 space-y-6 px-4 py-6" @submit.prevent="onSubmit">
    <!-- Başlık ve Kaydet -->
    <div class="flex flex-col md:flex-row items-start md:items-center justify-between mb-6">
      <h1 class="text-2xl font-bold text-gray-900 dark:text-white">Yeni Banka Hesabı Ekle</h1>
      <button
        type="submit"
        class="mt-4 md:mt-0 text-white bg-blue-700 hover:bg-blue-800 focus:ring-4 focus:ring-blue-300 font-semibold rounded-lg text-sm px-6 py-2.5 dark:bg-blue-600 dark:hover:bg-blue-700 focus:outline-none dark:focus:ring-blue-800"
      >
        Kaydet
      </button>
    </div>

    <!-- Banka Hesap Bilgileri -->
    <div class="grid grid-cols-1 md:grid-cols-2 gap-6">
      <FieldSelect fieldName="bankId" labelName="Banka" :options="getSelectBanks" placeholderName="Banka Seçiniz" />
      <FieldTextInput fieldName="branchName" labelName="Şube" placeholderName="Örn. Şehremini Şubesi" />
      <FieldTextInput fieldName="accountNumber" labelName="Hesap No" placeholderName="123456789" />
      <FieldTextInput fieldName="iban" labelName="IBAN" placeholderName="TR..." />
      <FieldTextInput fieldName="swiftCode" labelName="Swift Kodu" placeholderName="XXXXXX" />
      <FieldTextInput fieldName="currency" labelName="Para Birimi" placeholderName="TRY / USD / EUR" />
    </div>
  </form>
</template>

<script setup>
import { useForm } from "vee-validate";
import { useRoute } from "vue-router";
import FieldTextInput from "@/components/Form/FieldTextInput.vue";
import FieldSelect from "@/components/Form/FieldSelect.vue";
import { useBankStore } from "@/stores/bank.store";
import { useBankAccountStore } from "@/stores/bank-account.store";
import { storeToRefs } from "pinia";

const { id } = useRoute().params;

// Store'dan bankaları çek
const bankStore = useBankStore();
const bankAccountStore = useBankAccountStore();
const { getSelectBanks } = storeToRefs(bankStore);
await bankStore.fetchBanks();

// Form setup
const { handleSubmit, values } = useForm({
  initialValues: {
    bankId: 0,
    branchName: "",
    accountNumber: "",
    iban: "",
    swiftCode: "",
    currency: "",
  },
});

// Submit işlemi
const onSubmit = handleSubmit(async (formValues) => {
  // formValues.companies = [{ id }];
  console.log("📥 Form gönderildi:", formValues);
  // Burada API’ye gönderme veya store’a kayıt işlemi yapılabilir
  await bankAccountStore.createBankAccount(id, formValues);
});
</script>
