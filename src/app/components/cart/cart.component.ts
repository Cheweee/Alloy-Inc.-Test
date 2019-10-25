import { Component } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { MatTableDataSource } from '@angular/material/table';
import { Product } from '../../models/product.models';
import { ProductService } from '../../services/product.service';
import { CartProductComponent } from '../cart-product/cart-product.component';

@Component({
    selector: 'cart',
    templateUrl: './cart.component.html',
    styleUrls: ['../../app.component.css']
})
export class CartComponent {
    products: MatTableDataSource<Product>;
    id?: number;
    readonly cartSummary: number;
    isEmpty: boolean;
    displayedColumns: string[] = ['id', 'edit', 'name', 'price', 'count', 'summary', 'delete'];

    constructor(public dialog: MatDialog, private _service: ProductService) {
        this.products = new MatTableDataSource<Product>();
        this.cartSummary = _service.cartSummary;
    }

    ngOnInit(): void {
        this.getProducts();
    }

    edit(product: Product): void {
        const productMaxCount = this._service.products.find(o => o.id == product.id).count;
        const dialogRef = this.dialog.open(CartProductComponent, {
            data: {
                product: { ...product },
                maxCount: productMaxCount
            }
        });

        dialogRef.afterClosed().subscribe(result => {
            if (result)
                this._service.setProductCountInCart(result.product.id, result.product.count);

            this.getProducts();
        });
    }

    delete(id: number): void {
        this.deleteFromCart(id);
    }

    filter(filterValue: string): void {
        this.products.filter = filterValue;
    }

    private getProducts(): void {
        this.products.data = this._service.cart;
        this.isEmpty = !this.products.data;
    }

    private deleteFromCart(id: number) {
        this._service.deleteFromCart(id);
        this.getProducts();
    }
}