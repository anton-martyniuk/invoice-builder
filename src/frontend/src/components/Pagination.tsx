import React from 'react';

type Props = {
  total: number;
  offset: number;
  limit: number;
  onChange: (nextOffset: number, nextLimit: number) => void;
};

const Pagination: React.FC<Props> = ({ total, offset, limit, onChange }) => {
  const page = Math.floor(offset / limit) + 1;
  const totalPages = Math.max(1, Math.ceil(total / limit));
  const canPrev = offset > 0;
  const canNext = offset + limit < total;

  return (
    <div className="mt-4 flex items-center justify-between">
      <div className="text-sm text-gray-600">
        Page {page} of {totalPages} • {total} items
      </div>
      <div className="flex items-center gap-2">
        <select
          className="border border-gray-300 rounded-md px-2 py-1 text-sm"
          value={limit}
          onChange={(e) => onChange(0, Number(e.target.value))}
        >
          {[10, 20, 50, 100].map((l) => (
            <option key={l} value={l}>
              {l} / page
            </option>
          ))}
        </select>
        <button
          className={`px-3 py-1.5 text-sm rounded-md border ${
            canPrev ? 'bg-white hover:bg-gray-50 border-gray-300 text-gray-700' : 'bg-gray-100 border-gray-200 text-gray-400 cursor-not-allowed'
          }`}
          onClick={() => canPrev && onChange(Math.max(0, offset - limit), limit)}
          disabled={!canPrev}
        >
          Previous
        </button>
        <button
          className={`px-3 py-1.5 text-sm rounded-md border ${
            canNext ? 'bg-white hover:bg-gray-50 border-gray-300 text-gray-700' : 'bg-gray-100 border-gray-200 text-gray-400 cursor-not-allowed'
          }`}
          onClick={() => canNext && onChange(offset + limit, limit)}
          disabled={!canNext}
        >
          Next
        </button>
      </div>
    </div>
  );
};

export default Pagination;
