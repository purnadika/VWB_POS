import React, { useState, useEffect } from 'react';
import { useTranslation } from 'react-i18next';
import { Search, Plus, Minus, Trash2, CreditCard, X } from 'lucide-react';
import type { Item, SaleItem } from '../types';
import { fetchApi } from '../utils/api';
import { useLocale } from '../contexts/LocaleContext';
import { ReceiptModal } from '../components/ReceiptModal';
import './PosPage.css';

export function PosPage() {
  const [items, setItems] = useState<Item[]>([]);
  const [search, setSearch] = useState('');
  const [cart, setCart] = useState<SaleItem[]>([]);
  const [checkoutMsg, setCheckoutMsg] = useState<{ type: 'success' | 'error'; text: string } | null>(null);
  const [customers, setCustomers] = useState<{ id: number; firstName: string; lastName: string }[]>([]);
  const [selectedCustomer, setSelectedCustomer] = useState<number | ''>('');
  
  const [showPaymentModal, setShowPaymentModal] = useState(false);
  const [tenderedAmount, setTenderedAmount] = useState<number | ''>('');
  const [processing, setProcessing] = useState(false);
  
  const [receiptData, setReceiptData] = useState<any>(null);
  
  const { formatCurrency } = useLocale();
  const { t } = useTranslation();

  useEffect(() => {
    fetchApi<{ data: Item[] }>('/items')
      .then((res) => {
        if (Array.isArray(res.data)) {
          setItems(res.data);
        } else if (Array.isArray(res)) {
          setItems(res as unknown as Item[]);
        }
      })
      .catch((err) => console.error('Error fetching items', err));
      
    fetchApi<{ data: { id: number; firstName: string; lastName: string }[] } | { id: number; firstName: string; lastName: string }[]>('/customers')
      .then(res => {
        if (res && typeof res === 'object' && 'data' in res && Array.isArray(res.data)) {
          setCustomers(res.data);
        } else if (Array.isArray(res)) {
          setCustomers(res);
        }
      })
      .catch(console.error);
  }, []);

  const filteredItems = items.filter(
    (item) => item.name.toLowerCase().includes(search.toLowerCase())
  );

  const addToCart = (item: Item) => {
    setCart((prev) => {
      const existing = prev.find((si) => si.itemId === item.id);
      if (existing) {
        return prev.map((si) =>
          si.itemId === item.id ? { ...si, quantity: si.quantity + 1 } : si
        );
      }
      return [
        ...prev,
        {
          itemId: item.id,
          description: item.name,
          quantity: 1,
          itemCostPrice: item.costPrice,
          itemUnitPrice: item.unitPrice,
          discount: 0,
          discountType: 0,
        },
      ];
    });
  };

  const updateQuantity = (itemId: number, delta: number) => {
    setCart((prev) =>
      prev.map((si) => {
        if (si.itemId === itemId) {
          const newQ = Math.max(1, si.quantity + delta);
          return { ...si, quantity: newQ };
        }
        return si;
      })
    );
  };

  const removeFromCart = (itemId: number) => {
    setCart((prev) => prev.filter((si) => si.itemId !== itemId));
  };

  const subtotal = cart.reduce((sum, item) => sum + item.itemUnitPrice * item.quantity, 0);
  const tax = subtotal * 0.1;
  const total = subtotal + tax;

  const handleCheckoutClick = () => {
    if (cart.length === 0) return;
    setTenderedAmount('');
    setShowPaymentModal(true);
  };

  const handleCheckout = async (e?: React.FormEvent) => {
    if (e) e.preventDefault();
    if (cart.length === 0) return;
    if (typeof tenderedAmount !== 'number' || tenderedAmount < total) return;
    
    setProcessing(true);
    try {
      const saleItems = cart.map((item) => ({
        itemId: item.itemId,
        quantity: item.quantity,
        discountPercent: item.discount,
        unitPriceOverride: item.itemUnitPrice,
        serialNumber: '',
        locationId: 1
      }));

      const payments = [
        {
          paymentMethod: 0,
          amount: total
        }
      ];

      const res = await fetchApi<{ data: number } | number>('/sales', {
        method: 'POST',
        body: JSON.stringify({
          customerId: selectedCustomer === '' ? null : selectedCustomer,
          employeeId: 1,
          comment: 'POS Sale',
          saleItems: saleItems,
          payments: payments
        })
      });
      
      const saleId = (res && typeof res === 'object' && 'data' in res) ? res.data : (res as number);
      
      const customerObj = customers.find(c => c.id === selectedCustomer);
      setReceiptData({
        saleId: saleId,
        cart: [...cart],
        total: total,
        tendered: tenderedAmount,
        change: tenderedAmount - total,
        customerName: customerObj ? `${customerObj.firstName} ${customerObj.lastName}` : 'Walk-in Customer'
      });

      setCart([]);
      setShowPaymentModal(false);
      setCheckoutMsg({ type: 'success', text: t('Sale completed successfully!') });
      setTimeout(() => setCheckoutMsg(null), 4000);
    } catch (err: any) {
      console.error(err);
      setCheckoutMsg({ type: 'error', text: err.message || t('Checkout failed. Please try again.') });
    } finally {
      setProcessing(false);
    }
  };

  return (
    <div className="pos-container">
      {checkoutMsg && (
        <div
          className={`pos-notification pos-notification--${checkoutMsg.type}`}
          onClick={() => setCheckoutMsg(null)}
        >
          {checkoutMsg.text}
        </div>
      )}
      <div className="pos-products">
        <div className="search-bar">
          <Search size={20} className="search-icon" />
          <input
            type="text"
            className="input search-input"
            placeholder={t('Search items...')}
            value={search}
            onChange={(e) => setSearch(e.target.value)}
          />
        </div>
        
        <div className="product-grid">
          {filteredItems.map((item) => {
            const qty = item.itemQuantities?.reduce((sum, iq) => sum + iq.quantity, 0) || 0;
            return (
              <button
                key={item.id}
                className="product-card"
                onClick={() => addToCart(item)}
              >
                <div className="product-name">{item.name}</div>
                <div className="product-price">{formatCurrency(item.unitPrice)}</div>
                <div className="product-qty" style={{ fontSize: '0.8rem', color: qty > 0 ? 'green' : 'red' }}>
                  {t('Stock')}: {qty}
                </div>
              </button>
            );
          })}
          {filteredItems.length === 0 && (
            <div className="empty-state">{t('No items found')}</div>
          )}
        </div>
      </div>

      <div className="pos-cart">
        <div className="cart-header">
          <div style={{ display: 'flex', justifyContent: 'space-between', alignItems: 'center' }}>
            <h3>{t('Current Sale')}</h3>
            <select 
              value={selectedCustomer} 
              onChange={(e) => setSelectedCustomer(e.target.value ? Number(e.target.value) : '')}
              style={{ padding: '4px', borderRadius: '4px', border: '1px solid var(--color-border)' }}
            >
              <option value="">{t('Walk-in Customer')}</option>
              {customers.map(c => (
                <option key={c.id} value={c.id}>{c.firstName} {c.lastName}</option>
              ))}
            </select>
          </div>
        </div>
        
        <div className="cart-items">
          {cart.length === 0 ? (
            <div className="cart-empty">{t('Cart is empty')}</div>
          ) : (
            cart.map((item) => (
              <div key={item.itemId} className="cart-item">
                <div className="cart-item-details">
                  <div className="cart-item-title">{item.description}</div>
                  <div className="cart-item-price">{formatCurrency(item.itemUnitPrice)}</div>
                </div>
                <div className="cart-item-actions">
                  <button className="btn-qty" onClick={() => updateQuantity(item.itemId, -1)}>
                    <Minus size={14} />
                  </button>
                  <span className="qty">{item.quantity}</span>
                  <button className="btn-qty" onClick={() => updateQuantity(item.itemId, 1)}>
                    <Plus size={14} />
                  </button>
                  <button className="btn-qty btn-remove" onClick={() => removeFromCart(item.itemId)}>
                    <Trash2 size={14} />
                  </button>
                </div>
              </div>
            ))
          )}
        </div>

        <div className="cart-summary">
          <div className="summary-row">
            <span>{t('Subtotal')}</span>
            <span>{formatCurrency(subtotal)}</span>
          </div>
          <div className="summary-row">
            <span>{t('Tax (10%)')}</span>
            <span>{formatCurrency(tax)}</span>
          </div>
          <div className="summary-row total-row">
            <span>{t('Total')}</span>
            <span>{formatCurrency(total)}</span>
          </div>
          
          <button 
            className="btn btn-primary btn-checkout" 
            disabled={cart.length === 0}
            onClick={handleCheckoutClick}
          >
            <CreditCard size={20} />
            {t('Checkout')} {formatCurrency(total)}
          </button>
        </div>
      </div>

      {showPaymentModal && (
        <div className="pos-modal-overlay">
          <div className="pos-modal-content">
            <div className="pos-modal-header">
              <h3>{t('Payment')}</h3>
              <button className="btn-icon" onClick={() => setShowPaymentModal(false)}>
                <X size={20} />
              </button>
            </div>
            <form onSubmit={handleCheckout}>
              <div className="pos-modal-body">
                <div className="summary-row">
                  <span>{t('Total Due')}</span>
                  <span className="total-row">{formatCurrency(total)}</span>
                </div>
                
                <div className="payment-input-group">
                  <label>{t('Tendered Amount')}</label>
                  <input 
                    type="number"
                    step="0.01"
                    min={total}
                    required
                    className="payment-input"
                    value={tenderedAmount}
                    onChange={(e) => setTenderedAmount(e.target.value ? parseFloat(e.target.value) : '')}
                    autoFocus
                  />
                </div>

                {typeof tenderedAmount === 'number' && tenderedAmount >= total && (
                  <div className="summary-row" style={{ color: '#2e7d32', fontWeight: 'bold' }}>
                    <span>{t('Change')} (Kembalian)</span>
                    <span>{formatCurrency(tenderedAmount - total)}</span>
                  </div>
                )}
              </div>
              <div className="pos-modal-footer">
                <button type="button" className="btn" onClick={() => setShowPaymentModal(false)} disabled={processing}>{t('Cancel')}</button>
                <button 
                  type="submit" 
                  className="btn btn-primary" 
                  disabled={processing || typeof tenderedAmount !== 'number' || tenderedAmount < total}
                >
                  {processing ? t('Processing...') : t('Confirm Payment')}
                </button>
              </div>
            </form>
          </div>
        </div>
      )}

      {receiptData && (
        <ReceiptModal
          saleId={receiptData.saleId}
          cart={receiptData.cart}
          total={receiptData.total}
          tendered={receiptData.tendered}
          change={receiptData.change}
          customerName={receiptData.customerName}
          onClose={() => setReceiptData(null)}
        />
      )}
    </div>
  );
}
