import { OrderItem } from "./order-item.model";

export interface Order {
  id: number;
  orderDate: string;
  items: OrderItem[];
  totalAmount: number;
}