import React from 'react';

export const Label: React.FC<{ htmlFor?: string; children: React.ReactNode }>= ({ htmlFor, children }) => (
  <label htmlFor={htmlFor} className="block text-sm font-medium text-gray-700">
    {children}
  </label>
);

type BaseProps = {
  label?: string;
  error?: string;
  hint?: string;
  className?: string;
};

export const TextInput: React.FC<
  BaseProps & React.InputHTMLAttributes<HTMLInputElement>
> = ({ label, error, hint, className = '', id, ...rest }) => (
  <div className={`space-y-1 ${className}`}>
    {label && <Label htmlFor={id}>{label}</Label>}
    <input
      id={id}
      {...rest}
      className={`w-full rounded-md border ${
        error ? 'border-red-300 focus:ring-red-500' : 'border-gray-300 focus:ring-indigo-500'
      } px-3 py-2 text-sm focus:outline-none focus:ring-2`}
    />
    {hint && !error && <p className="text-xs text-gray-500">{hint}</p>}
    {error && <p className="text-xs text-red-600">{error}</p>}
  </div>
);

export const NumberInput: React.FC<
  BaseProps & React.InputHTMLAttributes<HTMLInputElement>
> = ({ label, error, hint, className = '', id, ...rest }) => (
  <div className={`space-y-1 ${className}`}>
    {label && <Label htmlFor={id}>{label}</Label>}
    <input
      id={id}
      type="number"
      step="any"
      {...rest}
      className={`w-full rounded-md border ${
        error ? 'border-red-300 focus:ring-red-500' : 'border-gray-300 focus:ring-indigo-500'
      } px-3 py-2 text-sm focus:outline-none focus:ring-2`}
    />
    {hint && !error && <p className="text-xs text-gray-500">{hint}</p>}
    {error && <p className="text-xs text-red-600">{error}</p>}
  </div>
);

export const SelectInput: React.FC<
  BaseProps & React.SelectHTMLAttributes<HTMLSelectElement>
> = ({ label, error, hint, className = '', id, children, ...rest }) => (
  <div className={`space-y-1 ${className}`}>
    {label && <Label htmlFor={id}>{label}</Label>}
    <select
      id={id}
      {...rest}
      className={`w-full rounded-md border ${
        error ? 'border-red-300 focus:ring-red-500' : 'border-gray-300 focus:ring-indigo-500'
      } px-3 py-2 text-sm focus:outline-none focus:ring-2 bg-white`}
    >
      {children}
    </select>
    {hint && !error && <p className="text-xs text-gray-500">{hint}</p>}
    {error && <p className="text-xs text-red-600">{error}</p>}
  </div>
);

export const TextArea: React.FC<
  BaseProps & React.TextareaHTMLAttributes<HTMLTextAreaElement>
> = ({ label, error, hint, className = '', id, ...rest }) => (
  <div className={`space-y-1 ${className}`}>
    {label && <Label htmlFor={id}>{label}</Label>}
    <textarea
      id={id}
      {...rest}
      className={`w-full rounded-md border ${
        error ? 'border-red-300 focus:ring-red-500' : 'border-gray-300 focus:ring-indigo-500'
      } px-3 py-2 text-sm focus:outline-none focus:ring-2`}
    />
    {hint && !error && <p className="text-xs text-gray-500">{hint}</p>}
    {error && <p className="text-xs text-red-600">{error}</p>}
  </div>
);
