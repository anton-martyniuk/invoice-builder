import React, { useEffect, useMemo, useState } from 'react';
import { Link } from 'react-router-dom';
import { useSendersStore } from '../../store/sendersStore';
import Pagination from '../../components/Pagination';
import ConfirmDialog from '../../components/ConfirmDialog';
import Loading from '../../components/Loading';
import EmptyState from '../../components/EmptyState';
import { EyeIcon, PencilSquareIcon, PlusIcon, TrashIcon } from '@heroicons/react/24/outline';

const SendersList: React.FC = () => {
  const { list, total, offset, limit, loading, error, fetch, setPage, remove } = useSendersStore();
  const [toDelete, setToDelete] = useState<string | null>(null);

  useEffect(() => { fetch(); }, [offset, limit]);

  const onChangePage = (nextOffset: number, nextLimit: number) => {
    setPage(nextOffset, nextLimit);
    fetch(nextOffset, nextLimit);
  };

  const rows = useMemo(() => list, [list]);

  return (
    <div className="space-y-6">
      <div className="flex items-center justify-between">
        <div>
          <h1 className="text-2xl font-semibold text-gray-900">Senders</h1>
          <p className="text-sm text-gray-600">Manage your company sender profiles.</p>
        </div>
        <Link to="/senders/new" className="inline-flex items-center gap-2 px-3 py-2 rounded-md bg-indigo-600 text-white text-sm hover:bg-indigo-700">
          <PlusIcon className="h-4 w-4" /> New Sender
        </Link>
      </div>

      {loading && rows.length === 0 ? (
        <Loading label="Loading senders..." />
      ) : rows.length === 0 ? (
        <EmptyState title="No senders" description="Create your first sender." actionText="New Sender" actionTo="/senders/new" />
      ) : (
        <div className="bg-white border border-gray-200 rounded-lg overflow-hidden">
          <table className="min-w-full divide-y divide-gray-200">
            <thead className="bg-gray-50">
              <tr>
                <th className="px-4 py-3 text-left text-xs font-medium text-gray-500 uppercase tracking-wider">Company</th>
                <th className="px-4 py-3 text-left text-xs font-medium text-gray-500 uppercase tracking-wider">Full Name</th>
                <th className="px-4 py-3 text-left text-xs font-medium text-gray-500 uppercase tracking-wider">Tax/VAT</th>
                <th className="px-4 py-3 text-right text-xs font-medium text-gray-500 uppercase tracking-wider">Actions</th>
              </tr>
            </thead>
            <tbody className="divide-y divide-gray-100 bg-white">
              {rows.map((s) => (
                <tr key={s.id} className="hover:bg-gray-50">
                  <td className="px-4 py-3 text-sm text-gray-900 font-medium">{s.senderCompanyName}</td>
                  <td className="px-4 py-3 text-sm text-gray-700">{s.senderFullName}</td>
                  <td className="px-4 py-3 text-sm text-gray-700">{s.senderTaxVatId}</td>
                  <td className="px-4 py-3 text-sm text-right">
                    <div className="flex items-center gap-2 justify-end">
                      <Link to={`/senders/${s.id}`} className="inline-flex items-center px-2 py-1 rounded-md border border-gray-300 text-gray-700 hover:bg-gray-50" title="View">
                        <EyeIcon className="h-4 w-4" />
                      </Link>
                      <Link to={`/senders/${s.id}/edit`} className="inline-flex items-center px-2 py-1 rounded-md border border-gray-300 text-gray-700 hover:bg-gray-50" title="Edit">
                        <PencilSquareIcon className="h-4 w-4" />
                      </Link>
                      <button onClick={() => setToDelete(s.id!)} className="inline-flex items-center px-2 py-1 rounded-md border border-red-200 text-red-600 hover:bg-red-50" title="Delete">
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

      {error && <div className="p-3 rounded-md bg-red-50 border border-red-200 text-sm text-red-700">{error}</div>}

      <ConfirmDialog
        open={Boolean(toDelete)}
        title="Delete sender?"
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

export default SendersList;
