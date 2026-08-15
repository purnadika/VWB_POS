
import { CrudDataTable } from '../components/CrudDataTable';
import type { ColumnDef, FormFieldDef } from '../components/CrudDataTable';

export function SuppliersPage() {
  const columns: ColumnDef<any>[] = [
    { key: 'companyName', header: 'Company' },
    { key: 'firstName', header: 'Contact First Name' },
    { key: 'lastName', header: 'Contact Last Name' },
    { key: 'email', header: 'Email' },
    { key: 'phoneNumber', header: 'Phone' },
  ];

  const formFields: FormFieldDef[] = [
    { name: 'companyName', label: 'Company Name', type: 'text', required: true },
    { name: 'firstName', label: 'Contact First Name', type: 'text', required: true },
    { name: 'lastName', label: 'Contact Last Name', type: 'text', required: true },
    { name: 'email', label: 'Email', type: 'email' },
    { name: 'phoneNumber', label: 'Phone Number', type: 'text' },
    { name: 'agencyName', label: 'Agency', type: 'text' },
    { name: 'accountNumber', label: 'Account Number', type: 'text' },
  ];

  return (
    <CrudDataTable 
      title="Suppliers"
      endpoint="/suppliers"
      columns={columns}
      formFields={formFields}
    />
  );
}
