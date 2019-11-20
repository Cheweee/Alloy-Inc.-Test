import { Observable } from 'rxjs';
import { Order, OrderGetOptions } from '../models/order.models';
import { Injectable } from '@angular/core';
import { HttpClient, HttpParams, HttpHeaders } from '@angular/common/http';

@Injectable({
    providedIn: 'root'
})
export class OrderService {
    private readonly _apiUrl = 'api/order';

    private readonly _httpOptions = {
        headers: new HttpHeaders({ 'Content-Type': 'application/json' })
    };

    constructor(private httpClient: HttpClient) { }

    public get(options: OrderGetOptions): Observable<Order[]> {
        const params = new HttpParams();
        if (options.id)
            params.set('id', options.id.toString());
        if (options.ids)
            params.set('ids', options.ids.toString());
        if (options.search)
            params.set('search', options.search);
        return this.httpClient.get<Order[]>(this._apiUrl, { params });
    }

    create(model: Order): Observable<Order> {
        return this.httpClient.post<any>(this._apiUrl, model, this._httpOptions);
    }

    update(model: Order): Observable<Order> {
        return this.httpClient.patch<Order>(this._apiUrl, model, this._httpOptions);
    }

    delete(ids: number[]): Observable<any> {
        let url = `${this._apiUrl}?ids=${ids}`;
        return this.httpClient.delete(url);
    }
}