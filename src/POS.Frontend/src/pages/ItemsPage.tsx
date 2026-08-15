
import { CrudDataTable } from '../components/CrudDataTable';
import type { ColumnDef, FormFieldDef } from '../components/CrudDataTable';
import './AdminPages.css';

export function ItemsPage() {
  const columns: ColumnDef<any>[] = [
    { key: 'name', header: 'Item Name' },
    { key: 'categoryId', header: 'Category ID' },
    { key: 'costPrice', header: 'Cost Price' },
    { key: 'unitPrice', header: 'Unit Price' },
  ];

  const formFields: FormFieldDef[] = [
    { name: 'name', label: 'Item Name', type: 'text', required: true },
    { name: 'categoryId', label: 'Category ID', type: 'number', required: true },
    { name: 'costPrice', label: 'Cost Price', type: 'number', required: true },
    { name: 'unitPrice', label: 'Unit Price', type: 'number', required: true },
    { name: 'description', label: 'Description', type: 'text' },
  ];

  return (
    <div className="admin-page">
      <CrudDataTable 
        title="Items"
        endpoint="/items"
        columns={columns}
        formFields={formFields}
      />
    </div>
  );
}

export default ItemsPage;
