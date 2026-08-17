import { Outlet, NavLink, useNavigate } from 'react-router-dom';
import { useTranslation } from 'react-i18next';
import { ShoppingCart, Package, Users, Settings, LogOut, FileText, Activity } from 'lucide-react';
import { useAuth } from '../contexts/AuthContext';
import './AdminLayout.css';

export function AdminLayout() {
  const { t } = useTranslation();
  const { logout } = useAuth();
  const navigate = useNavigate();

  const handleLogout = () => {
    logout();
    navigate('/login');
  };

  return (
    <div className="admin-layout">
      <aside className="sidebar">
        <div className="sidebar-header">
          <h2>{t('NETPOS_ADMIN')}</h2>
          <span className="badge">ADMIN</span>
        </div>
        
        <nav className="sidebar-nav">
          <NavLink to="/" className={({ isActive }) => `nav-item ${isActive ? 'active' : ''}`} end>
            <ShoppingCart size={18} />
            {t('Point of Sale')}
          </NavLink>
          
          <div className="nav-divider"></div>
          <div className="nav-group-title">{t('Inventory')}</div>
          <NavLink to="/admin/items" className={({ isActive }) => `nav-item ${isActive ? 'active' : ''}`}>
            <Package size={18} />
            {t('Items')}
          </NavLink>
          <NavLink to="/admin/item-categories" className={({ isActive }) => `nav-item ${isActive ? 'active' : ''}`}>
            <Package size={18} />
            {t('Item Categories')}
          </NavLink>
          <NavLink to="/admin/item-kits" className={({ isActive }) => `nav-item ${isActive ? 'active' : ''}`}>
            <Package size={18} />
            {t('Item Kits')}
          </NavLink>

          <div className="nav-divider"></div>
          <div className="nav-group-title">{t('People')}</div>
          <NavLink to="/admin/customers" className={({ isActive }) => `nav-item ${isActive ? 'active' : ''}`}>
            <Users size={18} />
            {t('Customers')}
          </NavLink>
          <NavLink to="/admin/suppliers" className={({ isActive }) => `nav-item ${isActive ? 'active' : ''}`}>
            <Users size={18} />
            {t('Suppliers')}
          </NavLink>
          <NavLink to="/admin/employees" className={({ isActive }) => `nav-item ${isActive ? 'active' : ''}`}>
            <Users size={18} />
            {t('Employees')}
          </NavLink>

          <div className="nav-divider"></div>
          <div className="nav-group-title">{t('Operations')}</div>
          <NavLink to="/admin/sales" className={({ isActive }) => `nav-item ${isActive ? 'active' : ''}`}>
            <Activity size={18} />
            {t('Sales History')}
          </NavLink>
          <NavLink to="/admin/expenses" className={({ isActive }) => `nav-item ${isActive ? 'active' : ''}`}>
            <Settings size={18} />
            {t('Expenses')}
          </NavLink>
          <NavLink to="/admin/receivings" className={({ isActive }) => `nav-item ${isActive ? 'active' : ''}`}>
            <ShoppingCart size={18} />
            {t('Receivings')}
          </NavLink>
          <NavLink to="/admin/taxes" className={({ isActive }) => `nav-item ${isActive ? 'active' : ''}`}>
            <Settings size={18} />
            {t('Taxes')}
          </NavLink>

          <div className="nav-divider"></div>
          <div className="nav-group-title">{t('General')}</div>
          <NavLink to="/admin/gift-cards" className={({ isActive }) => `nav-item ${isActive ? 'active' : ''}`}>
            <Package size={18} />
            {t('Gift Cards')}
          </NavLink>
          <NavLink to="/admin/messages" className={({ isActive }) => `nav-item ${isActive ? 'active' : ''}`}>
            <Settings size={18} />
            {t('Messages')}
          </NavLink>
          <NavLink to="/admin/reports" className={({ isActive }) => `nav-item ${isActive ? 'active' : ''}`}>
            <FileText size={18} />
            {t('Reports')}
          </NavLink>
          <NavLink to="/admin/settings" className={({ isActive }) => `nav-item ${isActive ? 'active' : ''}`}>
            <Settings size={18} />
            {t('Settings')}
          </NavLink>
        </nav>

        <div className="sidebar-footer">
          <button className="nav-item btn-logout" onClick={handleLogout}>
            <LogOut size={18} />
            {t('Logout')}
          </button>
        </div>
      </aside>
      
      <main className="main-content">
        <header className="main-header">
          <h1>{t('Admin Dashboard')}</h1>
        </header>
        <div className="content-area">
          <Outlet />
        </div>
      </main>
    </div>
  );
}
