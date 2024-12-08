import { Injectable } from '@angular/core';
import { HubConnection, HubConnectionBuilder } from '@microsoft/signalr';
@Injectable({
  providedIn: 'root'
})
export class OrdertrackingService {
  private hubConnection: HubConnection;

  constructor() {
    this.hubConnection = new HubConnectionBuilder()
      .withUrl('http://localhost:5000/orderTrackingHub')  // Match your backend URL
      .build();

    this.startConnection();
    this.addTrackingUpdateListener();
  }

  private startConnection() {
    this.hubConnection
      .start()
      .then(() => console.log('Connection established to SignalR hub'))
      .catch(err => console.error('Error while establishing connection: ' + err));
  }

  private addTrackingUpdateListener() {
    this.hubConnection.on('ReceiveTrackingUpdate', (orderId: number, status: string, location: string) => {
      console.log('Received tracking update:', orderId, status, location);
      // You can handle the update here, for example, update the UI with new status
    });
  }
}
