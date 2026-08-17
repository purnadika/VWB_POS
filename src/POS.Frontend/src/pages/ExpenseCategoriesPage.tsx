import React from 'react';
import { useTranslation } from 'react-i18next';
import { CrudDataTable } from '../components/CrudDataTable';
import type { ColumnDef, FormFieldDef } from '../components/CrudDataTable';

export function ExpenseCategoriesPage() {
  const { t } = useTranslation();

  const columns: ColumnDef<any>[] = [
    { key: 'id', header: 'ID' },
    { key: 'categoryName', header: t('Category Name') },
    { key: 'description', header: t('Description') },
  ];

  const formFields: FormFieldDef[] = [
    { name: 'categoryName', label: t('Category Name'), type: 'text', required: true },
    { name: 'description', label: t('Description'), type: 'text' },
  ];

  return (
    <div className="page-container">
      <CrudDataTable 
        title={t('Expense Categories')}
        endpoint="/expense-categories"
        columns={columns}
        formFields={formFields}
      />
    </div>
  );
}
