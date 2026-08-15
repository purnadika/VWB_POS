
import { CrudDataTable } from '../components/CrudDataTable';
import type { ColumnDef, FormFieldDef } from '../components/CrudDataTable';

export function GiftCardsPage() {
  const columns: ColumnDef<any>[] = [
    { key: 'giftcardNumber', header: 'Card Number' },
    { key: 'value', header: 'Value ($)' },
    { key: 'customerId', header: 'Customer ID' }
  ];

  const formFields: FormFieldDef[] = [
    { name: 'giftcardNumber', label: 'Card Number', type: 'text', required: true },
    { name: 'value', label: 'Value ($)', type: 'number', required: true },
    { name: 'customerId', label: 'Customer ID', type: 'number' }
  ];

  return (
    <CrudDataTable 
      title="GiftCards"
      endpoint="/giftcards"
      columns={columns}
      formFields={formFields}
    />
  );
}
