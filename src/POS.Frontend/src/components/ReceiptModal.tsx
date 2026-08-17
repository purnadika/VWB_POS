import React from 'react';
import { useTranslation } from 'react-i18next';
import { Printer, X } from 'lucide-react';
import { useLocale } from '../../contexts/LocaleContext';
import './ReceiptModal.css';

interface ReceiptModalProps {
  saleId: number;
  cart: any[];
  total: number;
  tendered: number;
  change: number;
  customerName: string;
  onClose: () => void;
}

export function ReceiptModal({ saleId, cart, total, tendered, change, customerName, onClose }: ReceiptModalProps) {
  const { t } = useTranslation();
  const { formatCurrency } = useLocale();

  const handlePrint = () => {
    window.print();
  };

  return (
    <div className="receipt-overlay">
      <div className="receipt-content">
        <div className="receipt-header-actions no-print">
          <button className="btn btn-secondary" onClick={handlePrint}>
            <Printer size={18} style={{ marginRight: '8px' }} />
            {t('Print Receipt')}
          </button>
          <button className="btn-icon" onClick={onClose}>
            <X size={20} />
          </button>
        </div>

        <div className="receipt-paper" id="printable-receipt">
          <div className="receipt-header">
            <h2>POS Store</h2>
            <p>123 Main Street, City</p>
            <p>Tel: (555) 123-4567</p>
            <p>----------------------------------------</p>
            <p>{t('Receipt')} #{saleId}</p>
            <p>{t('Date')}: {new Date().toLocaleString()}</p>
            <p>{t('Customer')}: {customerName}</p>
            <p>----------------------------------------</p>
          </div>

          <div className="receipt-items">
            {cart.map((item, idx) => (
              <div key={idx} className="receipt-item">
                <div className="receipt-item-name">{item.description}</div>
                <div className="receipt-item-row">
                  <span>{item.quantity} x {formatCurrency(item.itemUnitPrice)}</span>
                  <span>{formatCurrency(item.quantity * item.itemUnitPrice)}</span>
                </div>
              </div>
            ))}
          </div>

          <div className="receipt-footer">
            <p>----------------------------------------</p>
            <div className="receipt-summary-row">
              <strong>{t('Total')}</strong>
              <strong>{formatCurrency(total)}</strong>
            </div>
            <div className="receipt-summary-row">
              <span>{t('Tendered')}</span>
              <span>{formatCurrency(tendered)}</span>
            </div>
            <div className="receipt-summary-row">
              <span>{t('Change')}</span>
              <span>{formatCurrency(change)}</span>
            </div>
            <p>----------------------------------------</p>
            <p className="receipt-thanks">{t('Thank you for your business!')}</p>
          </div>
        </div>
      </div>
    </div>
  );
}
