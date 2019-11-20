import { Component, Input, Output } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { Cart } from '../../models/product.models';
import { OrderService } from 'src/app/services/order.service';
import { DeliveryInfoComponent } from '../delivery-info/delivery-info.component';
import { Delivery } from 'src/app/models/delivery.models';
import { DeliveryService } from 'src/app/services/delivery.service';
import { Order } from 'src/app/models/order.models';
import { EventEmitter } from '@angular/core';

@Component({
    selector: 'order',
    templateUrl: './order.component.html',
    styleUrls: ['../../app.component.css']
})
export class OrderComponent {
    @Input() public products: Cart[] = [];
    @Output() orderPlaced: EventEmitter<any> = new EventEmitter();
    isEmpty: boolean = true;
    deliveries: Delivery[];
    total: number = 0;

    constructor(private dialog: MatDialog,
        private _orderService: OrderService,
        private _deliveryService: DeliveryService) { }

    ngOnInit() {
        this._deliveryService.get({}).subscribe((result) => {
            this.deliveries = result;
        });
    }

    ngOnChanges(): void {
        this.isEmpty = this.products.length === 0;
        this.total = 0;
        for (const product of this.products) {
            this.total += product.price * product.count;
        }
    }

    public onBuyClick() {
        const newOrder: Order = {
            specialDate: undefined,
            paymentMethod: 0,
            addres: '',
            deliveryId: undefined,
            fullName: '',
            id: undefined,
            toSpecialDate: false
        };
        const dialogRef = this.dialog.open(DeliveryInfoComponent, {
            data: { subtotal: this.total, order: newOrder, deliveries: this.deliveries }
        });

        dialogRef.afterClosed().subscribe(result => {
            if (result) {
                const order = result.order;
                order.products = this.products;
                this._orderService.create(order).subscribe((result) => this.orderPlaced.emit(order));
            }
        });
    }
}