import { BaseGetOptions } from './common.models';

export interface Delivery
{
    id?: number;
    deliveryPrice: number;
    name: string;
}

export interface DeliveryGetOptions extends BaseGetOptions
{
    search?: string;
}