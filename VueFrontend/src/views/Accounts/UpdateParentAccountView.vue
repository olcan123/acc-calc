<template>
  <!-- Üst Başlık ve Kaydet Butonu -->
  <div
    class="flex flex-col sm:flex-row justify-between items-center mb-6 gap-4"
  >
    <h2 class="text-2xl font-semibold text-gray-800 dark:text-white">
      Üst Hesap Bilgilerini Düzenle
    </h2>

    <button
      type="submit"
      form="accountForm"
      :disabled="submitCount > 0 && !meta.valid"
      class="flex items-center justify-center gap-2 text-white font-medium rounded-lg text-sm px-5 py-2.5 focus:outline-none transition w-full sm:w-auto bg-blue-700 hover:bg-blue-800 focus:ring-4 focus:ring-blue-300 dark:bg-blue-600 dark:hover:bg-blue-700 dark:focus:ring-blue-800 disabled:bg-blue-300 disabled:cursor-not-allowed"
    >
      <span v-if="submitCount > 0 && !meta.valid">🚫</span>
      Düzenle
    </button>
  </div>

  <div v-if="loading" class="flex justify-center items-center py-10">
    <div class="animate-spin rounded-full h-10 w-10 border-b-2 border-blue-700"></div>
  </div>

  <!-- Form Grid -->
  <form
    v-else
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
      :options="filteredAccounts"
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
    />

    <div class="col-span-full">
      <div class="grid grid-cols-1 md:grid-cols-2 gap-4">
        <FieldCheckbox
          fieldName="isActive"
          labelName="Aktif"
        />
        
        <!-- isPostable seçeneği UI'dan kaldırıldı ve her zaman false -->
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
import { onMounted, computed } from "vue";
import { useRoute } from "vue-router";
import { storeToRefs } from "pinia";

const route = useRoute();
const accountId = parseInt(route.params.id);

const accountStore = useAccountStore();
const { accounts, account, loading, accountTypeOptions, normalBalanceOptions } = storeToRefs(accountStore);

// Filtrelenmiş hesap listesi (kendisini ve kendi alt hesaplarını üst hesap olarak seçimden çıkarma)
const filteredAccounts = computed(() => {
  // Hesabın kendisi ve alt hesaplarının ID'lerini bul
  const findAllChildIds = (parentId) => {
    const result = [parentId];
    const children = accounts.value.filter(a => a.parentAccountId === parentId);
    children.forEach(child => {
      result.push(...findAllChildIds(child.id));
    });
    return result;
  };
  
  const excludedIds = findAllChildIds(accountId);
  
  return accounts.value
    .filter(a => !excludedIds.includes(a.id))
    .map(a => ({
      value: a.id,
      label: `${a.code} - ${a.name}`,
    }));
});

// Form setup
const { handleSubmit, meta, submitCount, setValues } = useForm({
  validationSchema: toTypedSchema(accountValidationSchema),
});

// Hesap verilerini yükleme
onMounted(async () => {
  await accountStore.fetchAccounts();
  await loadAccount();
});

const loadAccount = async () => {
  await accountStore.fetchAccount(accountId);
  
  // Form değerlerini ayarla
  setValues({
    id: account.value.id,
    code: account.value.code,
    name: account.value.name,
    parentAccountId: account.value.parentAccountId,
    isActive: account.value.isActive,
    isPostable: false,  // Her zaman false olacak şekilde override
    normalBalance: account.value.normalBalance,
    accountType: account.value.accountType,
    description: account.value.description || "",
  });
};

// Form submit işlemi
const onSubmit = handleSubmit(async (values) => {
  // Üst hesap için isPostable her zaman false olmalı
  values.isPostable = false;
  
  await accountStore.updateAccount(values);
});
</script>

<style scoped></style>
