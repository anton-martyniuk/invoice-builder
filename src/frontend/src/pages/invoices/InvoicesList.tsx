import React, { useEffect, useMemo, useState } from 'react';
import { Link } from 'react-router-dom';
import { useInvoicesStore } from '../../store/invoicesStore';
import Pagination from '../../components/Pagination';
import ConfirmDialog from '../../components/ConfirmDialog';
import Loading from '../../components/Loading';
import EmptyState from '../../components/EmptyState';
import { formatCurrency, formatDate } from '../../utils/format';
import { ArrowDownTrayIcon, EyeIcon, PencilSquareIcon, TrashIcon, PlusIcon } from '@heroicons/react/24/outline';

const InvoicesList: React.FC = () => {
  const { list, total, offset, limit, loading, error, fetch, setPage, remove, download } = useInvoicesStore();
  const [toDelete, setToDelete] = useState<string | null>(null);

  useEffect(() => {
    fetch();
  }, [offset, limit]);

  const onChangePage = (nextOffset: number, nextLimit: number) => {
    setPage(nextOffset, nextLimit);
    fetch(nextOffset, nextLimit);
  };

  const rows = useMemo(() => list, [list]);

  return (
    <div className="space-y-6">
      <div className="flex items-center justify-between">
        <div>
          <h1 className="text-2xl font-semibold text-gray-900">Invoices</h1>
          <p className="text-sm text-gray-600">Manage invoices, view details, and download PDFs.</p>
        </div>
        <Link to="/invoices/new" className="inline-flex items-center gap-2 px-3 py-2 rounded-md bg-indigo-600 text-white text-sm hover:bg-indigo-700">
          <PlusIcon className="h-4 w-4" /> New Invoice
        </Link>
      </div>

      {loading && rows.length === 0 ? (
        <Loading label="Loading invoices..." />
      ) : rows.length === 0 ? (
        <EmptyState title="No invoices yet" description="Create your first invoice to get started." actionText="New Invoice" actionTo="/invoices/new" />
      ) : (
        <div className="bg-white border border-gray-200 rounded-lg overflow-hidden">
          <table className="min-w-full divide-y divide-gray-200">
            <thead className="bg-gray-50">
              <tr>
                <th className="px-4 py-3 text-left text-xs font-medium text-gray-500 uppercase tracking-wider">Invoice #</th>
                <th className="px-4 py-3 text-left text-xs font-medium text-gray-500 uppercase tracking-wider">Invoice Date</th>
                <th className="px-4 py-3 text-left text-xs font-medium text-gray-500 uppercase tracking-wider">Due Date</th>
                <th className="px-4 py-3 text-right text-xs font-medium text-gray-500 uppercase tracking-wider">Total</th>
                <th className="px-4 py-3 text-right text-xs font-medium text-gray-500 uppercase tracking-wider">Actions</th>
              </tr>
            </thead>
            <tbody className="divide-y divide-gray-100 bg-white">
              {rows.map((inv) => (
                <tr key={inv.id} className="hover:bg-gray-50">
                  <td className="px-4 py-3 text-sm text-gray-900 font-medium">{inv.invoiceNumber}</td>
                  <td className="px-4 py-3 text-sm text-gray-700">{formatDate(inv.invoiceDate)}</td>
                  <td className="px-4 py-3 text-sm text-gray-700">{formatDate(inv.dueDate)}</td>
                  <td className="px-4 py-3 text-sm text-gray-900 text-right">{formatCurrency(inv.totalAmount)}</td>
                  <td className="px-4 py-3 text-sm text-right">
                    <div className="flex items-center gap-2 justify-end">
                      <Link to={`/invoices/${inv.id}`} className="inline-flex items-center px-2 py-1 rounded-md border border-gray-300 text-gray-700 hover:bg-gray-50" title="View">
                        <EyeIcon className="h-4 w-4" />
                      </Link>
                      <Link to={`/invoices/${inv.id}/edit`} className="inline-flex items-center px-2 py-1 rounded-md border border-gray-300 text-gray-700 hover:bg-gray-50" title="Edit">
                        <PencilSquareIcon className="h-4 w-4" />
                      </Link>
                      <button onClick={() => download(inv.id)} className="inline-flex items-center px-2 py-1 rounded-md border border-gray-300 text-gray-700 hover:bg-gray-50" title="Download PDF">
                        <ArrowDownTrayIcon className="h-4 w-4" />
                      </button>
                      <button onClick={() => setToDelete(inv.id)} className="inline-flex items-center px-2 py-1 rounded-md border border-red-200 text-red-600 hover:bg-red-50" title="Delete">
                        <TrashIcon className="h-4 w-4" />
                      </button>
                    </div>
                  </td>
                </tr>
              ))}
            </tbody>
          </table>
          <div className="px-4 pb-4">
            <Pagination total={total} offset={offset} limit={limit} onChange={onChangePage} />
          </div>
        </div>
      )}

      {error && (
        <div className="p-3 rounded-md bg-red-50 border border-red-200 text-sm text-red-700">{error}</div>
      )}

      <ConfirmDialog
        open={Boolean(toDelete)}
        title="Delete invoice?"
        description="This action cannot be undone."
        confirmText="Delete"
        onCancel={() => setToDelete(null)}
        onConfirm={async () => {
          if (toDelete) {
            await remove(toDelete);
            setToDelete(null);
          }
        }}
      />
    </div>
  );
};

export default InvoicesList;
