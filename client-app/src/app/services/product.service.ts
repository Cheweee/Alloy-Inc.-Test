import { Observable, BehaviorSubject } from 'rxjs';
import { Product } from '../models/product.models';
import { Injectable } from '@angular/core';

@Injectable()
export class ProductService {
    private _products: Product[];
    private _cart: Product[];

    constructor() {
        this._products = [];
        this._cart = [];
    }

    get products(): Product[] {
        return this._products;
    }

    get cart(): Product[] {
        return this._cart;
    }

    get cartSummary(): number {
        let summary = 0;
        if (this._cart) {
            summary = this._cart.reduce((sum, current) => sum + (current.price * current.count), 0);
        }

        return summary;
    }

    create(product: Product): void {
        const id = Math.max(...this._products.map(o => o.id), 0) + 1;
        const newProduct = {
            ...product,
            id: id,
            originalCount: product.count
        };
        this._products.push(newProduct);
    }

    update(product: Product): void {
        const oldProduct = this._products.find(o => o.id == product.id);
        oldProduct.count = product.count;
        oldProduct.name = product.name;
        oldProduct.price = product.price;
        oldProduct.originalCount = product.count;
    }

    delete(id: number): void {
        this._products = this._products.splice(id, 1);
    }

    deleteFromCart(id: number): void {
        this._cart = this._cart.filter(o => o.id !== id);
        const product = this._products.find(o => o.id);
        if (product)
            product.count = product.originalCount;
    }

    setProductCount(id: number, count: number): Product {
        const product = this._products.find(o => o.id == id);
        product.count = product.originalCount - count;

        return product;
    }

    setProductCountInCart(id: number, count: number): void {
        const product = this.setProductCount(id, count);
        let productInCart = this._cart.find(o => o.id == id);
        if (productInCart) {
            productInCart.count = count;
        }
        else {
            productInCart = {
                count: count,
                id: product.id,
                name: product.name,
                price: product.price,
                originalCount: product.originalCount
            }

            this._cart.push(productInCart);
        }
        if (productInCart.count <= 0) {
            this._cart.splice(id, 1);
        }
    }

    incrementProductCountInCart(id: number, count: number): void {
        const product = this.decrementProductCount(id, count);
        let productInCart = this._cart.find(o => o.id == id);
        if (productInCart) {
            productInCart.count += count;
        }
        else {
            productInCart = {
                count: count,
                id: product.id,
                name: product.name,
                price: product.price,
                originalCount: product.count
            }

            this._cart.push(productInCart);
        }
        if (productInCart.count <= 0) {
            this._cart.splice(id, 1);
        }
    }

    decrementProductCount(id: number, count: number): Product {
        const product = this._products.find(o => o.id == id);
        product.count -= count;

        return product;
    }
}