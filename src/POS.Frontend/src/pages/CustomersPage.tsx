
import { CrudDataTable } from '../components/CrudDataTable';
import type { ColumnDef, FormFieldDef } from '../components/CrudDataTable';

export function CustomersPage() {
  const columns: ColumnDef<any>[] = [
    { key: 'firstName', header: 'First Name' },
    { key: 'lastName', header: 'Last Name' },
    { key: 'email', header: 'Email' },
    { key: 'phoneNumber', header: 'Phone' },
    { key: 'companyName', header: 'Company' },
  ];

  const formFields: FormFieldDef[] = [
    { name: 'firstName', label: 'First Name', type: 'text', required: true },
    { name: 'lastName', label: 'Last Name', type: 'text', required: true },
    { name: 'email', label: 'Email', type: 'email' },
    { name: 'phoneNumber', label: 'Phone Number', type: 'text' },
    { name: 'companyName', label: 'Company Name', type: 'text' },
    { name: 'address1', label: 'Address', type: 'text' },
    { name: 'city', label: 'City', type: 'text' },
    { name: 'state', label: 'State', type: 'text' },
    { name: 'zipCode', label: 'Zip Code', type: 'text' },
  ];

  return (
    <CrudDataTable 
      title="Customers"
      endpoint="/customers"
      columns={columns}
      formFields={formFields}
    />
  );
}
