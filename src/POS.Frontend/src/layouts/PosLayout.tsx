
import { Outlet, Link, useNavigate } from 'react-router-dom';
import { Shield, LogOut } from 'lucide-react';
import { useAuth } from '../contexts/AuthContext';
import './PosLayout.css';

export function PosLayout() {
  const { logout } = useAuth();
  const navigate = useNavigate();

  const handleLogout = () => {
    logout();
    navigate('/login');
  };
  return (
    <div className="pos-layout">
      <header className="pos-header">
        <div className="pos-brand">
          <h2>NETPOS</h2>
        </div>
        <div className="pos-actions">
          <Link to="/admin" className="btn btn-outline btn-sm">
            <Shield size={16} />
            Admin Panel
          </Link>
          <button className="btn btn-outline btn-sm" onClick={handleLogout} style={{marginLeft: '8px'}}>
            <LogOut size={16} />
            Logout
          </button>
        </div>
      </header>
      <main className="pos-main">
        <Outlet />
      </main>
    </div>
  );
}
