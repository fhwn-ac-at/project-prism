import { inject, Injectable } from '@angular/core';
import { ConfigService } from '../../../services/config/config.service';
import * as signalR from '@microsoft/signalr';
import { HttpClient } from '@angular/common/http';

@Injectable({
  providedIn: null
})
export class SignalRService {

  private httpClient: HttpClient = inject(HttpClient);
  private configService: ConfigService = inject(ConfigService);

  public Initialize(lobbyId: string)
  {
    const options : signalR.IHttpConnectionOptions = {
    }

    this.SignalRHub = new signalR.HubConnectionBuilder()
    .withUrl
    (
      this.configService.configData.api.base + this.configService.configData.api.websocket + '/' + lobbyId,
      options
    )
    .configureLogging(signalR.LogLevel.Information)
    .withAutomaticReconnect()
    .build();
  }

  public SignalRHub: signalR.HubConnection | undefined;

  // public Connect()
  // {
    // if (this.SignalRHub == undefined) return Promise.reject(new Error("Not initialized!"));

    // return this.SignalRHub.start();
  // }
}
