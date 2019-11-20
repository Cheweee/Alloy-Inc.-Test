import { Component, Inject, ViewChild } from '@angular/core';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { Order } from 'src/app/models/order.models';
import { Delivery } from 'src/app/models/delivery.models';
import { MatStepper } from '@angular/material/stepper';
import { StepperSelectionEvent } from '@angular/cdk/stepper';
import { NgForm } from '@angular/forms';

@Component({
  selector: 'delivery-info',
  templateUrl: './delivery-info.component.html',
  styleUrls: ['../../app.component.css']
})
export class DeliveryInfoComponent {
  @ViewChild('stepper', { static: false }) stepper: MatStepper;
  @ViewChild('destinationDetails', { static: false }) destinationDetails: NgForm;
  @ViewChild('shippingDetails', { static: false }) shippingDetails: NgForm;
  isLastStep: boolean = false;
  nextDisabled: boolean = true;
  delivery: Delivery;

  constructor(
    public dialogRef: MatDialogRef<DeliveryInfoComponent>,
    @Inject(MAT_DIALOG_DATA) public data: { subtotal: number, order: Order, deliveries: Delivery[] }) { }

  ngOnInit() {
    this.delivery = this.data.deliveries.find(o => o.id === this.data.order.deliveryId);
  }

  validateDestinationDetails() {
    this.nextDisabled = this.destinationDetails.invalid;
  }

  validateShippingDetails() {
    this.nextDisabled = this.destinationDetails.invalid;
  }

  displayFn(delivery?: Delivery): string | undefined {
    return delivery ? delivery.name : '';
  }

  checkStep(event: StepperSelectionEvent) {
    this.isLastStep = event.selectedIndex === this.stepper.steps.length - 1;
  }

  onCancelClick(): void {
    this.dialogRef.close();
  }

  onSubmit(): void {
    this.data.order.deliveryId = this.delivery.id;
    this.dialogRef.close(this.data);
  }
}