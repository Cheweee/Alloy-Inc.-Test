import { Observable, BehaviorSubject } from 'rxjs';
import { Product, ProductGetOptions } from '../models/product.models';
import { Injectable } from '@angular/core';
import { HttpClient, HttpParams, HttpHeaders } from '@angular/common/http';
import { ReportGetOptions, CartReport } from '../models/report.models';

@Injectable({
    providedIn: 'root'
})
export class ReportService {
    private readonly _apiUrl = 'api/report';

    private readonly _httpOptions = {
        headers: new HttpHeaders({ 'Content-Type': 'application/json' })
    };

    constructor(private httpClient: HttpClient) { }

    public getCartReport(options: ReportGetOptions): Observable<CartReport> {
        const params = new HttpParams();
        if (options.dateFrom)
            params.set('dateFrom', options.dateFrom.toString());
        if (options.dateTo)
            params.set('dateTo', options.dateTo.toString());
        return this.httpClient.get<CartReport>(`${this._apiUrl}/cart`, { params });
    }
}