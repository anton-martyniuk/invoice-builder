import React, { useEffect } from 'react';
import { Link } from 'react-router-dom';
import { useInvoicesStore } from '../store/invoicesStore';
import Loading from '../components/Loading';
import { ArrowDownTrayIcon, EyeIcon, PencilSquareIcon, TrashIcon } from '@heroicons/react/24/outline';

const InvoicesList: React.FC = () => {
  const { list, total, offset, limit, loading, error, fetch, setPage, remove, download, clearError } = useInvoicesStore();

  useEffect(() => {
    fetch(offset, limit);
    // eslint-disable-next-line react-hooks/exhaustive-deps
  }, [offset, limit]);

  const page = Math.floor(offset / limit) + 1;
  const totalPages = Math.max(1, Math.ceil(total / limit));

  const goToPage = (p: number) => {
    const newOffset = Math.max(0, (p - 1) * limit);
    setPage(newOffset, limit);
    fetch(newOffset, limit);
  };

  const onDelete = async (id?: string) => {
    if (!id) return;
    if (confirm('Delete this invoice? This action cannot be undone.')) {
      await remove(id);
    }
  };

  return (
    <div>
      <div className="flex items-center justify-between">
        <h1 className="text-2xl font-semibold text-gray-900">Invoices</h1>
        <Link to="/invoices/new" className="inline-flex items-center gap-2 rounded-md bg-indigo-600 text-white text-sm px-4 py-2 hover:bg-indigo-700">
          New Invoice
        </Link>
      </div>

      {error && (
        <div className="mt-4 rounded-md border border-red-200 bg-red-50 text-red-700 p-3 flex items-start justify-between">
          <div>{error}</div>
          <button className="text-sm underline" onClick={clearError}>Dismiss</button>
        </div>
      )}

      <div className="mt-6 bg-white shadow-sm ring-1 ring-gray-200 rounded-lg overflow-hidden">
        <div className="min-w-full overflow-x-auto">
          <table className="min-w-full divide-y divide-gray-200">
            <thead className="bg-gray-50">
              <tr>
                <th className="px-4 py-3 text-left text-xs font-medium text-gray-500 uppercase tracking-wider">Invoice #</th>
                <th className="px-4 py-3 text-left text-xs font-medium text-gray-500 uppercase tracking-wider">Date</th>
                <th className="px-4 py-3 text-left text-xs font-medium text-gray-500 uppercase tracking-wider">Due</th>
                <th className="px-4 py-3 text-left text-xs font-medium text-gray-500 uppercase tracking-wider">Currency</th>
                <th className="px-4 py-3 text-right text-xs font-medium text-gray-500 uppercase tracking-wider">Total</th>
                <th className="px-4 py-3 text-right text-xs font-medium text-gray-500 uppercase tracking-wider">Actions</th>
              </tr>
            </thead>
            <tbody className="bg-white divide-y divide-gray-100">
              {loading && (
                <tr>
                  <td colSpan={6}><Loading label="Loading invoices..." /></td>
                </tr>
              )}
              {!loading && list.length === 0 && (
                <tr>
                  <td colSpan={6} className="px-4 py-10 text-center text-gray-500">No invoices found.</td>
                </tr>
              )}
              {!loading && list.map((inv) => (
                <tr key={inv.id} className="hover:bg-gray-50">
                  <td className="px-4 py-3 whitespace-nowrap font-medium text-gray-900">{inv.invoiceNumber}</td>
                  <td className="px-4 py-3 whitespace-nowrap text-gray-700">{new Date(inv.invoiceDate).toLocaleDateString()}</td>
                  <td className="px-4 py-3 whitespace-nowrap text-gray-700">{new Date(inv.dueDate).toLocaleDateString()}</td>
                  <td className="px-4 py-3 whitespace-nowrap text-gray-700">{inv.currency}</td>
                  <td className="px-4 py-3 whitespace-nowrap text-right text-gray-900">{inv.totalAmount.toLocaleString(undefined, { style: 'currency', currency: inv.currency })}</td>
                  <td className="px-4 py-3 whitespace-nowrap">
                    <div className="flex items-center justify-end gap-2">
                      {inv.id && (
                        <>
                          <Link title="View" to={`/invoices/${inv.id}`} className="p-2 rounded-md hover:bg-gray-100">
                            <EyeIcon className="h-5 w-5 text-gray-600" />
                          </Link>
                          <Link title="Edit" to={`/invoices/${inv.id}/edit`} className="p-2 rounded-md hover:bg-gray-100">
                            <PencilSquareIcon className="h-5 w-5 text-gray-600" />
                          </Link>
                          <button title="Download PDF" onClick={() => download(inv.id!)} className="p-2 rounded-md hover:bg-gray-100">
                            <ArrowDownTrayIcon className="h-5 w-5 text-gray-600" />
                          </button>
                          <button title="Delete" onClick={() => onDelete(inv.id)} className="p-2 rounded-md hover:bg-red-50">
                            <TrashIcon className="h-5 w-5 text-red-600" />
                          </button>
                        </>
                      )}
                    </div>
                  </td>
                </tr>
              ))}
            </tbody>
          </table>
        </div>
      </div>

      <div className="mt-4 flex items-center justify-between">
        <div className="text-sm text-gray-600">Page {page} of {totalPages} · {total} total</div>
        <div className="flex items-center gap-2">
          <button disabled={page <= 1} onClick={() => goToPage(page - 1)} className="px-3 py-1.5 text-sm rounded-md border border-gray-300 disabled:opacity-50">Previous</button>
          <button disabled={page >= totalPages} onClick={() => goToPage(page + 1)} className="px-3 py-1.5 text-sm rounded-md border border-gray-300 disabled:opacity-50">Next</button>
        </div>
      </div>
    </div>
  );
};

export default InvoicesList;
