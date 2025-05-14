import { z } from "zod";

export const unitOfMeasureValidationSchema = z.object({
  id: z.number().optional(),

  name: z
    .string()
    .min(1, { message: "Birim adı boş olamaz." })
    .max(100, { message: "Birim adı en fazla 100 karakterden oluşmalıdır." }),

  abbreviation: z
    .string()
    .min(1, { message: "Birim kısaltması boş olamaz." })
    .max(10, { message: "Birim kısaltması en fazla 10 karakterden oluşmalıdır." }),
});
