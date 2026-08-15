export interface Item {
  id: number;
  name: string;
  category: string;
  costPrice: number;
  unitPrice: number;
  tax1Rate: number;
  tax1Name: string;
  tax2Rate: number;
  tax2Name: string;
  isTaxIncluded: boolean;
  stockType: number;
  itemType: number;
  deleted: boolean;
}

export interface Customer {
  id: number;
  firstName: string;
  lastName: string;
  email: string;
  phoneNumber: string;
  companyName: string;
  deleted: boolean;
}

export interface Sale {
  id: number;
  saleTime: string;
  customerId?: number;
  employeeId: number;
  comment: string;
  invoiceNumber: string;
  discountValue: number;
  discountType: number;
  saleStatus: number;
  paymentType: string;
  deleted: boolean;
  saleItems: SaleItem[];
}

export interface SaleItem {
  id?: number;
  itemId: number;
  description: string;
  quantity: number;
  itemCostPrice: number;
  itemUnitPrice: number;
  discount: number;
  discountType: number;
}
