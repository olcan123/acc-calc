<template>
  <form
    class="w-full min-h-screen mx-auto bg-white dark:bg-gray-900 space-y-6 px-4 py-6"
    @submit="onSubmit"
  >
    <!-- Başlık ve Kaydet -->
    <div
      class="flex flex-col md:flex-row items-start md:items-center justify-between mb-6"
    >
      <h1 class="text-2xl font-bold text-gray-900 dark:text-white">
        Yeni Şirket Ekle
      </h1>
      <button
        type="submit"
        class="mt-4 md:mt-0 text-white bg-blue-700 hover:bg-blue-800 focus:ring-4 focus:ring-blue-300 font-semibold rounded-lg text-sm px-6 py-2.5 dark:bg-blue-600 dark:hover:bg-blue-700 focus:outline-none dark:focus:ring-blue-800"
      >
        {{ id ? "Güncelle" : "Kaydet" }}
      </button>
    </div>

    <!-- Şirket Bilgileri -->
    <div class="grid grid-cols-1 md:grid-cols-2 gap-6">
      <FieldTextInput
        fieldName="name"
        labelName="Adı"
        placeholderName="Company Name"
      />
      <FieldTextInput
        fieldName="tradeName"
        labelName="Ticari Adı"
        placeholderName="Trade Name"
      />
      <FieldTextInput
        fieldName="uidNumber"
        labelName="UID Numarası"
        placeholderName="UID Number"
      />
      <FieldTextInput
        fieldName="vatNumber"
        labelName="KDV Numarası"
        placeholderName="VAT Number"
      />
      <FieldTextInput
        fieldName="period"
        labelName="Periyot"
        placeholderName="Period"
      />
    </div>

    <!-- Banka Hesapları -->
    <div class="mt-12 space-y-12">
      <div
        class="flex flex-col md:flex-row items-start md:items-center justify-between mb-4"
      >
        <h2 class="text-2xl font-bold text-gray-900 dark:text-white">
          Banka Hesapları
        </h2>
        <button
          type="button"
          @click="addBankAccount"
          class="mt-4 md:mt-0 text-white bg-blue-700 hover:bg-blue-800 focus:ring-4 focus:ring-blue-300 font-semibold rounded-lg text-sm px-6 py-2.5 dark:bg-blue-600 dark:hover:bg-blue-700 focus:outline-none dark:focus:ring-blue-800"
        >
          Yeni Hesap Ekle
        </button>
      </div>

      <div
        v-for="(account, aIndex) in bankAccountFields"
        :key="account.key"
        class="space-y-4 border border-gray-200 dark:border-gray-700 p-4 rounded-lg"
      >
        <div class="grid grid-cols-1 md:grid-cols-3 gap-4">
          <FieldSelect
            :fieldName="`bankAccounts[${aIndex}].bankId`"
            labelName="Banka"
            :options="getSelectBanks"
          />
          <FieldTextInput
            :fieldName="`bankAccounts[${aIndex}].branchName`"
            labelName="Şube"
            placeholderName="Şube Adı"
          />
          <FieldTextInput
            :fieldName="`bankAccounts[${aIndex}].accountNumber`"
            labelName="Hesap No"
            placeholderName="123456789"
          />
          <FieldTextInput
            :fieldName="`bankAccounts[${aIndex}].iban`"
            labelName="IBAN"
            placeholderName="TR..."
          />
          <FieldTextInput
            :fieldName="`bankAccounts[${aIndex}].swiftCode`"
            labelName="Swift Kodu"
            placeholderName="XXXXXX"
          />
          <FieldTextInput
            :fieldName="`bankAccounts[${aIndex}].currency`"
            labelName="Para Birimi"
            placeholderName="TRY / EUR / USD"
          />
        </div>
        <button
          type="button"
          @click="deleteBankAccount(aIndex, account.value.id)"
          class="text-white bg-red-600 hover:bg-red-700 focus:ring-4 focus:ring-red-300 font-semibold rounded-lg text-sm px-6 py-2.5 dark:bg-red-600 dark:hover:bg-red-700 focus:outline-none dark:focus:ring-red-800"
        >
          Banka Hesabını Sil
        </button>
      </div>
    </div>

    <!-- Depo Bilgileri -->
    <div class="mt-12 space-y-12">
      <div
        class="flex flex-col md:flex-row items-start md:items-center justify-between mb-4"
      >
        <h2 class="text-2xl font-bold text-gray-900 dark:text-white">
          Depolar
        </h2>
        <button
          type="button"
          @click="addWarehouse"
          class="mt-4 md:mt-0 text-white bg-blue-700 hover:bg-blue-800 focus:ring-4 focus:ring-blue-300 font-semibold rounded-lg text-sm px-6 py-2.5 dark:bg-blue-600 dark:hover:bg-blue-700 focus:outline-none dark:focus:ring-blue-800"
        >
          Yeni Depo Ekle
        </button>
      </div>

      <div
        v-for="(warehouse, wIndex) in warehouseFields"
        :key="warehouse.key"
        class="space-y-6 border border-gray-200 dark:border-gray-700 p-4 rounded-lg"
      >
        <div class="flex items-end gap-3">
          <div class="w-full">
            <FieldTextInput
              :fieldName="`warehouses[${wIndex}].name`"
              :labelName="`Depo Adı ${wIndex + 1}`"
              :placeholderName="`Depo Adı ${wIndex + 1}`"
            />
          </div>

          <button
            type="button"
            @click="deleteWarehouse(wIndex, warehouse.value.id)"
            class="mt-4 text-white bg-red-600 hover:bg-red-700 focus:ring-4 focus:ring-red-300 font-semibold rounded-lg text-sm px-6 py-2.5 dark:bg-red-600 dark:hover:bg-red-700 focus:outline-none dark:focus:ring-red-800"
          >
            Sil
          </button>
        </div>

        <!-- Adres Listesi -->
        <div
          v-for="(address, aIndex) in getAddresses(wIndex)"
          :key="aIndex"
          class="grid grid-cols-1 md:grid-cols-5 gap-4 items-end"
        >
          <FieldTextInput
            :fieldName="`warehouses[${wIndex}].addresses[${aIndex}].street`"
            labelName="Sokak"
            placeholderName="Sokak"
          />
          <FieldTextInput
            :fieldName="`warehouses[${wIndex}].addresses[${aIndex}].type`"
            labelName="Adres Tipi"
            placeholderName="Ev, Ofis..."
          />
          <FieldTextInput
            :fieldName="`warehouses[${wIndex}].addresses[${aIndex}].city`"
            labelName="Şehir"
            placeholderName="İstanbul"
          />
          <FieldTextInput
            :fieldName="`warehouses[${wIndex}].addresses[${aIndex}].country`"
            labelName="Ülke"
            placeholderName="Türkiye"
          />
          <FieldTextInput
            :fieldName="`warehouses[${wIndex}].addresses[${aIndex}].zipCode`"
            labelName="Posta Kodu"
            placeholderName="34000"
          />
        </div>
      </div>
    </div>
  </form>
