import axios, { AxiosInstance } from 'axios';

let api: AxiosInstance;

function getToken() {
  // Expect token to be stored in localStorage under 'token'
  return localStorage.getItem('token') || '';
}

export function getApi(): AxiosInstance {
  if (!api) {
    api = axios.create({
      baseURL: axios.defaults.baseURL || '',
      headers: { 'Content-Type': 'application/json' },
    });
    api.interceptors.request.use((config) => {
      const token = getToken();
      if (token) {
        config.headers = config.headers ?? {};
        (config.headers as any).Authorization = `Bearer ${token}`;
      }
      return config;
    });
  }
  return api;
}

export function setToken(token: string) {
  localStorage.setItem('token', token);
}
