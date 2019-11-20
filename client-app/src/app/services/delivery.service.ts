import { Observable } from 'rxjs';
import { Delivery, DeliveryGetOptions } from '../models/delivery.models';
import { Injectable } from '@angular/core';
import { HttpClient, HttpParams, HttpHeaders } from '@angular/common/http';

@Injectable({
    providedIn: 'root'
})
export class DeliveryService {
    private readonly _apiUrl = 'api/delivery';

    private readonly _httpOptions = {
        headers: new HttpHeaders({ 'Content-Type': 'application/json' })
    };

    constructor(private httpClient: HttpClient) { }

    public get(options: DeliveryGetOptions): Observable<Delivery[]> {
        const params = new HttpParams();
        if (options.id)
            params.set('id', options.id.toString());
        if (options.ids)
            params.set('ids', options.ids.toString());
        if (options.search)
            params.set('search', options.search);
        return this.httpClient.get<Delivery[]>(this._apiUrl, { params });
    }

    create(model: Delivery): Observable<Delivery> {
        return this.httpClient.post<Delivery>(this._apiUrl, model, this._httpOptions);
    }

    update(model: Delivery): Observable<Delivery> {
        return this.httpClient.patch<Delivery>(this._apiUrl, model, this._httpOptions);
    }

    delete(ids: number[]): Observable<any> {
        let url = `${this._apiUrl}?ids=${ids}`;
        return this.httpClient.delete(url);
    }
}