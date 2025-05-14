import { z } from "zod";

export const barcodeValidationSchema = z.object({
  id: z.number().optional(),
  code: z.string().nonempty("Barkod boş olamaz"),
  type: z.string().nonempty("Barkod tipi boş olamaz"),
  productId: z.number().optional()
});

export const productPriceValidationSchema = z.object({
  id: z.number().optional(),
  unitPrice: z.number()
    .min(0, "Fiyat negatif olamaz")
    .or(z.string().regex(/^\d+(\.\d+)?$/).transform(value => parseFloat(value))),
  category: z.number().int().min(0).max(1), // Regular = 0, Promo = 1
  side: z.number().int().min(0).max(1),    // Purchase = 0, Sales = 1
  validFrom: z.date().or(z.string().or(z.null())).optional(),
  validTo: z.date().or(z.string().or(z.null())).optional(),
  productId: z.number().optional()
});

export const productCategoryValidationSchema = z.object({
  categoryId: z.number().int().positive("Kategori seçilmelidir"),
  productId: z.number().optional()
});

export const productValidationSchema = z.object({
  product: z.object({
    id: z.number().optional(),
    name: z.string().nonempty("Ürün adı boş olamaz"),
    description: z.string().optional().nullable(),
    customsTaxRate: z.number()
      .min(0, "Gümrük vergisi oranı negatif olamaz")
      .max(100, "Gümrük vergisi oranı %100'den büyük olamaz")
      .or(z.string().regex(/^\d+(\.\d+)?$/).transform(value => parseFloat(value))),
    exciseTaxRate: z.number()
      .min(0, "ÖTV oranı negatif olamaz")
      .max(100, "ÖTV oranı %100'den büyük olamaz")
      .or(z.string().regex(/^\d+(\.\d+)?$/).transform(value => parseFloat(value))),
    vatId: z.number().int().positive("KDV oranı seçilmelidir"),
    unitOfMeasureId: z.number().int().positive("Ölçü birimi seçilmelidir"),
  }),
  
  barcodes: z.array(barcodeValidationSchema).optional(),
  productPrices: z.array(productPriceValidationSchema).optional(),
  productCategories: z.array(productCategoryValidationSchema).optional()
});
