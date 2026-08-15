
import { Outlet, NavLink, useNavigate } from 'react-router-dom';
import { ShoppingCart, Package, Users, Settings, LogOut } from 'lucide-react';
import { useAuth } from '../contexts/AuthContext';
import './AdminLayout.css';

export function AdminLayout() {
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
          <h2>NETPOS</h2>
          <span className="badge">ADMIN</span>
        </div>
        
        <nav className="sidebar-nav">
          <NavLink to="/" className={({ isActive }) => `nav-item ${isActive ? 'active' : ''}`} end>
            <ShoppingCart size={18} />
            Point of Sale
          </NavLink>
          <div className="nav-divider"></div>
          <NavLink to="/admin/items" className={({ isActive }) => `nav-item ${isActive ? 'active' : ''}`}>
            <Package size={18} />
            Items
          </NavLink>
          <NavLink to="/admin/item-categories" className={({ isActive }) => `nav-item ${isActive ? 'active' : ''}`}>
            <Package size={18} />
            Item Categories
          </NavLink>
          <NavLink to="/admin/item-kits" className={({ isActive }) => `nav-item ${isActive ? 'active' : ''}`}>
            <Package size={18} />
            Item Kits
          </NavLink>
          <NavLink to="/admin/customers" className={({ isActive }) => `nav-item ${isActive ? 'active' : ''}`}>
            <Users size={18} />
            Customers
          </NavLink>
          <NavLink to="/admin/suppliers" className={({ isActive }) => `nav-item ${isActive ? 'active' : ''}`}>
            <Users size={18} />
            Suppliers
          </NavLink>
          <NavLink to="/admin/employees" className={({ isActive }) => `nav-item ${isActive ? 'active' : ''}`}>
            <Users size={18} />
            Employees
          </NavLink>
          <NavLink to="/admin/gift-cards" className={({ isActive }) => `nav-item ${isActive ? 'active' : ''}`}>
            <Package size={18} />
            Gift Cards
          </NavLink>
          <NavLink to="/admin/messages" className={({ isActive }) => `nav-item ${isActive ? 'active' : ''}`}>
            <Settings size={18} />
            Messages
          </NavLink>
          <NavLink to="/admin/expenses" className={({ isActive }) => `nav-item ${isActive ? 'active' : ''}`}>
            <Settings size={18} />
            Expenses
          </NavLink>
          <NavLink to="/admin/receivings" className={({ isActive }) => `nav-item ${isActive ? 'active' : ''}`}>
            <ShoppingCart size={18} />
            Receivings
          </NavLink>
          <NavLink to="/admin/reports" className={({ isActive }) => `nav-item ${isActive ? 'active' : ''}`}>
            <Settings size={18} />
            Reports
          </NavLink>
          <NavLink to="/admin/taxes" className={({ isActive }) => `nav-item ${isActive ? 'active' : ''}`}>
            <Settings size={18} />
            Taxes
          </NavLink>
          <div className="nav-divider"></div>
          <NavLink to="/admin/settings" className={({ isActive }) => `nav-item ${isActive ? 'active' : ''}`}>
            <Settings size={18} />
            Settings
          </NavLink>
        </nav>

        <div className="sidebar-footer">
          <button className="nav-item btn-logout" onClick={handleLogout}>
            <LogOut size={18} />
            Logout
          </button>
        </div>
      </aside>
      
      <main className="main-content">
        <header className="main-header">
          <h1>Admin Dashboard</h1>
        </header>
        <div className="content-area">
          <Outlet />
        </div>
      </main>
    </div>
  );
}
