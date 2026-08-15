
import { CrudDataTable } from '../components/CrudDataTable';
import type { ColumnDef, FormFieldDef } from '../components/CrudDataTable';

export function MessagesPage() {
  const columns: ColumnDef<any>[] = [
    { key: 'senderId', header: 'Sender ID' },
    { key: 'receiverId', header: 'Receiver ID' },
    { key: 'subject', header: 'Subject' },
    { key: 'body', header: 'Body' },
    { key: 'sentAt', header: 'Sent At' }
  ];

  const formFields: FormFieldDef[] = [
    { name: 'senderId', label: 'Sender ID', type: 'number', required: true },
    { name: 'receiverId', label: 'Receiver ID', type: 'number', required: true },
    { name: 'subject', label: 'Subject', type: 'text', required: true },
    { name: 'body', label: 'Body', type: 'text', required: true },
    { name: 'sentAt', label: 'Sent At', type: 'date', required: true }
  ];

  return (
    <CrudDataTable 
      title="Messages"
      endpoint="/messages"
      columns={columns}
      formFields={formFields}
    />
  );
}
