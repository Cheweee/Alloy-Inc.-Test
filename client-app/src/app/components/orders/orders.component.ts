import { Component } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { MatTableDataSource } from '@angular/material/table';
import { Order, OrderGetOptions } from '../../models/order.models';
import { OrderService } from '../../services/order.service';
import { ReportService } from 'src/app/services/report.service';
import { CartService } from 'src/app/services/cart.service';
import { OrderInfoComponent } from '../order-info/order-info.component';
import { DeliveryService } from 'src/app/services/delivery.service';
import { Cart } from 'src/app/models/product.models';
import { Delivery } from 'src/app/models/delivery.models';

@Component({
    selector: 'orders',
    templateUrl: './orders.component.html',
    styleUrls: ['../../app.component.css']
})
export class OrdersComponent {
    orders: MatTableDataSource<Order>;
    orderedProducts: Cart[];
    usedDeliveries: Delivery[];
    id?: number;
    cartSummary: number = 0;
    isEmpty: boolean;
    loading: boolean = false;
    displayedColumns: string[] = ['id', 'fullName', 'address', 'show'];

    constructor(public dialog: MatDialog,
        private _service: OrderService,
        private _cartService: CartService,
        private _reportService: ReportService,
        private _deliveryService: DeliveryService
    ) {
        this.orders = new MatTableDataSource<Order>();
    }

    ngOnInit() {
        this.getOrders();
    }

    filter(filterValue: string): void {
        this.orders.filter = filterValue;
    }

    showOrder(order: Order) {
        const delivery = this.usedDeliveries.find(o => o.id === order.deliveryId);
        const products = this.orderedProducts.filter(o => o.orderId === order.id);
        this.dialog.open(OrderInfoComponent, {
            data: { order, delivery, products }
        });
    }

    private getCartSummary(): void {
        this._reportService.getCartReport({}).subscribe((result) => {
            this.cartSummary = result.summary;
            this.loading = false;
        });
    }

    private getDeliveries(): void {
        const deliveriesIds = this.orders.data.map(o => o.deliveryId);
        this._deliveryService.get({ ids: deliveriesIds }).subscribe((result) => {
            this.usedDeliveries = result;
        });
    }

    private getProducts(): void {
        const orderIds = this.orders.data.map(o => o.id);
        this._cartService.get({ orderIds: orderIds }).subscribe((result) => {
            this.orderedProducts = result;
        });
    }

    private getOrders(): void {
        this.loading = true;
        this._service.get({}).subscribe((result) => {
            this.orders.data = result;
            this.isEmpty = !this.orders.data;
            this.getProducts();
            this.getDeliveries();
        },
            null,
            () => this.getCartSummary());
    }
}