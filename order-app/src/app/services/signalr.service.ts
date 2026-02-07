import { Injectable } from '@angular/core';
import * as signalR from '@microsoft/signalr';
import { AuthService } from './auth.service';
import { ToastService } from './toast.service';

@Injectable({
    providedIn: 'root'
})
export class SignalRService {

    private hubConnection!: signalR.HubConnection;

    constructor(
        private authService: AuthService,
        private toast: ToastService
    ) { }

    connect() {
        if (typeof window === 'undefined') return;
        if (this.hubConnection?.state === 'Connected') return;

        const token = this.authService.getToken();
        if (!token) {
            console.warn('SignalR skipped: no token yet');
            return;
        }

        this.hubConnection = new signalR.HubConnectionBuilder()
            .withUrl('http://localhost:5197/hubs/orders', {
                accessTokenFactory: () => token
            })
            .withAutomaticReconnect()
            .build();

        this.hubConnection.start()
            .then(() => console.log('SignalR connected'))
            .catch(err => console.error('SignalR error:', err));
    }


    onOrderStatusUpdated(
        callback: (orderId: string, status: string) => void
    ) {
        this.hubConnection.on(
            'OrderStatusUpdated',
            (orderId: string, status: string) => {
                callback(orderId, status);
            }
        );
    }

    disconnect() {
        this.hubConnection?.stop();
        this.hubConnection = undefined!;
    }
}
