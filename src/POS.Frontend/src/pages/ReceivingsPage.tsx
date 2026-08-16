import { useState, useEffect } from 'react';
import { CrudDataTable } from '../components/CrudDataTable';
import type { ColumnDef, FormFieldDef } from '../components/CrudDataTable';
import { fetchApi } from '../utils/api';

export function ReceivingsPage() {
  const [employees, setEmployees] = useState<{ id: number; firstName: string; lastName: string }[]>([]);
  const [suppliers, setSuppliers] = useState<{ id: number; companyName: string }[]>([]);

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

    fetchApi<{ data: { id: number; companyName: string }[] } | { id: number; companyName: string }[]>('/suppliers')
      .then(res => {
        if (res && typeof res === 'object' && 'data' in res && Array.isArray(res.data)) {
          setSuppliers(res.data);
        } else if (Array.isArray(res)) {
          setSuppliers(res);
        }
      })
      .catch(console.error);
  }, []);
  const columns: ColumnDef<any>[] = [
    { 
      key: 'supplierId', 
      header: 'Supplier',
      render: (row) => suppliers.find(s => s.id === row.supplierId)?.companyName || row.supplierId
    },
    { 
      key: 'employeeId', 
      header: 'Employee',
      render: (row) => {
        const emp = employees.find(e => e.id === row.employeeId);
        return emp ? `${emp.firstName} ${emp.lastName}` : row.employeeId;
      }
    },
    { key: 'receivingTime', header: 'Receiving Time' },
    { key: 'comment', header: 'Comment' },
    { key: 'reference', header: 'Reference' },
    { key: 'paymentType', header: 'Payment Type' },
    { key: 'total', header: 'Total ($)' }
  ];

  const formFields: FormFieldDef[] = [
    { 
      name: 'supplierId', 
      label: 'Supplier', 
      type: 'select',
      options: suppliers.map(s => ({ label: s.companyName, value: s.id }))
    },
    { 
      name: 'employeeId', 
      label: 'Employee', 
      type: 'select', 
      required: true,
      options: employees.map(e => ({ label: `${e.firstName} ${e.lastName}`, value: e.id }))
    },
    { name: 'receivingTime', label: 'Receiving Time', type: 'date', required: true },
    { name: 'comment', label: 'Comment', type: 'text' },
    { name: 'reference', label: 'Reference', type: 'text' },
    { name: 'paymentType', label: 'Payment Type', type: 'text', required: true },
    { name: 'total', label: 'Total ($)', type: 'number', required: true }
  ];

  return (
    <CrudDataTable 
      title="Receivings"
      endpoint="/receivings"
      columns={columns}
      formFields={formFields}
    />
  );
}
