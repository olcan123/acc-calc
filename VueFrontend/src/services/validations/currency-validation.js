import { z } from "zod";

export const currencyValidationSchema = z.object({
  id: z.number().optional(),
  code: z
    .string()
    .nonempty("Para birimi kodu gereklidir")
    .max(10, "Para birimi kodu en fazla 10 karakter olabilir"),
  name: z
    .string()
    .nonempty("Para birimi adı gereklidir")
    .max(50, "Para birimi adı en fazla 50 karakter olabilir")
});
