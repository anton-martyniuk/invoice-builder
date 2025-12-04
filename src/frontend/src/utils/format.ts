export function formatCurrency(amount: number, currency = 'USD') {
  if (isNaN(amount as any)) return '-';
  try {
    return new Intl.NumberFormat(undefined, { style: 'currency', currency }).format(amount);
  } catch {
    return amount.toFixed(2);
  }
}

export function formatDate(iso?: string) {
  if (!iso) return '-';
  const d = new Date(iso);
  if (isNaN(d.getTime())) return iso;
  return d.toLocaleDateString();
}

export function toNumber(value: string | number): number {
  if (typeof value === 'number') return value;
  const n = Number(value);
  return isNaN(n) ? 0 : n;
}

export function isoToDateInput(iso?: string): string {
  if (!iso) return '';
  const d = new Date(iso);
  if (isNaN(d.getTime())) return '';
  const yyyy = d.getFullYear();
  const mm = String(d.getMonth() + 1).padStart(2, '0');
  const dd = String(d.getDate()).padStart(2, '0');
  return `${yyyy}-${mm}-${dd}`;
}

export function dateInputToIso(dateStr?: string): string {
  if (!dateStr) return new Date().toISOString();
  // treat as local date at midnight
  const d = new Date(dateStr + 'T00:00:00');
  return d.toISOString();
}
