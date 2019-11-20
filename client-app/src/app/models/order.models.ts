import { BaseGetOptions } from './common.models';

export interface Order {
    id?: number;
    deliveryId: number;
    paymentMethod: number;
    fullName: string;
    addres: string;
    specialDate?: Date;
    toSpecialDate: boolean;
}

export interface OrderGetOptions extends BaseGetOptions
{
    search?: string;
}