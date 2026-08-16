import { useState, useEffect } from 'react';
import { CrudDataTable } from '../components/CrudDataTable';
import type { ColumnDef, FormFieldDef } from '../components/CrudDataTable';
import { fetchApi } from '../utils/api';

export function GiftCardsPage() {
  const [customers, setCustomers] = useState<{ id: number; firstName: string; lastName: string }[]>([]);

  useEffect(() => {
    fetchApi<{ data: { id: number; firstName: string; lastName: string }[] } | { id: number; firstName: string; lastName: string }[]>('/customers')
      .then(res => {
        if (res && typeof res === 'object' && 'data' in res && Array.isArray(res.data)) {
          setCustomers(res.data);
        } else if (Array.isArray(res)) {
          setCustomers(res);
        }
      })
      .catch(console.error);
  }, []);
  const columns: ColumnDef<any>[] = [
    { key: 'giftcardNumber', header: 'Card Number' },
    { key: 'value', header: 'Value ($)' },
    { 
      key: 'customerId', 
      header: 'Customer',
      render: (row) => {
        const cust = customers.find(c => c.id === row.customerId);
        return cust ? `${cust.firstName} ${cust.lastName}` : row.customerId;
      }
    }
  ];

  const formFields: FormFieldDef[] = [
    { name: 'giftcardNumber', label: 'Card Number', type: 'text', required: true },
    { name: 'value', label: 'Value ($)', type: 'number', required: true },
    { 
      name: 'customerId', 
      label: 'Customer', 
      type: 'select',
      options: customers.map(c => ({ label: `${c.firstName} ${c.lastName}`, value: c.id }))
    }
  ];

  return (
    <CrudDataTable 
      title="GiftCards"
      endpoint="/giftcards"
      columns={columns}
      formFields={formFields}
    />
  );
}
