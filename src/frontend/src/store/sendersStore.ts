import { create } from 'zustand';
import { devtools } from 'zustand/middleware';
import type { Sender } from '../types/sender';
import type { Paged } from '../types/common';
import { getSenders, getSender, createSender, updateSender, deleteSender } from '../services/senders';

type State = {
  list: Sender[];
  total: number;
  offset: number;
  limit: number;
  current?: Sender;
  loading: boolean;
  error?: string;
};

type Actions = {
  setPage: (offset: number, limit: number) => void;
  fetch: (offset?: number, limit?: number) => Promise<void>;
  fetchOne: (id: string) => Promise<void>;
  create: (payload: Omit<Sender, 'id'>) => Promise<Sender>;
  update: (id: string, payload: Omit<Sender, 'id'>) => Promise<Sender>;
  remove: (id: string) => Promise<void>;
  setCurrent: (s?: Sender) => void;
  clearError: () => void;
};

export const useSendersStore = create<State & Actions>()(
  devtools((set, get) => ({
    list: [],
    total: 0,
    offset: 0,
    limit: 10,
    current: undefined,
    loading: false,
    error: undefined,

    setPage: (offset, limit) => set({ offset, limit }),
    setCurrent: (s) => set({ current: s }),
    clearError: () => set({ error: undefined }),

    fetch: async (offset, limit) => {
      const o = offset ?? get().offset;
      const l = limit ?? get().limit;
      set({ loading: true, error: undefined });
      try {
        const data: Paged<Sender> = await getSenders(o, l);
        set({ list: data.items, total: data.total, offset: data.offset, limit: data.limit });
      } catch (e: any) {
        set({ error: e?.message || 'Failed to load senders' });
      } finally {
        set({ loading: false });
      }
    },

    fetchOne: async (id: string) => {
      set({ loading: true, error: undefined });
      try {
        const s = await getSender(id);
        set({ current: s });
      } catch (e: any) {
        set({ error: e?.message || 'Failed to load sender' });
      } finally {
        set({ loading: false });
      }
    },

    create: async (payload: Omit<Sender, 'id'>) => {
      set({ loading: true, error: undefined });
      try {
        const created = await createSender(payload);
        await get().fetch(0, get().limit);
        return created;
      } catch (e: any) {
        set({ error: e?.message || 'Failed to create sender' });
        throw e;
      } finally {
        set({ loading: false });
      }
    },

    update: async (id: string, payload: Omit<Sender, 'id'>) => {
      set({ loading: true, error: undefined });
      try {
        const updated = await updateSender(id, payload);
        await get().fetch(get().offset, get().limit);
        return updated;
      } catch (e: any) {
        set({ error: e?.message || 'Failed to update sender' });
        throw e;
      } finally {
        set({ loading: false });
      }
    },

    remove: async (id: string) => {
      set({ loading: true, error: undefined });
      try {
        await deleteSender(id);
        await get().fetch(get().offset, get().limit);
      } catch (e: any) {
        set({ error: e?.message || 'Failed to delete sender' });
        throw e;
      } finally {
        set({ loading: false });
      }
    },
  }))
);
