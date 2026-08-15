
import { CrudDataTable } from '../components/CrudDataTable';
import type { ColumnDef, FormFieldDef } from '../components/CrudDataTable';

export function ReceivingsPage() {
  const columns: ColumnDef<any>[] = [
    { key: 'supplierId', header: 'Supplier ID' },
    { key: 'employeeId', header: 'Employee ID' },
    { key: 'receivingTime', header: 'Receiving Time' },
    { key: 'comment', header: 'Comment' },
    { key: 'reference', header: 'Reference' },
    { key: 'paymentType', header: 'Payment Type' },
    { key: 'total', header: 'Total ($)' }
  ];

  const formFields: FormFieldDef[] = [
    { name: 'supplierId', label: 'Supplier ID', type: 'number' },
    { name: 'employeeId', label: 'Employee ID', type: 'number', required: true },
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
