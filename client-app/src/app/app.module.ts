import { BrowserModule } from '@angular/platform-browser';
import { Routes, RouterModule } from '@angular/router';
import { NgModule } from '@angular/core';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { FormsModule } from '@angular/forms';

import { FlexLayoutModule } from "@angular/flex-layout";

import { AppComponent } from './app.component';

import { MaterialModule } from './material.module';
import { ProductsComponent } from './components/products/products.component';
import { StoreComponent } from './components/store/store.component';
import { CartComponent } from './components/cart/cart.component';
import { ProductService } from './services/product.service';
import { ProductComponent } from './components/product/product.component';
import { CartProductComponent } from './components/cart-product/cart-product.component';
import { ConfirmDialogComponent } from './components/confirm-dialog/confirm-dialog.component';

// определение маршрутов
const appRoutes: Routes =[
  { path: '', component: ProductsComponent},
  { path: 'products/management', component: StoreComponent},
  { path: 'cart', component: CartComponent }
];

@NgModule({
  declarations: [
    AppComponent,
    ProductsComponent,
    StoreComponent,
    CartComponent,
    ProductComponent,
    ConfirmDialogComponent,
    CartProductComponent
  ],
  imports: [
    FlexLayoutModule,
    FormsModule,
    BrowserModule,
    BrowserAnimationsModule,
    MaterialModule,
    RouterModule.forRoot(appRoutes)
  ],
  entryComponents: [
    ProductComponent,
    ProductsComponent,
    StoreComponent,
    CartComponent,
    ConfirmDialogComponent,
    CartProductComponent
  ],
  providers: [ProductService],
  bootstrap: [AppComponent]
})
export class AppModule { }
