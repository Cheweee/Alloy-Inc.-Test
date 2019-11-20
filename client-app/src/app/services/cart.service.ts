import { Observable, BehaviorSubject } from 'rxjs';
import { Product, ProductGetOptions, CartGetOptions, Cart } from '../models/product.models';
import { Injectable } from '@angular/core';
import { HttpClient, HttpParams, HttpHeaders } from '@angular/common/http';

@Injectable({
    providedIn: 'root'
})
export class CartService {
    private readonly _apiUrl = 'api/cart';

    private readonly _httpOptions = {
        headers: new HttpHeaders({ 'Content-Type': 'application/json' })
    };

    constructor(private httpClient: HttpClient) { }

    public get(options: CartGetOptions): Observable<Cart[]> {
        let url = this._apiUrl;
        let conditionIndex: number = 0;
        if (options.id)
            url += `${conditionIndex++ === 0 ? '?' : '&'}id=${options.id}`;
        if (options.orderId)
            url += `${conditionIndex++ === 0 ? '?' : '&'}orderId=${options.orderId}`;
        if (options.ids)
            url += `${conditionIndex++ === 0 ? '?' : '&'}ids=${options.ids}`;
        if (options.orderIds)
            for (const orderId of options.orderIds)
                url += `${conditionIndex++ === 0 ? '?' : '&'}orderIds=${orderId}`;
        if (options.search !== undefined)
            url += `${conditionIndex++ === 0 ? '?' : '&'}search=${options.search}`;
        if (options.ordered !== undefined)
            url += `${conditionIndex++ === 0 ? '?' : '&'}ordered=${options.ordered}`;

        return this.httpClient.get<Cart[]>(url);
    }

    update(model: Cart): Observable<Cart> {
        return this.httpClient.post<Cart>(this._apiUrl, model, this._httpOptions);
    }

    addToCart(model: Cart): Observable<Cart> {
        return this.httpClient.patch<Cart>(this._apiUrl, model, this._httpOptions);
    }

    delete(ids: number[]): Observable<any> {
        let url = `${this._apiUrl}?ids=${ids}`;
        return this.httpClient.delete(url);
    }
}