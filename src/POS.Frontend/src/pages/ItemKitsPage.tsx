
import { useTranslation } from 'react-i18next';
import { CrudDataTable } from '../components/CrudDataTable';
import type { ColumnDef, FormFieldDef } from '../components/CrudDataTable';

export function ItemKitsPage() {
  const { t } = useTranslation();

  const columns: ColumnDef<any>[] = [
    { key: 'name', header: t('Kit Name') },
    { key: 'description', header: t('Description') },
    { key: 'itemKitNumber', header: t('Kit Number') },
    { key: 'costPrice', header: t('Cost Price') },
    { key: 'unitPrice', header: t('Unit Price') },
  ];

  const formFields: FormFieldDef[] = [
    { name: 'name', label: t('Kit Name'), type: 'text', required: true },
    { name: 'description', label: t('Description'), type: 'text' },
    { name: 'itemKitNumber', label: t('Kit Number'), type: 'text' },
    { name: 'costPrice', label: t('Cost Price'), type: 'number' },
    { name: 'unitPrice', label: t('Unit Price'), type: 'number' },
  ];

  return (
    <div className="page-container">
      <h2>{t('Item Kits')}</h2>
      <p style={{ color: 'var(--color-text-light)', marginBottom: '20px' }}>
        {t('Item kits are bundles of items sold together.')}
      </p>
      <CrudDataTable 
        title={t('Item Kits')}
        endpoint="/itemkits"
      columns={columns}
      formFields={formFields}
    />
    </div>
  );
}
