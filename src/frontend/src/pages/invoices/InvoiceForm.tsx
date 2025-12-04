import React, { useEffect, useMemo, useState } from 'react';
import { useNavigate, useParams } from 'react-router-dom';
import { useInvoicesStore } from '../../store/invoicesStore';
import { useCustomersStore } from '../../store/customersStore';
import { useSendersStore } from '../../store/sendersStore';
import { NumberInput, SelectInput, TextArea, TextInput } from '../../components/Form';
import Loading from '../../components/Loading';
import type { LineItem, InvoiceUpsert } from '../../types/invoice';
import { dateInputToIso, isoToDateInput, toNumber } from '../../utils/format';

type Props = { mode: 'create' | 'edit' };

const emptyItem = (): LineItem => ({ itemName: '', quantity: 1, unitPrice: 0, total: 0 });

const InvoiceForm: React.FC<Props> = ({ mode }) => {
  const navigate = useNavigate();
  const { id } = useParams();
  const invoices = useInvoicesStore();
  const customers = useCustomersStore();
  const senders = useSendersStore();

  const [invoiceNumber, setInvoiceNumber] = useState('');
  const [invoiceDate, setInvoiceDate] = useState('');
  const [dueDate, setDueDate] = useState('');
  const [currency, setCurrency] = useState('USD');
  const [notes, setNotes] = useState('');
  const [customerId, setCustomerId] = useState('');
  const [senderId, setSenderId] = useState('');
  const [taxRatePct, setTaxRatePct] = useState<number>(0);
  const [lineItems, setLineItems] = useState<LineItem[]>([emptyItem()]);

  const [saving, setSaving] = useState(false);

  useEffect(() => {
    // preload dropdowns
    if (customers.limit !== 100) customers.setPage(0, 100);
    customers.fetch(0, 100);
    if (senders.limit !== 100) senders.setPage(0, 100);
    senders.fetch(0, 100);
  }, []);

  useEffect(() => {
    if (mode === 'edit' && id) {
      invoices.fetchOne(id).then(() => {
        const cur = invoices.current;
        if (cur) {
          setInvoiceNumber(cur.invoiceNumber);
          setInvoiceDate(isoToDateInput(cur.invoiceDate));
          setDueDate(isoToDateInput(cur.dueDate));
          setCurrency(cur.currency);
          setNotes(cur.notes || '');
          setCustomerId(cur.customer.id);
          setSenderId(cur.sender.id);
          setTaxRatePct(cur.taxRate * 100);
          setLineItems(cur.lineItems.map(li => ({ id: li.id, itemName: li.itemName, quantity: li.quantity, unitPrice: li.unitPrice, total: li.total })));
        }
      });
    }
  }, [mode, id]);

  const subtotal = useMemo(() => lineItems.reduce((sum, li) => sum + (toNumber(li.total) || (toNumber(li.quantity) * toNumber(li.unitPrice))), 0), [lineItems]);
  const taxRate = (taxRatePct || 0) / 100;
  const totalAmount = useMemo(() => subtotal * (1 + taxRate), [subtotal, taxRate]);

  const updateItem = (idx: number, patch: Partial<LineItem>) => {
    setLineItems(prev => prev.map((li, i) => i === idx ? recalc({ ...li, ...patch }) : li));
  };

  const recalc = (li: LineItem): LineItem => {
    const q = toNumber(li.quantity);
    const p = toNumber(li.unitPrice);
    const t = q * p;
    return { ...li, total: isNaN(t) ? 0 : t };
  };

  const addItem = () => setLineItems(prev => [...prev, emptyItem()]);
  const removeItem = (idx: number) => setLineItems(prev => prev.filter((_, i) => i !== idx));

  const canSave = invoiceNumber && invoiceDate && dueDate && customerId && senderId && lineItems.length > 0;

  const onSubmit = async (e: React.FormEvent) => {
    e.preventDefault();
    if (!canSave) return;
    setSaving(true);
    const payload: InvoiceUpsert = {
      invoiceNumber,
      invoiceDate: dateInputToIso(invoiceDate),
      dueDate: dateInputToIso(dueDate),
      currency,
      notes,
      customerId,
      senderId,
      subtotal: Number(subtotal.toFixed(2)),
      taxRate,
      totalAmount: Number(totalAmount.toFixed(2)),
      lineItems: lineItems.map(recalc),
    };

    try {
      if (mode === 'create') {
        const created = await invoices.create(payload);
        navigate(`/invoices/${created.id}`);
      } else if (mode === 'edit' && id) {
        const updated = await invoices.update(id, payload);
        navigate(`/invoices/${updated.id}`);
      }
    } finally {
      setSaving(false);
    }
  };

  if (mode === 'edit' && invoices.loading && !invoices.current) {
    return <Loading label="Loading invoice..." />;
  }

  return (
    <form onSubmit={onSubmit} className="space-y-6">
      <div className="flex items-center justify-between">
        <div>
          <h1 className="text-2xl font-semibold text-gray-900">{mode === 'create' ? 'New Invoice' : `Edit Invoice ${invoiceNumber}`}</h1>
          <p className="text-sm text-gray-600">Provide invoice details, select Customer and Sender, and add line items.</p>
        </div>
        <div className="flex items-center gap-2">
          <button type="button" onClick={() => navigate(-1)} className="px-3 py-2 rounded-md border border-gray-300 text-gray-700 hover:bg-gray-50">Cancel</button>
          <button disabled={!canSave || saving} type="submit" className="px-3 py-2 rounded-md bg-indigo-600 text-white hover:bg-indigo-700 disabled:opacity-50">
            {saving ? 'Saving...' : mode === 'create' ? 'Create Invoice' : 'Save Changes'}
          </button>
        </div>
      </div>

      <div className="grid grid-cols-1 lg:grid-cols-3 gap-6">
        <div className="lg:col-span-2 space-y-6">
          <div className="bg-white border border-gray-200 rounded-lg p-4">
            <div className="grid grid-cols-1 sm:grid-cols-2 gap-4">
              <TextInput id="invoiceNumber" label="Invoice Number" value={invoiceNumber} onChange={(e) => setInvoiceNumber(e.target.value)} required />
              <TextInput id="currency" label="Currency" value={currency} onChange={(e) => setCurrency(e.target.value)} required />
              <TextInput id="invoiceDate" label="Invoice Date" type="date" value={invoiceDate} onChange={(e) => setInvoiceDate(e.target.value)} required />
              <TextInput id="dueDate" label="Due Date" type="date" value={dueDate} onChange={(e) => setDueDate(e.target.value)} required />
              <SelectInput id="customerId" label="Customer" value={customerId} onChange={(e) => setCustomerId(e.target.value)} required>
                <option value="">Select customer...</option>
                {customers.list.map(c => (
                  <option key={c.id} value={c.id}>{c.companyName} — {c.customerName}</option>
                ))}
              </SelectInput>
              <SelectInput id="senderId" label="Sender" value={senderId} onChange={(e) => setSenderId(e.target.value)} required>
                <option value="">Select sender...</option>
                {senders.list.map(s => (
                  <option key={s.id} value={s.id}>{s.senderCompanyName} — {s.senderFullName}</option>
                ))}
              </SelectInput>
              <NumberInput id="taxRate" label="Tax Rate (%)" value={taxRatePct} onChange={(e) => setTaxRatePct(toNumber(e.target.value))} min={0} max={100} step={0.01} />
            </div>
            <TextArea id="notes" label="Notes" value={notes} onChange={(e) => setNotes(e.target.value)} rows={3} className="mt-4" />
          </div>

          <div className="bg-white border border-gray-200 rounded-lg">
            <div className="p-4 flex items-center justify-between">
              <h2 className="text-sm font-semibold text-gray-900">Line Items</h2>
              <button type="button" onClick={addItem} className="px-3 py-1.5 rounded-md border border-gray-300 text-gray-700 hover:bg-gray-50">Add Item</button>
            </div>
            <div className="divide-y divide-gray-100">
              {lineItems.map((li, idx) => (
                <div key={idx} className="p-4 grid grid-cols-12 gap-4">
                  <div className="col-span-12 md:col-span-5">
                    <TextInput placeholder="Item name" value={li.itemName} onChange={(e) => updateItem(idx, { itemName: e.target.value })} />
                  </div>
                  <div className="col-span-6 md:col-span-2">
                    <NumberInput placeholder="Qty" value={li.quantity} onChange={(e) => updateItem(idx, { quantity: toNumber(e.target.value) })} min={0} step={1} />
                  </div>
                  <div className="col-span-6 md:col-span-2">
                    <NumberInput placeholder="Unit Price" value={li.unitPrice} onChange={(e) => updateItem(idx, { unitPrice: toNumber(e.target.value) })} min={0} step={0.01} />
                  </div>
                  <div className="col-span-8 md:col-span-2 self-center">
                    <TextInput readOnly value={(toNumber(li.quantity) * toNumber(li.unitPrice)).toFixed(2)} />
                  </div>
                  <div className="col-span-4 md:col-span-1 flex items-center justify-end whitespace-nowrap md:self-center">
                    <button type="button" onClick={() => removeItem(idx)} className="mt-2 md:mt-0 px-2 py-1 rounded-md border border-red-200 text-red-600 hover:bg-red-50">Remove</button>
                  </div>
                </div>
              ))}
            </div>
          </div>
        </div>
        <div className="space-y-6">
          <div className="bg-white border border-gray-200 rounded-lg p-4">
            <h2 className="text-sm font-semibold text-gray-900 mb-3">Summary</h2>
            <div className="space-y-2 text-sm">
              <div className="flex items-center justify-between text-gray-700"><span>Subtotal</span><span>{subtotal.toFixed(2)}</span></div>
              <div className="flex items-center justify-between text-gray-700"><span>Tax Rate</span><span>{taxRatePct.toFixed(2)}%</span></div>
              <div className="flex items-center justify-between font-semibold text-gray-900"><span>Total</span><span>{totalAmount.toFixed(2)}</span></div>
            </div>
          </div>
        </div>
      </div>
    </form>
  );
};

export default InvoiceForm;
