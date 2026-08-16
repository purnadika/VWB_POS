import { useState, useEffect } from 'react';
import { CrudDataTable } from '../components/CrudDataTable';
import type { ColumnDef, FormFieldDef } from '../components/CrudDataTable';
import { fetchApi } from '../utils/api';

export function ExpensesPage() {
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
    { key: 'categoryId', header: 'Category ID' },
    { key: 'amount', header: 'Amount ($)' },
    { key: 'paymentType', header: 'Payment Type' },
    { key: 'description', header: 'Description' },
    { 
      key: 'employeeId', 
      header: 'Employee', 
      render: (row) => {
        const emp = employees.find(e => e.id === row.employeeId);
        return emp ? `${emp.firstName} ${emp.lastName}` : row.employeeId;
      }
    },
    { key: 'date', header: 'Date' }
  ];

  const formFields: FormFieldDef[] = [
    { name: 'categoryId', label: 'Category ID', type: 'number', required: true },
    { name: 'amount', label: 'Amount ($)', type: 'number', required: true },
    { name: 'paymentType', label: 'Payment Type', type: 'text', required: true },
    { name: 'description', label: 'Description', type: 'text' },
    { 
      name: 'employeeId', 
      label: 'Employee', 
      type: 'select', 
      required: true,
      options: employees.map(e => ({ label: `${e.firstName} ${e.lastName}`, value: e.id }))
    },
    { name: 'date', label: 'Date', type: 'date', required: true }
  ];

  return (
    <CrudDataTable 
      title="Expenses"
      endpoint="/expenses"
      columns={columns}
      formFields={formFields}
    />
  );
}
