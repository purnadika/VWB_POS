import { useState, useEffect } from 'react';
import { CrudDataTable } from '../components/CrudDataTable';
import type { ColumnDef, FormFieldDef } from '../components/CrudDataTable';
import { fetchApi } from '../utils/api';

export function ExpensesPage() {
  const [employees, setEmployees] = useState<{ id: number; firstName: string; lastName: string }[]>([]);
  const [categories, setCategories] = useState<{ id: number; categoryName: string }[]>([]);

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
      
    fetchApi<{ data: { id: number; categoryName: string }[] } | { id: number; categoryName: string }[]>('/expense-categories')
      .then(res => {
        if (res && typeof res === 'object' && 'data' in res && Array.isArray(res.data)) {
          setCategories(res.data);
        } else if (Array.isArray(res)) {
          setCategories(res);
        }
      })
      .catch(console.error);
  }, []);
  const columns: ColumnDef<any>[] = [
    { 
      key: 'categoryId', 
      header: 'Category Name',
      render: (row) => {
        const cat = categories.find(c => c.id === row.categoryId);
        return cat ? cat.categoryName : row.categoryId;
      }
    },
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
    { 
      name: 'categoryId', 
      label: 'Category Name', 
      type: 'select', 
      required: true,
      options: categories.map(c => ({ label: c.categoryName, value: c.id }))
    },
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
