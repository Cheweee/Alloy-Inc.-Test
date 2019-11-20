import { Component, Inject } from '@angular/core';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { Order } from 'src/app/models/order.models';
import { Delivery } from 'src/app/models/delivery.models';
import { Cart } from 'src/app/models/product.models';

@Component({
    selector: 'order-info',
    templateUrl: './order-info.component.html',
})
export class OrderInfoComponent {
    public deliveryName: string = 'not shipping';
    public deliveryPrice: number = 0;
    public subtotal: number = 0;
    public total: number = 0;
    constructor(
        public dialogRef: MatDialogRef<OrderInfoComponent>,
        @Inject(MAT_DIALOG_DATA) public data: {
            order: Order,
            delivery: Delivery,
            products: Cart[]
        }) { }

    ngOnInit(): void {
        const products = this.data.products || [];
        this.deliveryName = this.data.delivery ? this.data.delivery.name : 'not shipping';
        this.deliveryPrice = this.data.delivery ? this.data.delivery.deliveryPrice : 0;
        this.subtotal = products.map(o => (o.count * o.price)).reduce((prev, current) => prev + current, 0);
        this.total = this.subtotal + this.deliveryPrice;
    }
}