import { CrudDataTable } from '../components/CrudDataTable';
import type { ColumnDef, FormFieldDef } from '../components/CrudDataTable';
import { useTranslation } from 'react-i18next';

export function SalesPage() {
  const { t } = useTranslation();
  
  const columns: ColumnDef<any>[] = [
    { key: 'id', header: 'Sale ID' },
    { key: 'saleTime', header: 'Time' },
    { key: 'customerName', header: 'Customer' },
    { key: 'employeeName', header: 'Employee' },
    { key: 'comment', header: 'Comment' }
  ];

  // Sales are typically read-only for now, so no form fields are strictly needed for editing,
  // but CrudDataTable expects it. We will provide empty formFields to prevent errors if New is clicked.
  // In a real read-only setup, we'd disable the New/Edit buttons.
  const formFields: FormFieldDef[] = [];

  return (
    <CrudDataTable 
      title={t('Sales History')}
      endpoint="/sales"
      columns={columns}
      formFields={formFields}
    />
  );
}
