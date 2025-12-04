import React, { PropsWithChildren, useEffect, useState } from 'react';
import { Link, NavLink, useLocation } from 'react-router-dom';

const Layout: React.FC<PropsWithChildren> = ({ children }) => {
  const location = useLocation();
  const [token, setToken] = useState<string>('');

  useEffect(() => {
    setToken(localStorage.getItem('token') || '');
  }, [location.pathname]);

  const saveToken = () => {
    localStorage.setItem('token', token);
    // simple visual feedback
    alert('Token saved in localStorage.');
  };

  return (
    <div className="min-h-screen bg-gray-50">
      <header className="bg-white border-b border-gray-200 sticky top-0 z-30">
        <div className="max-w-7xl mx-auto px-4 sm:px-6 lg:px-8 py-4 flex items-center justify-between">
          <Link to="/invoices" className="flex items-center gap-2">
            <div className="h-9 w-9 rounded-lg bg-indigo-600 text-white grid place-items-center font-bold">IB</div>
            <div>
              <div className="text-lg font-semibold text-gray-900">Invoice Builder</div>
              {/*<div className="text-xs text-gray-500">by Anton Martyniuk</div>*/}
            </div>
          </Link>
          <nav className="hidden sm:flex items-center gap-6">
            <NavLink to="/invoices" className={({ isActive }) => `text-sm ${isActive ? 'text-indigo-600 font-medium' : 'text-gray-600 hover:text-gray-900'}`}>Invoices</NavLink>
            <NavLink to="/customers" className={({ isActive }) => `text-sm ${isActive ? 'text-indigo-600 font-medium' : 'text-gray-600 hover:text-gray-900'}`}>Customers</NavLink>
            <NavLink to="/senders" className={({ isActive }) => `text-sm ${isActive ? 'text-indigo-600 font-medium' : 'text-gray-600 hover:text-gray-900'}`}>Senders</NavLink>
            <NavLink to="/invoices/new" className={({ isActive }) => `text-sm ${isActive ? 'text-indigo-600 font-medium' : 'text-gray-600 hover:text-gray-900'}`}>New Invoice</NavLink>
          </nav>
          {/*<div className="flex items-center gap-2">*/}
          {/*  <input*/}
          {/*    value={token}*/}
          {/*    onChange={(e) => setToken(e.target.value)}*/}
          {/*    placeholder="Bearer token"*/}
          {/*    className="hidden md:block w-64 rounded-md border border-gray-300 px-3 py-2 text-sm focus:outline-none focus:ring-2 focus:ring-indigo-500"*/}
          {/*  />*/}
          {/*  <button onClick={saveToken} className="hidden md:inline-flex px-3 py-2 text-sm rounded-md bg-indigo-600 text-white hover:bg-indigo-700">Save Token</button>*/}
          {/*</div>*/}
        </div>
      </header>
      <main className="max-w-screen-2xl mx-auto px-4 sm:px-6 lg:px-8 py-6">{children}</main>
      </div>
    );
  };

export default Layout;
