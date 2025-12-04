import React, { useEffect } from 'react';
import { Link, useParams } from 'react-router-dom';
import { useInvoicesStore } from '../../store/invoicesStore';
import Loading from '../../components/Loading';
import { formatCurrency, formatDate } from '../../utils/format';
import { ArrowDownTrayIcon, PencilSquareIcon } from '@heroicons/react/24/outline';

const Row: React.FC<{ label: string; value?: React.ReactNode }> = ({ label, value }) => (
  <div className="grid grid-cols-12 py-2">
    <div className="col-span-4 text-sm text-gray-500">{label}</div>
    <div className="col-span-8 text-sm text-gray-900">{value ?? '-'}</div>
  </div>
);

const InvoiceDetails: React.FC = () => {
  const { id } = useParams();
  const { current, fetchOne, loading, download } = useInvoicesStore();

  useEffect(() => {
    if (id) fetchOne(id);
  }, [id]);

  if (!current || loading) {
    return <Loading label="Loading invoice..." />;
  }

  return (
    <div className="space-y-6">
      <div className="flex items-center justify-between">
        <div>
          <h1 className="text-2xl font-semibold text-gray-900">Invoice {current.invoiceNumber}</h1>
          <p className="text-sm text-gray-600">Issued on {formatDate(current.invoiceDate)} • Due {formatDate(current.dueDate)}</p>
        </div>
        <div className="flex items-center gap-2">
          <button onClick={() => id && download(id)} className="inline-flex items-center gap-2 px-3 py-2 rounded-md border border-gray-300 text-gray-700 hover:bg-gray-50">
            <ArrowDownTrayIcon className="h-4 w-4" /> Download PDF
          </button>
          <Link to={`/invoices/${id}/edit`} className="inline-flex items-center gap-2 px-3 py-2 rounded-md bg-indigo-600 text-white hover:bg-indigo-700">
            <PencilSquareIcon className="h-4 w-4" /> Edit
          </Link>
        </div>
      </div>

      <div className="grid grid-cols-1 lg:grid-cols-3 gap-6">
        <div className="lg:col-span-2 space-y-6">
          <div className="bg-white border border-gray-200 rounded-lg p-4">
            <h2 className="text-sm font-semibold text-gray-900 mb-3">Customer</h2>
            <Row label="Company" value={current.customer.companyName} />
            <Row label="Contact" value={current.customer.customerName} />
            <Row label="Email" value={current.customer.customerEmail} />
            <Row label="Address" value={current.customer.customerAddress} />
            <Row label="Postal Code" value={current.customer.postalCode} />
            <Row label="Tax/VAT" value={current.customer.customerTaxVatId} />
          </div>

          <div className="bg-white border border-gray-200 rounded-lg p-4">
            <h2 className="text-sm font-semibold text-gray-900 mb-3">Sender</h2>
            <Row label="Company" value={current.sender.senderCompanyName} />
            <Row label="Full Name" value={current.sender.senderFullName} />
            <Row label="Address" value={current.sender.senderAddress} />
            <Row label="Tax/VAT" value={current.sender.senderTaxVatId} />
            <Row label="Bank" value={current.sender.bankDetails} />
          </div>

          <div className="bg-white border border-gray-200 rounded-lg overflow-hidden">
            <div className="p-4">
              <h2 className="text-sm font-semibold text-gray-900">Line Items</h2>
            </div>
            <table className="min-w-full divide-y divide-gray-200">
              <thead className="bg-gray-50">
                <tr>
                  <th className="px-4 py-2 text-left text-xs font-medium text-gray-500 uppercase tracking-wider">Item</th>
                  <th className="px-4 py-2 text-right text-xs font-medium text-gray-500 uppercase tracking-wider">Qty</th>
                  <th className="px-4 py-2 text-right text-xs font-medium text-gray-500 uppercase tracking-wider">Unit Price</th>
                  <th className="px-4 py-2 text-right text-xs font-medium text-gray-500 uppercase tracking-wider">Total</th>
                </tr>
              </thead>
              <tbody className="divide-y divide-gray-100 bg-white">
                {current.lineItems.map((li, idx) => (
                  <tr key={li.id ?? idx}>
                    <td className="px-4 py-2 text-sm text-gray-900">{li.itemName}</td>
                    <td className="px-4 py-2 text-sm text-gray-700 text-right">{li.quantity}</td>
                    <td className="px-4 py-2 text-sm text-gray-700 text-right">{formatCurrency(li.unitPrice, current.currency)}</td>
                    <td className="px-4 py-2 text-sm text-gray-900 text-right">{formatCurrency(li.total, current.currency)}</td>
                  </tr>
                ))}
              </tbody>
            </table>
          </div>
        </div>
        <div className="space-y-6">
          <div className="bg-white border border-gray-200 rounded-lg p-4">
            <h2 className="text-sm font-semibold text-gray-900 mb-3">Summary</h2>
            <div className="space-y-2 text-sm">
              <div className="flex items-center justify-between text-gray-700"><span>Subtotal</span><span>{formatCurrency(current.subtotal, current.currency)}</span></div>
              <div className="flex items-center justify-between text-gray-700"><span>Tax Rate</span><span>{(current.taxRate * 100).toFixed(0)}%</span></div>
              <div className="flex items-center justify-between font-semibold text-gray-900"><span>Total</span><span>{formatCurrency(current.totalAmount, current.currency)}</span></div>
            </div>
          </div>

          {current.notes && (
            <div className="bg-white border border-gray-200 rounded-lg p-4">
              <h2 className="text-sm font-semibold text-gray-900 mb-2">Notes</h2>
              <p className="text-sm text-gray-700 whitespace-pre-wrap">{current.notes}</p>
            </div>
          )}
        </div>
      </div>
    </div>
  );
};

export default InvoiceDetails;
