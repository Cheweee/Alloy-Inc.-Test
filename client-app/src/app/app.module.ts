import { BrowserModule } from '@angular/platform-browser';
import { Routes, RouterModule } from '@angular/router';
import { NgModule } from '@angular/core';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';

import { FlexLayoutModule } from "@angular/flex-layout";

import { AppComponent } from './app.component';

import { MaterialModule } from './material.module';
import { ProductsComponent } from './components/products/products.component';
import { StoreComponent } from './components/store/store.component';
import { CartComponent } from './components/cart/cart.component';
import { ProductComponent } from './components/product/product.component';
import { ConfirmDialogComponent } from './components/confirm-dialog/confirm-dialog.component';
import { HTTP_INTERCEPTORS, HttpClientModule } from '@angular/common/http';
import { ApiUrlInterceptor } from './interceptors/url.iterceptor';
import { DeliveryInfoComponent } from './components/delivery-info/delivery-info.component';
import { OrderComponent } from './components/order/order.component';
import { DeliveryComponent } from './components/delivery/delivery.component';
import { DeliveriesComponent } from './components/deliveries/deliveries.component';
import { OrdersComponent } from './components/orders/orders.component';
import { OrderInfoComponent } from './components/order-info/order-info.component';

// определение маршрутов
const appRoutes: Routes = [
  { path: '', component: ProductsComponent },
  { path: 'products/management', component: StoreComponent },
  { path: 'cart', component: CartComponent },
  { path: 'deliveries', component: DeliveriesComponent },
  { path: 'orders', component: OrdersComponent }
];

@NgModule({
  declarations: [
    AppComponent,
    ProductsComponent,
    StoreComponent,
    CartComponent,
    ProductComponent,
    ConfirmDialogComponent,
    DeliveryInfoComponent,
    OrderComponent,
    DeliveryComponent,
    DeliveriesComponent,
    OrdersComponent,
    OrderInfoComponent,
  ],
  imports: [
    FlexLayoutModule,
    FormsModule,
    ReactiveFormsModule,
    BrowserModule,
    BrowserAnimationsModule,
    HttpClientModule,
    MaterialModule,
    RouterModule.forRoot(appRoutes)
  ],
  entryComponents: [
    ProductComponent,
    ConfirmDialogComponent,
    OrderInfoComponent,
    DeliveryInfoComponent,
    DeliveryComponent,
  ],
  providers: [
    { provide: HTTP_INTERCEPTORS, useClass: ApiUrlInterceptor, multi: true }
  ],
  bootstrap: [AppComponent]
})
export class AppModule { }
