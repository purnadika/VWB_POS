import { useState, useEffect } from 'react';
import { CrudDataTable } from '../components/CrudDataTable';
import type { ColumnDef, FormFieldDef } from '../components/CrudDataTable';
import { fetchApi } from '../utils/api';

export function TaxesPage() {
  const [categories, setCategories] = useState<{ id: number; taxCategoryName: string }[]>([]);

  useEffect(() => {
    fetchApi<{ data: { id: number; taxCategoryName: string }[] } | { id: number; taxCategoryName: string }[]>('/tax-categories')
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
    { key: 'name', header: 'Tax Name' },
    { key: 'rate', header: 'Rate (%)' },
    { 
      key: 'taxCategoryId', 
      header: 'Tax Category',
      render: (row) => {
        const cat = categories.find(c => c.id === row.taxCategoryId);
        return cat ? cat.taxCategoryName : row.taxCategoryId;
      }
    }
  ];

  const formFields: FormFieldDef[] = [
    { name: 'name', label: 'Tax Name', type: 'text', required: true },
    { name: 'rate', label: 'Rate (%)', type: 'number', required: true },
    { 
      name: 'taxCategoryId', 
      label: 'Tax Category', 
      type: 'select', 
      required: true,
      options: categories.map(c => ({ label: c.taxCategoryName, value: c.id }))
    }
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
