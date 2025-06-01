import { z } from "zod";

export const accountValidationSchema = z.object({
  id: z.number().optional(),
  code: z.string().min(1, "Hesap kodu gereklidir"),
  name: z.string().min(1, "Hesap adı gereklidir"),
  parentAccountId: z.number().nullable(),
  isActive: z.boolean().default(true),
  isPostable: z.boolean().default(true),
  normalBalance: z.number().nullable(),
  accountType: z.number().min(1, "Hesap türü seçilmelidir"),
  description: z.string().nullable(),
});
