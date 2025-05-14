import { z } from "zod";

export const categoryValidationSchema = z.object({
    id: z
        .number()
        .optional(),
    name: z
        .string()
        .min(1, { message: "Kategori adı boş olamaz." })
        .max(100, { message: "Kategori adı en fazla 100 karakterden oluşmalıdır." })
});
