import { Injectable, OnDestroy } from '@angular/core';
import { HubConnection, HubConnectionBuilder } from '@microsoft/signalr';

@Injectable({
  providedIn: 'root'
})
export class RealTimeCommunicationService implements OnDestroy {
  private static readonly WEBSOCKET_PATH = "/api/rtc";

  private readonly hubConnection: HubConnection;

  constructor() {
    this.hubConnection = new HubConnectionBuilder()
      .withUrl(RealTimeCommunicationService.WEBSOCKET_PATH)
      .withAutomaticReconnect()
      .build();
  }

  ngOnDestroy(): void {
    this.closeConnection();
  }

  openConnection(): Promise<void> {
    return this.hubConnection.start();
  }

  closeConnection(): Promise<void> {
    return this.hubConnection.stop();
  }

  callMethod(methodName: string, ...args: any[]): Promise<void> {
    return this.hubConnection.send(methodName, args);
  }

  onCall(methodName: string, handler: (...args: any[]) => any): void {
    this.hubConnection.on(methodName, handler);
  }
}
