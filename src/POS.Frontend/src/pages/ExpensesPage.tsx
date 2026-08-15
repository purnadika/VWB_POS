
import { CrudDataTable } from '../components/CrudDataTable';
import type { ColumnDef, FormFieldDef } from '../components/CrudDataTable';

export function ExpensesPage() {
  const columns: ColumnDef<any>[] = [
    { key: 'categoryId', header: 'Category ID' },
    { key: 'amount', header: 'Amount ($)' },
    { key: 'paymentType', header: 'Payment Type' },
    { key: 'description', header: 'Description' },
    { key: 'employeeId', header: 'Employee ID' },
    { key: 'date', header: 'Date' }
  ];

  const formFields: FormFieldDef[] = [
    { name: 'categoryId', label: 'Category ID', type: 'number', required: true },
    { name: 'amount', label: 'Amount ($)', type: 'number', required: true },
    { name: 'paymentType', label: 'Payment Type', type: 'text', required: true },
    { name: 'description', label: 'Description', type: 'text' },
    { name: 'employeeId', label: 'Employee ID', type: 'number', required: true },
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
