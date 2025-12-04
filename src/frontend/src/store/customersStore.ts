import { create } from 'zustand';
import { devtools } from 'zustand/middleware';
import type { Customer } from '../types/customer';
import type { Paged } from '../types/common';
import { getCustomers, getCustomer, createCustomer, updateCustomer, deleteCustomer } from '../services/customers';

type State = {
  list: Customer[];
  total: number;
  offset: number;
  limit: number;
  current?: Customer;
  loading: boolean;
  error?: string;
};

type Actions = {
  setPage: (offset: number, limit: number) => void;
  fetch: (offset?: number, limit?: number) => Promise<void>;
  fetchOne: (id: string) => Promise<void>;
  create: (payload: Omit<Customer, 'id'>) => Promise<Customer>;
  update: (id: string, payload: Omit<Customer, 'id'>) => Promise<Customer>;
  remove: (id: string) => Promise<void>;
  setCurrent: (c?: Customer) => void;
  clearError: () => void;
};

export const useCustomersStore = create<State & Actions>()(
  devtools((set, get) => ({
    list: [],
    total: 0,
    offset: 0,
    limit: 10,
    current: undefined,
    loading: false,
    error: undefined,

    setPage: (offset, limit) => set({ offset, limit }),
    setCurrent: (c) => set({ current: c }),
    clearError: () => set({ error: undefined }),

    fetch: async (offset, limit) => {
      const o = offset ?? get().offset;
      const l = limit ?? get().limit;
      set({ loading: true, error: undefined });
      try {
        const data: Paged<Customer> = await getCustomers(o, l);
        set({ list: data.items, total: data.total, offset: data.offset, limit: data.limit });
      } catch (e: any) {
        set({ error: e?.message || 'Failed to load customers' });
      } finally {
        set({ loading: false });
      }
    },

    fetchOne: async (id: string) => {
      set({ loading: true, error: undefined });
      try {
        const c = await getCustomer(id);
        set({ current: c });
      } catch (e: any) {
        set({ error: e?.message || 'Failed to load customer' });
      } finally {
        set({ loading: false });
      }
    },

    create: async (payload: Omit<Customer, 'id'>) => {
      set({ loading: true, error: undefined });
      try {
        const created = await createCustomer(payload);
        await get().fetch(0, get().limit);
        return created;
      } catch (e: any) {
        set({ error: e?.message || 'Failed to create customer' });
        throw e;
      } finally {
        set({ loading: false });
      }
    },

    update: async (id: string, payload: Omit<Customer, 'id'>) => {
      set({ loading: true, error: undefined });
      try {
        const updated = await updateCustomer(id, payload);
        await get().fetch(get().offset, get().limit);
        return updated;
      } catch (e: any) {
        set({ error: e?.message || 'Failed to update customer' });
        throw e;
      } finally {
        set({ loading: false });
      }
    },

    remove: async (id: string) => {
      set({ loading: true, error: undefined });
      try {
        await deleteCustomer(id);
        await get().fetch(get().offset, get().limit);
      } catch (e: any) {
        set({ error: e?.message || 'Failed to delete customer' });
        throw e;
      } finally {
        set({ loading: false });
      }
    },
  }))
);
