
import { CrudDataTable } from '../components/CrudDataTable';
import type { ColumnDef, FormFieldDef } from '../components/CrudDataTable';
import './AdminPages.css';

export function ItemCategoriesPage() {
  const columns: ColumnDef<any>[] = [
    { key: 'name', header: 'Category Name' },
    { key: 'description', header: 'Description' },
  ];

  const formFields: FormFieldDef[] = [
    { name: 'name', label: 'Category Name', type: 'text', required: true },
    { name: 'description', label: 'Description', type: 'text' },
  ];

  return (
    <div className="admin-page">
      <CrudDataTable
        title="Item Categories"
        endpoint="/item-categories"
        columns={columns}
        formFields={formFields}
      />
    </div>
  );
}

export default ItemCategoriesPage;
