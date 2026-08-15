
import { CrudDataTable } from '../components/CrudDataTable';
import type { ColumnDef, FormFieldDef } from '../components/CrudDataTable';

export function SettingsPage() {
  const columns: ColumnDef<any>[] = [
    { key: 'key', header: 'Key' },
    { key: 'value', header: 'Value' }
  ];

  const formFields: FormFieldDef[] = [
    { name: 'key', label: 'Key', type: 'text', required: true },
    { name: 'value', label: 'Value', type: 'text', required: true }
  ];

  return (
    <CrudDataTable 
      title="Settings"
      endpoint="/configuration"
      columns={columns}
      formFields={formFields}
      primaryKey="key"
    />
  );
}
