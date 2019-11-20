import { Observable } from 'rxjs';
import { Product, ProductGetOptions } from '../models/product.models';
import { Injectable } from '@angular/core';
import { HttpClient, HttpParams, HttpHeaders } from '@angular/common/http';

@Injectable({
    providedIn: 'root'
})
export class ProductService {
    private readonly _apiUrl = 'api/product';

    private readonly _httpOptions = {
        headers: new HttpHeaders({ 'Content-Type': 'application/json' })
    };

    constructor(private httpClient: HttpClient) { }

    public get(options: ProductGetOptions): Observable<Product[]> {
        const params = new HttpParams();
        if (options.id)
            params.set('id', options.id.toString());
        if (options.ids)
            params.set('ids', options.ids.toString());
        if (options.search)
            params.set('search', options.search);
        return this.httpClient.get<Product[]>(this._apiUrl, { params });
    }

    create(model: Product): Observable<Product> {
        return this.httpClient.post<Product>(this._apiUrl, model, this._httpOptions);
    }

    update(model: Product): Observable<Product> {
        return this.httpClient.patch<Product>(this._apiUrl, model, this._httpOptions);
    }

    delete(ids: number[]): Observable<any> {
        let url = `${this._apiUrl}?ids=${ids}`;
        return this.httpClient.delete(url);
    }
}