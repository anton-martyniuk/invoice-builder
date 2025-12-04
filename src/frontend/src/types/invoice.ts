export type LineItem = {
  id?: string;
  itemName: string;
  quantity: number;
  unitPrice: number;
  total: number;
};

// List item shape from FRONTEND.MD: id, invoiceNumber, invoiceDate, dueDate, totalAmount
export type InvoiceListItem = {
  id: string;
  invoiceNumber: string;
  invoiceDate: string; // ISO
  dueDate: string; // ISO
  currency: string;
  totalAmount: number;
};

// Detail shape includes nested customer and sender
export type CustomerRef = {
  id: string;
  companyName: string;
  customerName: string;
  customerAddress: string;
  postalCode: string;
  customerEmail: string;
  customerTaxVatId: string;
};

export type SenderRef = {
  id: string;
  senderCompanyName: string;
  senderFullName: string;
  senderAddress: string;
  senderTaxVatId: string;
  bankDetails: string;
};

export type InvoiceDetail = {
  id: string;
  invoiceNumber: string;
  invoiceDate: string;
  dueDate: string;
  currency: string;
  notes?: string;
  customer: CustomerRef;
  sender: SenderRef;
  lineItems: LineItem[];
  subtotal: number;
  taxRate: number;
  totalAmount: number;
};

// Create/Update payloads expect ids for customer and sender
export type InvoiceUpsert = {
  invoiceNumber: string;
  invoiceDate: string;
  dueDate: string;
  currency: string;
  notes?: string;
  customerId: string;
  senderId: string;
  subtotal: number;
  taxRate: number;
  totalAmount: number;
  lineItems: LineItem[];
};

export type PagedInvoices = {
  items: InvoiceListItem[];
  total: number;
  offset: number;
  limit: number;
};
