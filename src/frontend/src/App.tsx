import React from 'react';
import { BrowserRouter, Routes, Route, Navigate } from 'react-router-dom';
import InvoicesList from './pages/invoices/InvoicesList';
import InvoiceDetails from './pages/invoices/InvoiceDetails';
import InvoiceForm from './pages/invoices/InvoiceForm';
import CustomersList from './pages/customers/CustomersList';
import CustomerDetails from './pages/customers/CustomerDetails';
import CustomerForm from './pages/customers/CustomerForm';
import SendersList from './pages/senders/SendersList';
import SenderDetails from './pages/senders/SenderDetails';
import SenderForm from './pages/senders/SenderForm';
import Layout from './components/Layout';

const App: React.FC = () => {
  return (
    <BrowserRouter>
      <Layout>
        <Routes>
          <Route path="/" element={<Navigate to="/invoices" replace />} />
          {/* Invoices */}
          <Route path="/invoices" element={<InvoicesList />} />
          <Route path="/invoices/new" element={<InvoiceForm mode="create" />} />
          <Route path="/invoices/:id" element={<InvoiceDetails />} />
          <Route path="/invoices/:id/edit" element={<InvoiceForm mode="edit" />} />
          {/* Customers */}
          <Route path="/customers" element={<CustomersList />} />
          <Route path="/customers/new" element={<CustomerForm mode="create" />} />
          <Route path="/customers/:id" element={<CustomerDetails />} />
          <Route path="/customers/:id/edit" element={<CustomerForm mode="edit" />} />
          {/* Senders */}
          <Route path="/senders" element={<SendersList />} />
          <Route path="/senders/new" element={<SenderForm mode="create" />} />
          <Route path="/senders/:id" element={<SenderDetails />} />
          <Route path="/senders/:id/edit" element={<SenderForm mode="edit" />} />
          <Route path="*" element={<Navigate to="/invoices" replace />} />
        </Routes>
      </Layout>
    </BrowserRouter>
  );
};

export default App;
