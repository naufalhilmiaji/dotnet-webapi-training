import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

@Injectable({
    providedIn: 'root'
})
export class OrdersService {

    private apiUrl = 'http://localhost:5197/api/orders';

    constructor(private http: HttpClient) { }

    getOrders(): Observable<any[]> {
        return this.http.get<any[]>(this.apiUrl);
    }

    getOrderById(id: string): Observable<any> {
        return this.http.get<any>(`${this.apiUrl}/${id}`);
    }

    createOrder(payload: {
        items: { productId: string; quantity: number }[];
    }) {
        return this.http.post<any>(this.apiUrl, payload);
    }

}
