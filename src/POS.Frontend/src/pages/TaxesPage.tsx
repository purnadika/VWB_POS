
import { CrudDataTable } from '../components/CrudDataTable';
import type { ColumnDef, FormFieldDef } from '../components/CrudDataTable';

export function TaxesPage() {
  const columns: ColumnDef<any>[] = [
    { key: 'name', header: 'Tax Name' },
    { key: 'rate', header: 'Rate (%)' },
    { key: 'taxCategoryId', header: 'Tax Category ID' }
  ];

  const formFields: FormFieldDef[] = [
    { name: 'name', label: 'Tax Name', type: 'text', required: true },
    { name: 'rate', label: 'Rate (%)', type: 'number', required: true },
    { name: 'taxCategoryId', label: 'Tax Category ID', type: 'number', required: true }
  ];

  return (
    <CrudDataTable 
      title="Taxes"
      endpoint="/taxes"
      columns={columns}
      formFields={formFields}
    />
  );
}
