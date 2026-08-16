import { useState, useEffect } from 'react';
import { CrudDataTable } from '../components/CrudDataTable';
import type { ColumnDef, FormFieldDef } from '../components/CrudDataTable';
import { fetchApi } from '../utils/api';
import './AdminPages.css';

export function ItemsPage() {
  const [categories, setCategories] = useState<{ id: number; name: string }[]>([]);

  useEffect(() => {
    fetchApi<{ data: { id: number; name: string }[] } | { id: number; name: string }[]>('/item-categories')
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
    { key: 'name', header: 'Item Name' },
    { key: 'categoryId', header: 'Category', render: (row) => categories.find(c => c.id === row.categoryId)?.name || row.categoryId },
    { key: 'costPrice', header: 'Cost Price' },
    { key: 'unitPrice', header: 'Unit Price' },
  ];

  const formFields: FormFieldDef[] = [
    { name: 'name', label: 'Item Name', type: 'text', required: true },
    { 
      name: 'categoryId', 
      label: 'Category', 
      type: 'select', 
      required: true,
      options: categories.map(c => ({ label: c.name, value: c.id }))
    },
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
