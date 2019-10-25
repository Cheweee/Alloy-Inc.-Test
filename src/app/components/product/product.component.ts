import { Component, Inject } from '@angular/core';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { Product } from 'src/app/models/product.models';

@Component({
    selector: 'product',
    templateUrl: './product.component.html',
})
export class ProductComponent {
    constructor(
      public dialogRef: MatDialogRef<ProductComponent>,
      @Inject(MAT_DIALOG_DATA) public data: Product) {}
    
      onCancelClick(): void {
        this.dialogRef.close();
      }
}