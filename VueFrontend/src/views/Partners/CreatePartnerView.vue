<template>
  <!-- Başlık ve Kaydet Butonu -->
  <div
    class="flex flex-col sm:flex-row justify-between items-center mb-6 gap-4"
  >
    <h2 class="text-2xl font-semibold text-gray-800 dark:text-white">
      Yeni Partner Ekle
    </h2>

    <button
      type="submit"
      form="partnerForm"
      :disabled="submitCount > 0 && !meta.valid"
      class="flex items-center justify-center gap-2 text-white font-medium rounded-lg text-sm px-5 py-2.5 focus:outline-none transition w-full sm:w-auto bg-blue-700 hover:bg-blue-800 focus:ring-4 focus:ring-blue-300 dark:bg-blue-600 dark:hover:bg-blue-700 dark:focus:ring-blue-800 disabled:bg-blue-300 disabled:cursor-not-allowed"
    >
      <span v-if="submitCount > 0 && !meta.valid">🚫</span>
      Kaydet
    </button>
  </div>

  <!-- Form -->
  <form
    @submit="onSubmit"
    id="partnerForm"
    class="grid grid-cols-1 md:grid-cols-2 gap-6"
  >
    <FieldTextInput fieldName="partner.name" labelName="Ad" />
    <FieldTextInput fieldName="partner.tradeName" labelName="Ticari Ünvan" />

    <!-- PartnerType Seçimi -->
    <FieldSelect
      fieldName="partner.partnerType"
      labelName="Partner Tipi"
      :options="[
        { label: 'İşletme', value: 0 }, // PartnerType.Business
        { label: 'Bireysel', value: 1 }, // PartnerType.Individual
        { label: 'Çalışan', value: 2 }, // PartnerType.Employee
      ]"
    />

    <!-- BusinessPartnerType sadece Business ve Individual için -->
    <FieldSelect
      v-if="[0, 1].includes(formValues.partner.partnerType)"
      fieldName="partner.businessPartnerType"
      labelName="İş Partner Tipi"
      :options="[
        { label: 'Tedarikçi', value: 0 }, // BusinessPartnerType.Supplier
        { label: 'Müşteri', value: 1 }, // BusinessPartnerType.Customer
        { label: 'Her ikisi', value: 2 }, // BusinessPartnerType.Both
      ]"
    />

    <FieldTextInput fieldName="partner.identityNumber" labelName="Kimlik No" />
    <FieldTextInput fieldName="partner.vatNumber" labelName="Vergi No" />

    <!-- Adres Bilgileri Başlığı -->
    <div class="md:col-span-2 border-t pt-4">
      <h3 class="text-lg font-semibold text-gray-700 dark:text-white mb-2">
        Adres Bilgileri
      </h3>
    </div>

    <!-- Adres Alanları -->
    <FieldTextInput fieldName="address.type" labelName="Adres Tipi" />
    <FieldTextInput fieldName="address.street" labelName="Sokak" />
    <FieldTextInput fieldName="address.city" labelName="Şehir" />
    <FieldTextInput fieldName="address.country" labelName="Ülke" />
    <FieldTextInput fieldName="address.zipCode" labelName="Posta Kodu" />
  </form>
</template>

<script setup>
import { useForm } from "vee-validate";
import { toTypedSchema } from "@vee-validate/zod";
import { watch } from "vue";
import { useRouter } from "vue-router";
import { usePartnerStore } from "@/stores/partner.store";
import FieldTextInput from "@/components/Form/FieldTextInput.vue";
import FieldSelect from "@/components/Form/FieldSelect.vue";
import { partnerValidationSchema } from "@/services/validations/partner.validation";

const router = useRouter();
const partnerStore = usePartnerStore();

const {
  handleSubmit,
  values: formValues,
  submitCount,
  meta,
  setFieldValue,
} = useForm({
  validationSchema: toTypedSchema(partnerValidationSchema),
  initialValues: {
    partner: {
      name: "",
      tradeName: "",
      partnerType: 0,
      businessPartnerType: null,
      identityNumber: "",
      vatNumber: "",
    },
    address: {
      type: "",
      street: "",
      city: "",
      country: "",
      zipCode: "",
    },
  },
});

// Eğer PartnerType "Çalışan" (0) ise, businessPartnerType null yapılır
watch(
  () => formValues.partner.partnerType,
  (type) => {
    if (type === 2) {
      // PartnerType.Employee
      setFieldValue("partner.businessPartnerType", null); // ✅ Doğru kullanım
    }
  }
);

const onSubmit = handleSubmit(async (values) => {
  await partnerStore.addPartner(values);
});
</script>
