import { getApi } from '../lib/api';
import type { Sender } from '../types/sender';
import type { Paged } from '../types/common';

const base = '/api/senders';

export async function getSenders(offset = 0, limit = 10): Promise<Paged<Sender>> {
  const api = getApi();
  const { data } = await api.get(`${base}?offset=${offset}&limit=${limit}`);
  if (Array.isArray(data)) {
    return { items: data as Sender[], offset, limit, total: data.length };
  }
  return data as Paged<Sender>;
}

export async function getSender(id: string): Promise<Sender> {
  const api = getApi();
  const { data } = await api.get(`${base}/${id}`);
  return data as Sender;
}

export async function createSender(payload: Omit<Sender, 'id'>): Promise<Sender> {
  const api = getApi();
  const { data } = await api.post(base, payload);
  return data as Sender;
}

export async function updateSender(id: string, payload: Omit<Sender, 'id'>): Promise<Sender> {
  const api = getApi();
  const { data } = await api.put(`${base}/${id}`, payload);
  return data as Sender;
}

export async function deleteSender(id: string): Promise<void> {
  const api = getApi();
  await api.delete(`${base}/${id}`);
}