</template>

<script setup>
import { useForm, useFieldArray } from "vee-validate";
import { storeToRefs } from "pinia";
import { useRoute } from "vue-router";
import FieldTextInput from "@/components/Form/FieldTextInput.vue";
import FieldSelect from "@/components/Form/FieldSelect.vue";
import { useCompanyStore } from "@/stores/company.store";
import { useBankStore } from "@/stores/bank.store";
import { useBankAccountStore } from "@/stores/bank-account.store";
import { useWarehouseStore } from "@/stores/warehouse.store";
import { useSanitize } from "@/composables/useSanitize";

const { removeEmptyArraysAndNulls } = useSanitize();

const { id } = useRoute().params;

const companyStore = useCompanyStore();
const bankStore = useBankStore();
const bankAccountStore = useBankAccountStore();
const warehouseStore = useWarehouseStore();

const { getSelectBanks } = storeToRefs(bankStore);

const { company } = storeToRefs(companyStore);

await bankStore.fetchBanks();

if (id) await companyStore.fetchCompanyWithInclude(id);

const { handleSubmit, values, setValues } = useForm({
  initialValues: {
    name: "",
    tradeName: "",
    uidNumber: "",
    vatNumber: "",
    period: "",
    bankAccounts: [
      {
        bankId: 0,
        branchName: "",
        accountNumber: "",
        iban: "",
        swiftCode: "",
        currency: "",
      },
    ],
    warehouses: [
      {
        name: "",
        addresses: [
          {
            street: "",
            type: "",
            city: "",
            country: "",
            zipCode: "",
          },
        ],
        contacts: [
          {
            name: "",
            contactDetails: [
              {
                name: "",
                value: "",
                description: "",
                isActive: true,
                isPrimary: false,
              },
            ],
          },
        ],
      },
    ],
  },
});

if (id) {
  const { bankAccounts, warehouses, ...data } = company.value.data;

  setValues({
    bankAccounts,
    warehouses,
    ...data,
  });
}

// Depo işlemleri
const {
  fields: warehouseFields,
  push: pushWarehouse,
  remove: removeWarehouse,
} = useFieldArray("warehouses");

const getAddresses = (warehouseIndex) =>
  values.warehouses[warehouseIndex]?.addresses ?? [];

const addWarehouse = () => {
  pushWarehouse({
    name: "",
    addresses: [
      {
        street: "",
        type: "",
        city: "",
        country: "",
        zipCode: "",
      },
    ],
  });
};

// Banka işlemleri
const {
  fields: bankAccountFields,
  push: pushBankAccount,
  remove: removeBankAccount,
} = useFieldArray("bankAccounts");

const addBankAccount = () => {
  pushBankAccount({
    bankId: "",
    branchName: "",
    accountNumber: "",
    iban: "",
    swiftCode: "",
    currency: "",
  });
};

const deleteBankAccount = async (aIndex, id) => {
  if (id) {
    await bankAccountStore.deleteBankAccount(id);
  }
  removeBankAccount(aIndex);
};

const deleteWarehouse = async (wIndex, id) => {
  if (id) {
    await warehouseStore.deleteWarehouse(id);
  }
  removeWarehouse(wIndex);
};

const onSubmit = handleSubmit(async (formValues) => {
  formValues = removeEmptyArraysAndNulls(formValues);
  if (id) {
    await companyStore.updateCompany(formValues);
  } else {
    await companyStore.createCompany(formValues);
  }
});
</script>
