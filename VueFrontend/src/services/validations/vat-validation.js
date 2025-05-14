import { z } from "zod";

export const vatValidationSchema = z.object({
    id: z
        .number()
        .optional(),
  name: z
    .string()
    .min(1, { message: "KDV adı boş olamaz." })
    .max(50, { message: "KDV adı en fazla 50 karakterden oluşmalıdır." }),

  rate: z.preprocess(
    (val) => val === "" ? undefined : Number(val),
    z
      .number({
        required_error: "KDV oranı boş olamaz.",
        invalid_type_error: "KDV oranı sayı olmalıdır.",
      })
      .min(0, { message: "KDV oranı 0 veya daha büyük olmalıdır." })
      .max(100, { message: "KDV oranı en fazla %100 olabilir." })
  ),

  isDefault: z.boolean().default(false),
  isActive: z.boolean().default(true),
});
