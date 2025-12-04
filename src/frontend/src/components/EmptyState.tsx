import React from 'react';
import { Link } from 'react-router-dom';

type Props = {
  title: string;
  description?: string;
  actionText?: string;
  actionTo?: string;
};

const EmptyState: React.FC<Props> = ({ title, description, actionText, actionTo }) => {
  return (
    <div className="border border-dashed border-gray-300 rounded-lg p-10 text-center bg-white">
      <div className="mx-auto h-12 w-12 rounded-full bg-indigo-50 text-indigo-600 grid place-items-center text-xl font-bold">ℹ️</div>
      <h3 className="mt-4 text-lg font-semibold text-gray-900">{title}</h3>
      {description && <p className="mt-1 text-sm text-gray-600">{description}</p>}
      {actionText && actionTo && (
        <div className="mt-6">
          <Link
            to={actionTo}
            className="inline-flex items-center px-4 py-2 rounded-md bg-indigo-600 text-white text-sm font-medium hover:bg-indigo-700"
          >
            {actionText}
          </Link>
        </div>
      )}
    </div>
  );
};

export default EmptyState;
