import { useState, useEffect } from 'react';
import { CrudDataTable } from '../components/CrudDataTable';
import type { ColumnDef, FormFieldDef } from '../components/CrudDataTable';
import { fetchApi } from '../utils/api';

export function MessagesPage() {
  const [employees, setEmployees] = useState<{ id: number; firstName: string; lastName: string }[]>([]);

  useEffect(() => {
    fetchApi<{ data: { id: number; firstName: string; lastName: string }[] } | { id: number; firstName: string; lastName: string }[]>('/employees')
      .then(res => {
        if (res && typeof res === 'object' && 'data' in res && Array.isArray(res.data)) {
          setEmployees(res.data);
        } else if (Array.isArray(res)) {
          setEmployees(res);
        }
      })
      .catch(console.error);
  }, []);
  const columns: ColumnDef<any>[] = [
    { 
      key: 'senderId', 
      header: 'Sender',
      render: (row) => {
        const emp = employees.find(e => e.id === row.senderId);
        return emp ? `${emp.firstName} ${emp.lastName}` : row.senderId;
      }
    },
    { 
      key: 'receiverId', 
      header: 'Receiver',
      render: (row) => {
        const emp = employees.find(e => e.id === row.receiverId);
        return emp ? `${emp.firstName} ${emp.lastName}` : row.receiverId;
      }
    },
    { key: 'subject', header: 'Subject' },
    { key: 'body', header: 'Body' },
    { key: 'sentAt', header: 'Sent At' }
  ];

  const formFields: FormFieldDef[] = [
    { 
      name: 'senderId', 
      label: 'Sender', 
      type: 'select', 
      required: true,
      options: employees.map(e => ({ label: `${e.firstName} ${e.lastName}`, value: e.id }))
    },
    { 
      name: 'receiverId', 
      label: 'Receiver', 
      type: 'select', 
      required: true,
      options: employees.map(e => ({ label: `${e.firstName} ${e.lastName}`, value: e.id }))
    },
    { name: 'subject', label: 'Subject', type: 'text', required: true },
    { name: 'body', label: 'Body', type: 'text', required: true },
    { name: 'sentAt', label: 'Sent At', type: 'date', required: true }
  ];

  return (
    <CrudDataTable 
      title="Messages"
      endpoint="/messages"
      columns={columns}
      formFields={formFields}
    />
  );
}
