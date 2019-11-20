import { BaseGetOptions } from './common.models';

export interface Product {
    id: number;
    price: number;
    count: number;
    name: string;
}

export interface Cart extends Product {
    id: number;
    productId?: number;
    orderId?: number;
    price: number;
    count: number;
    name: string;
}

export class ProductGetOptions extends BaseGetOptions {
    search?: string;
}

export interface CartGetOptions extends ProductGetOptions {
    ordered?: boolean;
    orderId?: number;
    orderIds?: number[];
}