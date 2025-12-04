import { getApi } from '../lib/api';
import type { InvoiceListItem, InvoiceDetail, InvoiceUpsert, PagedInvoices } from '../types/invoice';

const base = '/api/invoices';

export async function getInvoices(offset = 0, limit = 10): Promise<PagedInvoices> {
  const api = getApi();
  const { data } = await api.get(`${base}?offset=${offset}&limit=${limit}`);
  // If backend returns array with total headers, adapt as needed.
  if (Array.isArray(data)) {
    return { items: data as InvoiceListItem[], total: data.length, offset, limit } as PagedInvoices;
  }
  return data as PagedInvoices;
}

export async function getInvoice(id: string): Promise<InvoiceDetail> {
  const api = getApi();
  const { data } = await api.get(`${base}/${id}`);
  return data as InvoiceDetail;
}

export async function createInvoice(payload: InvoiceUpsert): Promise<InvoiceDetail> {
  const api = getApi();
  const { data } = await api.post(base, payload);
  return data as InvoiceDetail;
}

export async function updateInvoice(id: string, payload: InvoiceUpsert): Promise<InvoiceDetail> {
  const api = getApi();
  const { data } = await api.put(`${base}/${id}`, payload);
  return data as InvoiceDetail;
}

export async function deleteInvoice(id: string): Promise<void> {
  const api = getApi();
  await api.delete(`${base}/${id}`);
}

export async function downloadInvoicePdf(
  id: string
): Promise<{ blob: Blob; filename: string }> {
  const api = getApi();
  const response = await api.get(`${base}/${id}/download`, { responseType: 'blob' });
  // Axios v1+ uses AxiosHeaders which requires using .get() to read header values in the browser
  const headers: any = response.headers as any;
  const disposition = (
    headers?.get?.('content-disposition') ??
    headers?.get?.('Content-Disposition') ??
    headers?.['content-disposition'] ??
    headers?.['Content-Disposition']
  ) as string | undefined;
  
  console.log("disposition: ", disposition);

  let filename = `invoice-${id}.pdf`;
  if (disposition) {
    // Try to parse filename from header e.g. attachment; filename="INV-2024-001.pdf"
    const match = /filename\*=UTF-8''([^;]+)|filename="?([^";]+)"?/i.exec(disposition);
    const raw = decodeURIComponent(match?.[1] || match?.[2] || '').trim();
    
    console.log('disposition:', disposition);
    console.log('filename:', raw);
    if (raw) filename = raw;
  }

  return { blob: response.data as Blob, filename };
}
