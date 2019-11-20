import { Component } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { MatTableDataSource } from '@angular/material/table';
import { Product, ProductGetOptions, Cart } from '../../models/product.models';
import { ProductService } from '../../services/product.service';
import { ReportService } from 'src/app/services/report.service';
import { CartService } from 'src/app/services/cart.service';

@Component({
    selector: 'products',
    templateUrl: './products.component.html',
    styleUrls: ['../../app.component.css']
})
export class ProductsComponent {
    products: MatTableDataSource<Product>;
    id?: number;
    cartSummary: number = 0;
    isEmpty: boolean;
    loading: boolean = true;
    displayedColumns: string[] = ['id', 'addToCart', 'name', 'price', 'count'];

    constructor(public dialog: MatDialog, private _service: ProductService, private _cartService: CartService, private _reportService: ReportService) {
        this.products = new MatTableDataSource<Product>();
    }

    ngOnInit() {
        this.getProducts();
    }

    addToCart(product: Product): void {
        const cartProduct: Cart = { ...product, id: 0, count: 1, productId: product.id };
        this.updateCart(cartProduct);
    }

    filter(filterValue: string): void {
        this.products.filter = filterValue;
    }

    private updateCart(cart: Cart): void {
        this._cartService.addToCart(cart).subscribe(null, null, () => this.getProducts());
    }

    private getCartSummary(): void {
        this._reportService.getCartReport({}).subscribe((result) => {
            this.cartSummary = result.summary;
            this.loading = false;
        });
    }

    private getProducts(): void {
        this.loading = true;
        this._service.get({}).subscribe((result) => {
            this.products.data = result;
            this.isEmpty = !this.products.data;
        },
            null,
            () => this.getCartSummary());
    }
}