import i18n from 'i18next';
import { initReactI18next } from 'react-i18next';

// the translations
const resources = {
  en: {
    translation: {
      "NETPOS_ADMIN": "NETPOS ADMIN",
      "Point of Sale": "Point of Sale",
      "Inventory": "Inventory",
      "Items": "Items",
      "Item Categories": "Item Categories",
      "Item Kits": "Item Kits",
      "People": "People",
      "Customers": "Customers",
      "Suppliers": "Suppliers",
      "Employees": "Employees",
      "Operations": "Operations",
      "Expenses": "Expenses",
      "Receivings": "Receivings",
      "Sales History": "Sales History",
      "Taxes": "Taxes",
      "General": "General",
      "Messages": "Messages",
      "Gift Cards": "Gift Cards",
      "Reports": "Reports",
      "Settings": "Settings",
      "Logout": "Logout",
      "Admin Dashboard": "Admin Dashboard",
      
      // CrudDataTable
      "Loading...": "Loading...",
      "No records found": "No records found",
      "Actions": "Actions",
      "Cancel": "Cancel",
      "Save": "Save",
      "Saving...": "Saving...",
      "Edit": "Edit",
      "New": "New",
      "Select an option": "Select an option",
      "Are you sure you want to delete this": "Are you sure you want to delete this",
      
      // Items Page
      "Item Name": "Item Name",
      "Category": "Category",
      "Cost Price": "Cost Price",
      "Unit Price": "Unit Price",
      "Description": "Description",
      
      // Categories
      "Category Name": "Category Name",
      
      // POS
      "Tendered Amount": "Tendered Amount",
      "Change": "Change",
      "Payment Type": "Payment Type",
      "Total": "Total",
      "Checkout": "Checkout"
    }
  },
  id: {
    translation: {
      "NETPOS_ADMIN": "ADMIN NETPOS",
      "Point of Sale": "Kasir",
      "Inventory": "Inventaris",
      "Items": "Barang",
      "Item Categories": "Kategori Barang",
      "Item Kits": "Paket Barang",
      "People": "Orang",
      "Customers": "Pelanggan",
      "Suppliers": "Pemasok",
      "Employees": "Karyawan",
      "Operations": "Operasional",
      "Expenses": "Pengeluaran",
      "Receivings": "Penerimaan",
      "Sales History": "Riwayat Penjualan",
      "Taxes": "Pajak",
      "General": "Umum",
      "Messages": "Pesan",
      "Gift Cards": "Kartu Hadiah",
      "Reports": "Laporan",
      "Settings": "Pengaturan",
      "Logout": "Keluar",
      "Admin Dashboard": "Dasbor Admin",
      
      // CrudDataTable
      "Loading...": "Memuat...",
      "No records found": "Tidak ada data",
      "Actions": "Aksi",
      "Cancel": "Batal",
      "Save": "Simpan",
      "Saving...": "Menyimpan...",
      "Edit": "Ubah",
      "New": "Baru",
      "Select an option": "Pilih salah satu",
      "Are you sure you want to delete this": "Apakah Anda yakin ingin menghapus ini",
      
      // Items Page
      "Item Name": "Nama Barang",
      "Category": "Kategori",
      "Cost Price": "Harga Modal",
      "Unit Price": "Harga Jual",
      "Description": "Keterangan",
      
      // Categories
      "Category Name": "Nama Kategori",
      
      // POS
      "Tendered Amount": "Jumlah Uang",
      "Change": "Kembalian",
      "Payment Type": "Tipe Pembayaran",
      "Total": "Total",
      "Checkout": "Bayar"
    }
  }
};

i18n
  .use(initReactI18next)
  .init({
    resources,
    lng: "en",
    fallbackLng: "en",
    interpolation: {
      escapeValue: false
    }
  });

export default i18n;
