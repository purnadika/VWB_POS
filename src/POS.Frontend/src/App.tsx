
import { BrowserRouter, Routes, Route, Navigate } from 'react-router-dom';
import { PosLayout } from './layouts/PosLayout';
import { AdminLayout } from './layouts/AdminLayout';
import { PosPage } from './pages/PosPage';
import { ItemsPage } from './pages/ItemsPage';
import { ItemCategoriesPage } from './pages/ItemCategoriesPage';
import { CustomersPage } from './pages/CustomersPage';
import { EmployeesPage } from './pages/EmployeesPage';
import { SuppliersPage } from './pages/SuppliersPage';
import { ItemKitsPage } from './pages/ItemKitsPage';
import { GiftCardsPage } from './pages/GiftCardsPage';
import { MessagesPage } from './pages/MessagesPage';
import { ExpensesPage } from './pages/ExpensesPage';
import { ReceivingsPage } from './pages/ReceivingsPage';
import { SalesPage } from './pages/SalesPage';
import { ReportsPage } from './pages/ReportsPage';
import { TaxesPage } from './pages/TaxesPage';
import { SettingsPage } from './pages/SettingsPage';

import { LoginPage } from './pages/LoginPage';
import { ProtectedRoute } from './components/ProtectedRoute';
import { AuthProvider } from './contexts/AuthContext';
import { LocaleProvider } from './contexts/LocaleContext';
import './i18n';

function App() {
  return (
    <AuthProvider>
      <LocaleProvider>
      <BrowserRouter>
        <Routes>
          <Route path="/login" element={<LoginPage />} />
          
          <Route element={<ProtectedRoute />}>
            {/* POS Route */}
            <Route element={<PosLayout />}>
              <Route path="/" element={<PosPage />} />
            </Route>
            
            {/* Admin Routes */}
            <Route path="/admin" element={<AdminLayout />}>
              <Route index element={<Navigate to="/admin/items" replace />} />
              <Route path="items" element={<ItemsPage />} />
              <Route path="item-categories" element={<ItemCategoriesPage />} />
              <Route path="item-kits" element={<ItemKitsPage />} />
              <Route path="customers" element={<CustomersPage />} />
              <Route path="suppliers" element={<SuppliersPage />} />
              <Route path="employees" element={<EmployeesPage />} />
              <Route path="gift-cards" element={<GiftCardsPage />} />
              <Route path="messages" element={<MessagesPage />} />
              <Route path="sales" element={<SalesPage />} />
              <Route path="expenses" element={<ExpensesPage />} />
              <Route path="receivings" element={<ReceivingsPage />} />
              <Route path="reports" element={<ReportsPage />} />
              <Route path="taxes" element={<TaxesPage />} />
              <Route path="settings" element={<SettingsPage />} />
            </Route>
          </Route>
        </Routes>
      </BrowserRouter>
      </LocaleProvider>
    </AuthProvider>
  );
}

export default App;
