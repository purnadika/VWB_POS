
import { CrudDataTable } from '../components/CrudDataTable';
import type { ColumnDef, FormFieldDef } from '../components/CrudDataTable';

export function ItemKitsPage() {
  const columns: ColumnDef<any>[] = [
    { key: 'name', header: 'Kit Name' },
    { key: 'description', header: 'Description' },
    { key: 'itemKitNumber', header: 'Kit Number' },
    { key: 'costPrice', header: 'Cost Price' },
    { key: 'unitPrice', header: 'Unit Price' },
  ];

  const formFields: FormFieldDef[] = [
    { name: 'name', label: 'Kit Name', type: 'text', required: true },
    { name: 'description', label: 'Description', type: 'text' },
    { name: 'itemKitNumber', label: 'Kit Number', type: 'text' },
    { name: 'costPrice', label: 'Cost Price', type: 'number' },
    { name: 'unitPrice', label: 'Unit Price', type: 'number' },
  ];

  return (
    <CrudDataTable 
      title="Item Kits"
      endpoint="/itemkits"
      columns={columns}
      formFields={formFields}
    />
  );
}
