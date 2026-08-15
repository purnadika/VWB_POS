import React, { useState, useEffect, createContext, useContext } from 'react';

import { fetchApi } from '../utils/api';

interface LocaleConfig {
  locale: string;       // e.g. "id-ID", "en-US"
  currency: string;     // e.g. "IDR", "USD"
  language: string;     // e.g. "id", "en"
}

interface LocaleContextType {
  config: LocaleConfig;
  formatCurrency: (amount: number) => string;
  formatDate: (date: string | Date) => string;
  isLoading: boolean;
}

const DEFAULT_CONFIG: LocaleConfig = {
  locale: 'en-US',
  currency: 'USD',
  language: 'en',
};

const LocaleContext = createContext<LocaleContextType | undefined>(undefined);

export function LocaleProvider({ children }: { children: React.ReactNode }) {
  const [config, setConfig] = useState<LocaleConfig>(DEFAULT_CONFIG);
  const [isLoading, setIsLoading] = useState(true);

  useEffect(() => {
    fetchApi<{ data: Array<{ key: string; value: string }> }>('/configuration')
      .then((res) => {
        const items = Array.isArray(res) ? res : (res?.data ?? []);
        const get = (key: string) => items.find((i: any) => i.key === key)?.value;
        setConfig({
          locale: get('locale') ?? DEFAULT_CONFIG.locale,
          currency: get('currency') ?? DEFAULT_CONFIG.currency,
          language: get('language') ?? DEFAULT_CONFIG.language,
        });
      })
      .catch(() => {
        // Keep defaults if config endpoint fails
      })
      .finally(() => setIsLoading(false));
  }, []);

  const formatCurrency = (amount: number): string => {
    try {
      return new Intl.NumberFormat(config.locale, {
        style: 'currency',
        currency: config.currency,
        minimumFractionDigits: 0,
        maximumFractionDigits: 2,
      }).format(amount);
    } catch {
      return `${config.currency} ${amount.toFixed(2)}`;
    }
  };

  const formatDate = (date: string | Date): string => {
    try {
      return new Intl.DateTimeFormat(config.locale, {
        dateStyle: 'medium',
      }).format(new Date(date));
    } catch {
      return String(date);
    }
  };

  return (
    <LocaleContext.Provider value={{ config, formatCurrency, formatDate, isLoading }}>
      {children}
    </LocaleContext.Provider>
  );
}

export function useLocale(): LocaleContextType {
  const context = useContext(LocaleContext);
  if (!context) throw new Error('useLocale must be used within a LocaleProvider');
  return context;
}
