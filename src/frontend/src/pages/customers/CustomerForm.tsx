import React, { useEffect, useState } from 'react';
import { useNavigate, useParams } from 'react-router-dom';
import { TextArea, TextInput } from '../../components/Form';
import Loading from '../../components/Loading';
import { useCustomersStore } from '../../store/customersStore';

type Props = { mode: 'create' | 'edit' };

const CustomerForm: React.FC<Props> = ({ mode }) => {
  const navigate = useNavigate();
  const { id } = useParams();
  const store = useCustomersStore();

  const [companyName, setCompanyName] = useState('');
  const [customerName, setCustomerName] = useState('');
  const [customerAddress, setCustomerAddress] = useState('');
  const [postalCode, setPostalCode] = useState('');
  const [customerEmail, setCustomerEmail] = useState('');
  const [customerTaxVatId, setCustomerTaxVatId] = useState('');
  const [saving, setSaving] = useState(false);

  useEffect(() => {
    if (mode === 'edit' && id) {
      store.fetchOne(id).then(() => {
        const c = store.current;
        if (c) {
          setCompanyName(c.companyName);
          setCustomerName(c.customerName);
          setCustomerAddress(c.customerAddress);
          setPostalCode(c.postalCode);
          setCustomerEmail(c.customerEmail);
          setCustomerTaxVatId(c.customerTaxVatId);
        }
      });
    }
  }, [mode, id]);

  const canSave = companyName && customerName && customerEmail;

  const onSubmit = async (e: React.FormEvent) => {
    e.preventDefault();
    if (!canSave) return;
    setSaving(true);
    const payload = { companyName, customerName, customerAddress, postalCode, customerEmail, customerTaxVatId };
    try {
      if (mode === 'create') {
        const created = await store.create(payload);
        navigate(`/customers/${created.id}`);
      } else if (mode === 'edit' && id) {
        const updated = await store.update(id, payload);
        navigate(`/customers/${updated.id}`);
      }
    } finally {
      setSaving(false);
    }
  };

  if (mode === 'edit' && store.loading && !store.current) {
    return <Loading label="Loading customer..." />;
  }

  return (
    <form onSubmit={onSubmit} className="space-y-6">
      <div className="flex items-center justify-between">
        <div>
          <h1 className="text-2xl font-semibold text-gray-900">{mode === 'create' ? 'New Customer' : `Edit ${companyName || 'Customer'}`}</h1>
          <p className="text-sm text-gray-600">Provide customer details.</p>
        </div>
        <div className="flex items-center gap-2">
          <button type="button" onClick={() => navigate(-1)} className="px-3 py-2 rounded-md border border-gray-300 text-gray-700 hover:bg-gray-50">Cancel</button>
          <button disabled={!canSave || saving} type="submit" className="px-3 py-2 rounded-md bg-indigo-600 text-white hover:bg-indigo-700 disabled:opacity-50">
            {saving ? 'Saving...' : mode === 'create' ? 'Create Customer' : 'Save Changes'}
          </button>
        </div>
      </div>

      <div className="bg-white border border-gray-200 rounded-lg p-4 grid grid-cols-1 sm:grid-cols-2 gap-4">
        <TextInput id="companyName" label="Company Name" value={companyName} onChange={(e) => setCompanyName(e.target.value)} required />
        <TextInput id="customerName" label="Customer Name" value={customerName} onChange={(e) => setCustomerName(e.target.value)} required />
        <TextInput id="customerEmail" label="Email" type="email" value={customerEmail} onChange={(e) => setCustomerEmail(e.target.value)} required />
        <TextInput id="postalCode" label="Postal Code" value={postalCode} onChange={(e) => setPostalCode(e.target.value)} />
        <TextInput id="customerTaxVatId" label="Tax/VAT ID" value={customerTaxVatId} onChange={(e) => setCustomerTaxVatId(e.target.value)} />
        <div className="sm:col-span-2">
          <TextArea id="customerAddress" label="Address" value={customerAddress} onChange={(e) => setCustomerAddress(e.target.value)} rows={3} />
        </div>
      </div>
    </form>
  );
};

export default CustomerForm;
