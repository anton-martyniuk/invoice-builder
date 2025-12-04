import { create } from 'zustand';
import { devtools } from 'zustand/middleware';
import type { InvoiceListItem, InvoiceDetail, InvoiceUpsert, PagedInvoices } from '../types/invoice';
import { getInvoices, getInvoice, createInvoice, updateInvoice, deleteInvoice, downloadInvoicePdf } from '../services/invoices';

type State = {
  list: InvoiceListItem[];
  total: number;
  offset: number;
  limit: number;
  current?: InvoiceDetail;
  loading: boolean;
  error?: string;
};

type Actions = {
  fetch: (offset?: number, limit?: number) => Promise<void>;
  fetchOne: (id: string) => Promise<void>;
  create: (payload: InvoiceUpsert) => Promise<InvoiceDetail>;
  update: (id: string, payload: InvoiceUpsert) => Promise<InvoiceDetail>;
  remove: (id: string) => Promise<void>;
  download: (id: string) => Promise<void>;
  setPage: (offset: number, limit: number) => void;
  clearError: () => void;
  setCurrent: (inv?: InvoiceDetail) => void;
};

export const useInvoicesStore = create<State & Actions>()(
  devtools((set, get) => ({
    list: [],
    total: 0,
    offset: 0,
    limit: 10,
    current: undefined,
    loading: false,
    error: undefined,

    setPage: (offset, limit) => set({ offset, limit }),
    setCurrent: (inv) => set({ current: inv }),
    clearError: () => set({ error: undefined }),

    fetch: async (offset, limit) => {
      const o = offset ?? get().offset;
      const l = limit ?? get().limit;
      set({ loading: true, error: undefined });
      try {
        const data: PagedInvoices = await getInvoices(o, l);
        set({ list: data.items, total: data.total, offset: data.offset, limit: data.limit });
      } catch (e: any) {
        set({ error: e?.message || 'Failed to load invoices' });
      } finally {
        set({ loading: false });
      }
    },

    fetchOne: async (id: string) => {
      set({ loading: true, error: undefined });
      try {
        const inv = await getInvoice(id);
        set({ current: inv });
      } catch (e: any) {
        set({ error: e?.message || 'Failed to load invoice' });
      } finally {
        set({ loading: false });
      }
    },

    create: async (payload: InvoiceUpsert) => {
      set({ loading: true, error: undefined });
      try {
        const created = await createInvoice(payload);
        // optimistic refresh
        await get().fetch(0, get().limit);
        return created;
      } catch (e: any) {
        set({ error: e?.message || 'Failed to create invoice' });
        throw e;
      } finally {
        set({ loading: false });
      }
    },

    update: async (id: string, payload: InvoiceUpsert) => {
      set({ loading: true, error: undefined });
      try {
        const updated = await updateInvoice(id, payload);
        await get().fetch(get().offset, get().limit);
        return updated;
      } catch (e: any) {
        set({ error: e?.message || 'Failed to update invoice' });
        throw e;
      } finally {
        set({ loading: false });
      }
    },

    remove: async (id: string) => {
      set({ loading: true, error: undefined });
      try {
        await deleteInvoice(id);
        await get().fetch(get().offset, get().limit);
      } catch (e: any) {
        set({ error: e?.message || 'Failed to delete invoice' });
        throw e;
      } finally {
        set({ loading: false });
      }
    },

    download: async (id: string) => {
      try {
        const { blob, filename } = await downloadInvoicePdf(id);
        const url = window.URL.createObjectURL(blob);
        const a = document.createElement('a');
        a.href = url;
        a.download = filename || `invoice-${id}.pdf`;
        a.click();
        window.URL.revokeObjectURL(url);
      } catch (e: any) {
        set({ error: e?.message || 'Failed to download PDF' });
      }
    },
  }))
);
