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
    isEmpty: boolean;
    loading: boolean = false;
    displayedColumns: string[] = ['id', 'edit', 'name', 'price', 'count', 'delete'];

    constructor(public dialog: MatDialog, private _service: ProductService) {
        this.products = new MatTableDataSource<Product>();
    }

    ngOnInit(): void {
        this.getProducts();
    }

    add(): void {
        const dialogRef = this.dialog.open(ProductComponent, {
            data: {}
        });

        dialogRef.afterClosed().subscribe((result: Product) => {
            if (result)
                this.createProduct(result);
        });
    }

    edit(product: Product): void {
        const dialogRef = this.dialog.open(ProductComponent, {
            data: { ...product }
        });

        dialogRef.afterClosed().subscribe(result => {
            if (result) {
                this.updateProduct(result);
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
                this.deleteProduct(id);
            }
        })
    }

    filter(filterValue: string): void {
        this.products.filter = filterValue;
    }

    private getProducts(): void {
        this.loading = true;
        this._service.get({}).subscribe((result) => {
            this.products.data = result;
            this.isEmpty = !this.products.data;
            this.loading = false;
        });
    }

    private createProduct(product: Product): void {
        this._service.create(product).subscribe(null, null, () => this.getProducts());
    }

    private updateProduct(product: Product): void {
        this._service.update(product).subscribe(null, null, () => this.getProducts());
    }

    private deleteProduct(id: number): void {
        this._service.delete([id]).subscribe(null, null, () => this.getProducts());
    }
}