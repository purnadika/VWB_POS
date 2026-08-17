import React from 'react';
import { useTranslation } from 'react-i18next';
import { CrudDataTable } from '../components/CrudDataTable';
import type { ColumnDef, FormFieldDef } from '../components/CrudDataTable';

export function TaxCategoriesPage() {
  const { t } = useTranslation();

  const columns: ColumnDef<any>[] = [
    { key: 'id', header: 'ID' },
    { key: 'categoryName', header: t('Category Name') },
  ];

  const formFields: FormFieldDef[] = [
    { name: 'categoryName', label: t('Category Name'), type: 'text', required: true },
  ];

  return (
    <div className="page-container">
      <CrudDataTable 
        title={t('Tax Categories')}
        endpoint="/tax-categories"
        columns={columns}
        formFields={formFields}
      />
    </div>
  );
}
