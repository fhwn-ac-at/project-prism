import { inject, Injectable } from '@angular/core';
import { ConfigService } from '../../../services/config/config.service';
import * as signalR from '@microsoft/signalr';
import { Observable, Subject } from 'rxjs';
import { AngularSignalRHttpClient } from './signal-r-http-client';
import Keycloak, { KeycloakAdapter, KeycloakTokenParsed } from 'keycloak-js';
import { KEYCLOAK_EVENT_SIGNAL } from 'keycloak-angular';

@Injectable({
  providedIn: null
})
export class SignalRService 
{
  private configService: ConfigService = inject(ConfigService);
  private httpClient: AngularSignalRHttpClient = inject(AngularSignalRHttpClient);
  private keycloakSignal = inject(Keycloak);

  private signalRHub:signalR.HubConnection | undefined;

  private dataReceivedEventSub: Subject<object> = new Subject<object>
  constructor() {
    
  }

  public Initialize(lobbyId: string)
  {
    const test = this.keycloakSignal;
    const options : signalR.IHttpConnectionOptions = {
      // httpClient: this.httpClient,
    }

    this.signalRHub = new signalR.HubConnectionBuilder()
    .withUrl
    (
      this.configService.configData.api.base + this.configService.configData.api.websocket + '/' + lobbyId,
      options
    )
    .configureLogging(signalR.LogLevel.Information)
    .withAutomaticReconnect()
    .build();

    this.signalRHub.on("Frontend", (data) => {console.log(data); this.dataReceivedEventSub.next(data)});
  }

  public get DataReceivedEvent(): Observable<object>
  {
    return this.dataReceivedEventSub.asObservable();
  }

  public async Connect(): Promise<void>
  {
    if (this.signalRHub == undefined) return Promise.reject(new Error("Not initialized!"));

    await this.signalRHub.start();
  }

  public Stop(): Promise<void>
  {
    if (this.signalRHub == undefined) return Promise.resolve();

    return this.signalRHub.stop();
  }

  public SendData(data: object): Promise<void>
  {
    if (this.signalRHub == undefined) return Promise.reject(new Error("Not initialized!"));

    return this.signalRHub.send("Backend", JSON.stringify(data));
  }
}
