import { Component } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { MatTableDataSource } from '@angular/material/table';
import { Product } from '../../models/product.models';
import { ProductService } from '../../services/product.service';
import { CartProductComponent } from '../cart-product/cart-product.component';

@Component({
    selector: 'products',
    templateUrl: './products.component.html',
    styleUrls: ['../../app.component.css']
})
export class ProductsComponent {
    products: MatTableDataSource<Product>;
    id?: number;
    cartSummary: number;
    isEmpty: boolean;
    displayedColumns: string[] = ['id', 'addToCart', 'name', 'price', 'count'];

    constructor(public dialog: MatDialog, private _service: ProductService) {
        this.products = new MatTableDataSource<Product>();
    }

    ngOnInit(): void {
        this.getProducts();
    }

    addToCart(product: Product): void {
        this.addProductToCart(product);
    }

    filter(filterValue: string): void {
        this.products.filter = filterValue;
    }

    private addProductToCart(product: Product): void {
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

    private getProducts(): void {
        this.products.data = this._service.products;
        this.cartSummary = this._service.cartSummary;
        this.isEmpty = !this.products.data;
    }
}