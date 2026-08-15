import React, { useState, useEffect, createContext, useContext } from 'react';

import { Search, Plus, Minus, Trash2, CreditCard } from 'lucide-react';
import type { Item, SaleItem } from '../types';
import { fetchApi } from '../utils/api';
import { useLocale } from '../contexts/LocaleContext';
import './PosPage.css';

export function PosPage() {
  const [items, setItems] = useState<Item[]>([]);
  const [search, setSearch] = useState('');
  const [cart, setCart] = useState<SaleItem[]>([]);
  const [checkoutMsg, setCheckoutMsg] = useState<{ type: 'success' | 'error'; text: string } | null>(null);
  const { formatCurrency } = useLocale();

  useEffect(() => {
    fetchApi<{ data: Item[] }>('/items')
      .then((res) => {
        // Ensure data is array
        if (Array.isArray(res.data)) {
          setItems(res.data);
        } else if (Array.isArray(res)) {
          setItems(res as unknown as Item[]);
        }
      })
      .catch((err) => console.error('Error fetching items', err));
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
  const tax = subtotal * 0.1; // Hardcoded 10% tax for demo
  const total = subtotal + tax;

  const handleCheckout = async () => {
    if (cart.length === 0) return;
    
    try {
      // Map cart to SaleItemDto format expected by the backend
      const saleItems = cart.map((item) => ({
        itemId: item.itemId,
        quantity: item.quantity,
        discountPercent: item.discount,
        unitPriceOverride: item.itemUnitPrice,
        serialNumber: '', // Provide empty string to satisfy validation
        locationId: 1 // Default location
      }));

      // Map payments to SalePaymentDto format expected by the backend
      const payments = [
        {
          paymentMethod: 0, // 0 = Cash based on backend enum
          amount: total
        }
      ];

      await fetchApi('/sales', {
        method: 'POST',
        body: JSON.stringify({
          employeeId: 1, // Default employee
          comment: 'POS Sale',
          saleItems: saleItems,
          payments: payments
        })
      });
      setCart([]);
      setCheckoutMsg({ type: 'success', text: 'Sale completed successfully!' });
      setTimeout(() => setCheckoutMsg(null), 4000);
    } catch (err: any) {
      console.error(err);
      setCheckoutMsg({ type: 'error', text: err.message || 'Checkout failed. Please try again.' });
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
      {/* Left Area: Product Selection */}
      <div className="pos-products">
        <div className="search-bar">
          <Search size={20} className="search-icon" />
          <input
            type="text"
            className="input search-input"
            placeholder="Search items..."
            value={search}
            onChange={(e) => setSearch(e.target.value)}
          />
        </div>
        
        <div className="product-grid">
          {filteredItems.map((item) => (
            <button
              key={item.id}
              className="product-card"
              onClick={() => addToCart(item)}
            >
              <div className="product-name">{item.name}</div>
              <div className="product-price">{formatCurrency(item.unitPrice)}</div>
            </button>
          ))}
          {filteredItems.length === 0 && (
            <div className="empty-state">No items found</div>
          )}
        </div>
      </div>

      {/* Right Area: Cart / Checkout Panel (Inverted Metro Style) */}
      <div className="pos-cart">
        <div className="cart-header">
          <h3>Current Sale</h3>
        </div>
        
        <div className="cart-items">
          {cart.length === 0 ? (
            <div className="cart-empty">Cart is empty</div>
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
            <span>Subtotal</span>
            <span>{formatCurrency(subtotal)}</span>
          </div>
          <div className="summary-row">
            <span>Tax (10%)</span>
            <span>{formatCurrency(tax)}</span>
          </div>
          <div className="summary-row total-row">
            <span>Total</span>
            <span>{formatCurrency(total)}</span>
          </div>
          
          <button 
            className="btn btn-primary btn-checkout" 
            disabled={cart.length === 0}
            onClick={handleCheckout}
          >
            <CreditCard size={20} />
            Pay {formatCurrency(total)}
          </button>
        </div>
      </div>
    </div>
  );
}
