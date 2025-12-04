import React, { useEffect } from 'react';
import { Link, useParams } from 'react-router-dom';
import Loading from '../../components/Loading';
import { useSendersStore } from '../../store/sendersStore';
import { PencilSquareIcon } from '@heroicons/react/24/outline';

const Row: React.FC<{ label: string; value?: React.ReactNode }> = ({ label, value }) => (
  <div className="grid grid-cols-12 py-2">
    <div className="col-span-4 text-sm text-gray-500">{label}</div>
    <div className="col-span-8 text-sm text-gray-900">{value ?? '-'}</div>
  </div>
);

const SenderDetails: React.FC = () => {
  const { id } = useParams();
  const { current, fetchOne, loading } = useSendersStore();

  useEffect(() => { if (id) fetchOne(id); }, [id]);

  if (!current || loading) return <Loading label="Loading sender..." />;

  return (
    <div className="space-y-6">
      <div className="flex items-center justify-between">
        <div>
          <h1 className="text-2xl font-semibold text-gray-900">{current.senderCompanyName}</h1>
          <p className="text-sm text-gray-600">{current.senderFullName}</p>
        </div>
        <Link to={`/senders/${id}/edit`} className="inline-flex items-center gap-2 px-3 py-2 rounded-md bg-indigo-600 text-white hover:bg-indigo-700">
          <PencilSquareIcon className="h-4 w-4" /> Edit
        </Link>
      </div>

      <div className="bg-white border border-gray-200 rounded-lg p-4">
        <Row label="Company" value={current.senderCompanyName} />
        <Row label="Full Name" value={current.senderFullName} />
        <Row label="Address" value={current.senderAddress} />
        <Row label="Tax/VAT" value={current.senderTaxVatId} />
        <Row label="Bank Details" value={current.bankDetails} />
      </div>
    </div>
  );
};

export default SenderDetails;
