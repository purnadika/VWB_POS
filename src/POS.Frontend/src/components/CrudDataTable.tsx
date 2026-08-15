import React, { useState, useEffect, createContext, useContext } from 'react';

import { Plus, Edit2, Trash2, X } from 'lucide-react';
import { fetchApi } from '../utils/api';
import '../pages/AdminPages.css';

export interface ColumnDef<T> {
  key: keyof T | string;
  header: string;
  render?: (row: T) => React.ReactNode;
}

export interface FormFieldDef {
  name: string;
  label: string;
  type: 'text' | 'number' | 'email' | 'checkbox' | 'date';
  required?: boolean;
}

interface CrudDataTableProps<T> {
  title: string;
  endpoint: string;
  columns: ColumnDef<T>[];
  formFields: FormFieldDef[];
  primaryKey?: string; // default to 'id'
}

export function CrudDataTable<T extends Record<string, any>>({
  title,
  endpoint,
  columns,
  formFields,
  primaryKey = 'id',
}: CrudDataTableProps<T>) {
  const [data, setData] = useState<T[]>([]);
  const [loading, setLoading] = useState(true);
  
  // Modal state
  const [isModalOpen, setIsModalOpen] = useState(false);
  const [editingItem, setEditingItem] = useState<T | null>(null);
  const [formData, setFormData] = useState<Record<string, any>>({});
  const [saving, setSaving] = useState(false);
  const [error, setError] = useState<string | null>(null);

  const loadData = async () => {
    try {
      const res = await fetchApi<{ data: T[] } | T[]>(endpoint);
      if (res && typeof res === 'object' && 'data' in res && Array.isArray(res.data)) {
        setData(res.data);
      } else if (Array.isArray(res)) {
        setData(res);
      } else {
        setData([]);
      }
    } catch (err) {
      console.error(err);
    } finally {
      setLoading(false);
    }
  };

  useEffect(() => {
    loadData();
  }, [endpoint]);

  const handleDelete = async (id: any) => {
    if (!confirm(`Are you sure you want to delete this ${title.toLowerCase().slice(0, -1)}?`)) return;
    try {
      await fetchApi(`${endpoint}/${id}`, { method: 'DELETE' });
      await loadData();
    } catch (err: any) {
      alert(err.message || `Failed to delete ${title.toLowerCase().slice(0, -1)}.`);
    }
  };

  const openModal = (item?: T) => {
    setError(null);
    if (item) {
      setEditingItem(item);
      setFormData({ ...item });
    } else {
      setEditingItem(null);
      setFormData({});
    }
    setIsModalOpen(true);
  };

  const closeModal = () => {
    setIsModalOpen(false);
    setEditingItem(null);
    setFormData({});
  };

  const handleInputChange = (name: string, value: any, type: string) => {
    let parsedValue = value;
    if (type === 'number') parsedValue = value === '' ? '' : Number(value);
    setFormData((prev) => ({ ...prev, [name]: parsedValue }));
  };

  const handleSubmit = async (e: React.FormEvent) => {
    e.preventDefault();
    setSaving(true);
    setError(null);
    try {
      if (editingItem) {
        // Update
        const id = editingItem[primaryKey];
        await fetchApi(`${endpoint}/${id}`, {
          method: 'PUT',
          body: JSON.stringify(formData),
        });
      } else {
        // Create
        await fetchApi(endpoint, {
          method: 'POST',
          body: JSON.stringify(formData),
        });
      }
      closeModal();
      await loadData();
    } catch (err: any) {
      console.error(err);
      setError(err.message || 'Failed to save data.');
    } finally {
      setSaving(false);
    }
  };

  return (
    <div className="admin-page">
      <div className="page-header">
        <h2>{title}</h2>
        <button className="btn btn-primary" onClick={() => openModal()}>
          <Plus size={16} />
          New {title.slice(0, -1)}
        </button>
      </div>

      <div className="table-container">
        <table>
          <thead>
            <tr>
              {columns.map((col, i) => (
                <th key={i}>{col.header}</th>
              ))}
              <th>Actions</th>
            </tr>
          </thead>
          <tbody>
            {loading ? (
              <tr><td colSpan={columns.length + 1} style={{ textAlign: 'center' }}>Loading...</td></tr>
            ) : data.length === 0 ? (
              <tr><td colSpan={columns.length + 1} style={{ textAlign: 'center' }}>No records found</td></tr>
            ) : (
              data.map((row, rowIndex) => (
                <tr key={row[primaryKey] || rowIndex}>
                  {columns.map((col, colIndex) => (
                    <td key={colIndex}>
                      {col.render ? col.render(row) : (row[col.key as keyof T] as React.ReactNode)}
                    </td>
                  ))}
                  <td>
                    <div className="action-buttons">
                      <button className="btn-icon" aria-label="Edit" onClick={() => openModal(row)}>
                        <Edit2 size={16} />
                      </button>
                      <button className="btn-icon text-danger" aria-label="Delete" onClick={() => handleDelete(row[primaryKey])}>
                        <Trash2 size={16} />
                      </button>
                    </div>
                  </td>
                </tr>
              ))
            )}
          </tbody>
        </table>
      </div>

      {isModalOpen && (
        <div className="modal-overlay">
          <div className="modal-content">
            <div className="modal-header">
              <h3>{editingItem ? 'Edit' : 'New'} {title.slice(0, -1)}</h3>
              <button className="btn-icon" onClick={closeModal}>
                <X size={20} />
              </button>
            </div>
            <form onSubmit={handleSubmit} style={{ display: 'flex', flexDirection: 'column', overflow: 'hidden' }}>
              <div className="modal-body">
                {error && <div className="alert-error" style={{ color: 'red', marginBottom: '16px', padding: '10px', backgroundColor: '#ffebee', borderRadius: '4px' }}>{error}</div>}
                {formFields.map((field) => (
                  <div className="form-group" key={field.name}>
                    <label htmlFor={`field-${field.name}`}>{field.label}</label>
                    <input
                      id={`field-${field.name}`}
                      type={field.type}
                      required={field.required}
                      value={
                        formData[field.name] === undefined 
                          ? '' 
                          : (field.type === 'date' && formData[field.name] 
                              ? String(formData[field.name]).split('T')[0] 
                              : formData[field.name])
                      }
                      onChange={(e) => handleInputChange(field.name, field.type === 'checkbox' ? e.target.checked : e.target.value, field.type)}
                      className="form-control"
                    />
                  </div>
                ))}
              </div>
              <div className="modal-footer">
                <button type="button" className="btn" onClick={closeModal} disabled={saving}>Cancel</button>
                <button type="submit" className="btn btn-primary" disabled={saving}>
                  {saving ? 'Saving...' : 'Save'}
                </button>
              </div>
            </form>
          </div>
        </div>
      )}
    </div>
  );
}
