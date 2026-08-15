const fs = require('fs'); 
const modules = ['GiftCards', 'Messages', 'Expenses', 'Receivings', 'Taxes', 'Reports', 'Settings']; 

modules.forEach(m => { 
  const c = `import React from 'react';
import { CrudDataTable } from '../components/CrudDataTable';
import type { ColumnDef, FormFieldDef } from '../components/CrudDataTable';

export function ${m}Page() {
  const columns: ColumnDef<any>[] = [
    { key: 'name', header: 'Name' },
  ];

  const formFields: FormFieldDef[] = [
    { name: 'name', label: 'Name', type: 'text', required: true },
  ];

  return (
    <CrudDataTable 
      title="${m}"
      endpoint="/${m.toLowerCase()}"
      columns={columns}
      formFields={formFields}
    />
  );
}
`; 
  fs.writeFileSync('d:/Projects/NETPOS/src/POS.Frontend/src/pages/'+m+'Page.tsx', c); 
});
