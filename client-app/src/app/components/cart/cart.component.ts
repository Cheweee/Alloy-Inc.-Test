import { Component } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { MatTableDataSource } from '@angular/material/table';
import { Product, Cart } from '../../models/product.models';
import { ReportService } from 'src/app/services/report.service';
import { CartService } from 'src/app/services/cart.service';
import { SelectionModel } from '@angular/cdk/collections';
import { ConfirmDialogComponent } from '../confirm-dialog/confirm-dialog.component';

@Component({
    selector: 'cart',
    templateUrl: './cart.component.html',
    styleUrls: ['../../app.component.css']
})
export class CartComponent {
    products: MatTableDataSource<Cart>;
    selection: SelectionModel<Cart>;
    id?: number;
    cartSummary: number = 0;
    isEmpty: boolean = true;
    loading: boolean = true;
    readonly displayedColumns: string[] = ['select', 'id', 'decrement', 'count', 'increment', 'name', 'price', 'summary', 'delete'];

    constructor(public dialog: MatDialog, private _service: CartService, private _reportService: ReportService) {
        this.products = new MatTableDataSource<Product>();
        this.selection = new SelectionModel<Cart>(true, []);
    }

    ngOnInit(): void {
        this.getProducts();
    }

    onOrderPlaced() {
        this.getProducts();
    }

    isAllSelected() {
        const numSelected = this.selection.selected.length;
        const numRows = this.products.data.length;
        return numSelected === numRows;
    }

    masterToggle() {
        this.isAllSelected() ?
            this.selection.clear() :
            this.products.data.forEach(row => this.selection.select(row));
    }

    checkboxLabel(row?: Cart): string {
        if (!row) {
            return `${this.isAllSelected() ? 'select' : 'deselect'} all`;
        }
        return `${this.selection.isSelected(row) ? 'deselect' : 'select'} row ${row.id + 1}`;
    }

    increment(product: Product): void {
        product.count++;
        this.updateCart(product);
    }

    decrement(product: Product): void {
        product.count--;
        this.updateCart(product);
    }

    delete(id: number): void {
        const dialogRef = this.dialog.open(ConfirmDialogComponent, {
            data: { message: 'Are you sure want to delete product from cart?' }
        });
        dialogRef.afterClosed().subscribe(result => {
            if (result) {
                this.deleteFromCart(id);
            }
        });
    }

    filter(filterValue: string): void {
        this.products.filter = filterValue;
        this.getProducts();
    }

    private updateCart(cart: Cart): void {
        this._service.update(cart).subscribe(null, null, () => this.getProducts());
    }

    private getCartSummary(): void {
        this._reportService.getCartReport({}).subscribe((result) => {
            this.cartSummary = result.summary;
            this.loading = false;
        });
    }

    private getProducts(): void {
        this.loading = true;
        this._service.get({ ordered: false }).subscribe((result) => {
            this.products.data = result;
            this.selection = new SelectionModel<Cart>(true, []);
            this.isEmpty = !this.products.data;
        },
            null,
            () => this.getCartSummary());
    }

    private deleteFromCart(id: number) {
        this._service.delete([id]).subscribe(null, null, () => this.getProducts());
    }
}