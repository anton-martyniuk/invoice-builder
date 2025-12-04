import { getApi } from '../lib/api';
import type { Customer } from '../types/customer';
import type { Paged } from '../types/common';

const base = '/api/customers';

export async function getCustomers(offset = 0, limit = 10): Promise<Paged<Customer>> {
  const api = getApi();
  const { data } = await api.get(`${base}?offset=${offset}&limit=${limit}`);
  if (Array.isArray(data)) {
    return { items: data as Customer[], offset, limit, total: data.length };
  }
  return data as Paged<Customer>;
}

export async function getCustomer(id: string): Promise<Customer> {
  const api = getApi();
  const { data } = await api.get(`${base}/${id}`);
  return data as Customer;
}

export async function createCustomer(payload: Omit<Customer, 'id'>): Promise<Customer> {
  const api = getApi();
  const { data } = await api.post(base, payload);
  return data as Customer;
}

export async function updateCustomer(id: string, payload: Omit<Customer, 'id'>): Promise<Customer> {
  const api = getApi();
  const { data } = await api.put(`${base}/${id}`, payload);
  return data as Customer;
}

export async function deleteCustomer(id: string): Promise<void> {
  const api = getApi();
  await api.delete(`${base}/${id}`);
}
