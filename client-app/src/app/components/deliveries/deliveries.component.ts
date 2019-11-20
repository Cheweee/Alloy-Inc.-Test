import { Component } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { MatTableDataSource } from '@angular/material/table';
import { Delivery } from '../../models/delivery.models';
import { DeliveryService } from '../../services/delivery.service';
import { ConfirmDialogComponent } from '../confirm-dialog/confirm-dialog.component';
import { DeliveryComponent } from '../delivery/delivery.component';

@Component({
    selector: 'deliveries',
    templateUrl: './deliveries.component.html',
    styleUrls: ['../../app.component.css']
})
export class DeliveriesComponent {
    deliveries: MatTableDataSource<Delivery>;
    id?: number;
    isEmpty: boolean;
    loading: boolean = false;
    displayedColumns: string[] = ['name', 'edit', 'deliveryPrice', 'delete'];

    constructor(public dialog: MatDialog, private _service: DeliveryService) {
        this.deliveries = new MatTableDataSource<Delivery>();
    }

    ngOnInit(): void {
        this.getDeliveries();
    }

    add(): void {
        const dialogRef = this.dialog.open(DeliveryComponent, {
            data: {}
        });

        dialogRef.afterClosed().subscribe((result: Delivery) => {
            if (result)
                this.createDelivery(result);
        });
    }

    edit(delivery: Delivery): void {
        const dialogRef = this.dialog.open(DeliveryComponent, {
            data: { ...delivery }
        });

        dialogRef.afterClosed().subscribe(result => {
            if (result) {
                this.updateDelivery(result);
            }
            else {
                this.getDeliveries();
            }
        });
    }

    delete(id: number): void {
        const dialogRef = this.dialog.open(ConfirmDialogComponent, {
            data: { message: 'Are you sure want to delete delivery?' }
        });

        dialogRef.afterClosed().subscribe(result => {
            if (result) {
                this.deleteDelivery(id);
            }
        })
    }

    filter(filterValue: string): void {
        this.deliveries.filter = filterValue;
    }

    private getDeliveries(): void {
        this.loading = true;
        this._service.get({}).subscribe((result) => {
            this.deliveries.data = result;
            this.isEmpty = !this.deliveries.data;
            this.loading = false;
        });
    }

    private createDelivery(delivery: Delivery): void {
        this._service.create(delivery).subscribe(null, null, () => this.getDeliveries());
    }

    private updateDelivery(delivery: Delivery): void {
        this._service.update(delivery).subscribe(null, null, () => this.getDeliveries());
    }

    private deleteDelivery(id: number): void {
        this._service.delete([id]).subscribe(null, null, () => this.getDeliveries());
    }
}