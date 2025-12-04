import React, { useEffect, useState } from 'react';
import { useNavigate, useParams } from 'react-router-dom';
import { TextArea, TextInput } from '../../components/Form';
import Loading from '../../components/Loading';
import { useSendersStore } from '../../store/sendersStore';

type Props = { mode: 'create' | 'edit' };

const SenderForm: React.FC<Props> = ({ mode }) => {
  const navigate = useNavigate();
  const { id } = useParams();
  const store = useSendersStore();

  const [senderCompanyName, setSenderCompanyName] = useState('');
  const [senderFullName, setSenderFullName] = useState('');
  const [senderAddress, setSenderAddress] = useState('');
  const [senderTaxVatId, setSenderTaxVatId] = useState('');
  const [bankDetails, setBankDetails] = useState('');
  const [saving, setSaving] = useState(false);

  useEffect(() => {
    if (mode === 'edit' && id) {
      store.fetchOne(id).then(() => {
        const s = store.current;
        if (s) {
          setSenderCompanyName(s.senderCompanyName);
          setSenderFullName(s.senderFullName);
          setSenderAddress(s.senderAddress);
          setSenderTaxVatId(s.senderTaxVatId);
          setBankDetails(s.bankDetails);
        }
      });
    }
  }, [mode, id]);

  const canSave = senderCompanyName && senderFullName;

  const onSubmit = async (e: React.FormEvent) => {
    e.preventDefault();
    if (!canSave) return;
    setSaving(true);
    const payload = { senderCompanyName, senderFullName, senderAddress, senderTaxVatId, bankDetails };
    try {
      if (mode === 'create') {
        const created = await store.create(payload);
        navigate(`/senders/${created.id}`);
      } else if (mode === 'edit' && id) {
        const updated = await store.update(id, payload);
        navigate(`/senders/${updated.id}`);
      }
    } finally {
      setSaving(false);
    }
  };

  if (mode === 'edit' && store.loading && !store.current) {
    return <Loading label="Loading sender..." />;
  }

  return (
    <form onSubmit={onSubmit} className="space-y-6">
      <div className="flex items-center justify-between">
        <div>
          <h1 className="text-2xl font-semibold text-gray-900">{mode === 'create' ? 'New Sender' : `Edit ${senderCompanyName || 'Sender'}`}</h1>
          <p className="text-sm text-gray-600">Provide sender details.</p>
        </div>
        <div className="flex items-center gap-2">
          <button type="button" onClick={() => navigate(-1)} className="px-3 py-2 rounded-md border border-gray-300 text-gray-700 hover:bg-gray-50">Cancel</button>
          <button disabled={!canSave || saving} type="submit" className="px-3 py-2 rounded-md bg-indigo-600 text-white hover:bg-indigo-700 disabled:opacity-50">
            {saving ? 'Saving...' : mode === 'create' ? 'Create Sender' : 'Save Changes'}
          </button>
        </div>
      </div>

      <div className="bg-white border border-gray-200 rounded-lg p-4 grid grid-cols-1 sm:grid-cols-2 gap-4">
        <TextInput id="senderCompanyName" label="Company Name" value={senderCompanyName} onChange={(e) => setSenderCompanyName(e.target.value)} required />
        <TextInput id="senderFullName" label="Full Name" value={senderFullName} onChange={(e) => setSenderFullName(e.target.value)} required />
        <TextInput id="senderTaxVatId" label="Tax/VAT ID" value={senderTaxVatId} onChange={(e) => setSenderTaxVatId(e.target.value)} />
        <TextInput id="bankDetails" label="Bank Details" value={bankDetails} onChange={(e) => setBankDetails(e.target.value)} />
        <div className="sm:col-span-2">
          <TextArea id="senderAddress" label="Address" value={senderAddress} onChange={(e) => setSenderAddress(e.target.value)} rows={3} />
        </div>
      </div>
    </form>
  );
};

export default SenderForm;
