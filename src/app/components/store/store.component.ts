import { Component } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { MatTableDataSource } from '@angular/material/table';
import { Product } from '../../models/product.models';
import { ProductService } from '../../services/product.service';
import { ConfirmDialogComponent } from '../confirm-dialog/confirm-dialog.component';
import { ProductComponent } from '../product/product.component';

@Component({
    selector: 'store',
    templateUrl: './store.component.html',
    styleUrls: ['../../app.component.css']
})
export class StoreComponent {
    products: MatTableDataSource<Product>;
    id?: number;
    cartSummary: number;
    isEmpty: boolean;
    displayedColumns: string[] = ['id', 'edit', 'name', 'price', 'count', 'delete'];

    constructor(public dialog: MatDialog, private _service: ProductService) {
        this.products = new MatTableDataSource<Product>();
    }

    ngOnInit(): void {
        this.getProducts();
    }

    add(): void {
        const dialogRef = this.dialog.open(ProductComponent, {
            data: new Product()
        });

        dialogRef.afterClosed().subscribe(result => {
            if (result)
                this.createInstructor(result);
        });
    }

    edit(product: Product): void {
        const dialogRef = this.dialog.open(ProductComponent, {
            data: { ...product }
        });

        dialogRef.afterClosed().subscribe(result => {
            if (result) {
                this.updateInstructor(result);
            }
            else {
                this.getProducts();
            }
        });
    }

    delete(id: number): void {
        const dialogRef = this.dialog.open(ConfirmDialogComponent, {
            data: { message: 'Are you sure want to delete product?' }
        });

        dialogRef.afterClosed().subscribe(result => {
            if (result) {
                this.deleteInstructor(id);
            }
        })
    }

    filter(filterValue: string): void {
        this.products.filter = filterValue;
    }

    private getProducts(): void {
        this.products.data = this._service.products;
        this.isEmpty = !this.products.data;
        this.cartSummary = this._service.cartSummary;
    }

    private createInstructor(product: Product): void {
        this._service.create(product);
        this.getProducts();
    }

    private updateInstructor(product: Product): void {
        this._service.update(product);
        this.getProducts();
    }

    private deleteInstructor(id: number): void {
        this._service.delete(id);
        this.getProducts();
    }
}