import { Component, Inject } from '@angular/core';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { Delivery } from 'src/app/models/delivery.models';

@Component({
    selector: 'delivery',
    templateUrl: './delivery.component.html',
})
export class DeliveryComponent {
    constructor(
      public dialogRef: MatDialogRef<DeliveryComponent>,
      @Inject(MAT_DIALOG_DATA) public data: Delivery) {}
    
      onCancelClick(): void {
        this.dialogRef.close();
      }
}