
import { CrudDataTable } from '../components/CrudDataTable';
import type { ColumnDef, FormFieldDef } from '../components/CrudDataTable';

export function EmployeesPage() {
  const columns: ColumnDef<any>[] = [
    { key: 'firstName', header: 'First Name' },
    { key: 'lastName', header: 'Last Name' },
    { key: 'email', header: 'Email' },
    { key: 'phoneNumber', header: 'Phone Number' },
    { key: 'username', header: 'Username' }
  ];

  const formFields: FormFieldDef[] = [
    { name: 'firstName', label: 'First Name', type: 'text', required: true },
    { name: 'lastName', label: 'Last Name', type: 'text', required: true },
    { name: 'email', label: 'Email', type: 'email' },
    { name: 'phoneNumber', label: 'Phone Number', type: 'text' },
    { name: 'username', label: 'Username', type: 'text', required: true },
    { name: 'passwordHash', label: 'Password', type: 'text', required: true }
  ];

  return (
    <CrudDataTable 
      title="Employees"
      endpoint="/employees"
      columns={columns}
      formFields={formFields}
    />
  );
}

export default EmployeesPage;
