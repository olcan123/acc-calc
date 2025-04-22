import axios from "axios";

export const axiosInstance = axios.create({
  baseURL: import.meta.env.VITE_API_URL || "https://localhost:7096/api/",
  headers: {
    "Content-Type": "application/json",
  },
});
