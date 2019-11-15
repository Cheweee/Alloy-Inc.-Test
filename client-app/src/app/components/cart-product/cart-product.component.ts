import { Component, Inject } from '@angular/core';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { Product } from '../../models/product.models';

@Component({
    selector: 'cart-product',
    templateUrl: './cart-product.component.html',
    styleUrls: ['../../app.component.css']
})
export class CartProductComponent {
    readonly min: number = 1;
    readonly max: number;

    constructor(
      public dialogRef: MatDialogRef<CartProductComponent>,
      @Inject(MAT_DIALOG_DATA) public data: { product: Product, maxCount: number}) {
          this.max = data.maxCount;
      }
    
      onCancelClick(): void {
        this.dialogRef.close();
      }
}