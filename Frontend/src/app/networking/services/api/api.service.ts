import { inject, Injectable } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { ConfigService } from '../../../services/config/config.service';
import { map, Observable } from 'rxjs';
import { ConnectToLobbyResponse, isConnectToLobbyResponse } from './dto/ConnectToLobbyResponse';
import { isUser } from '../../dtos/shared/User.guard';
import { User } from '../../dtos/shared/User';

@Injectable({
  providedIn: null
})
export class ApiService 
{
  private httpClient: HttpClient = inject(HttpClient);
  private configService: ConfigService = inject(ConfigService);

  public ConnectToLobby(lobbyId: string): Observable<User>
  {
    let params = new HttpParams().set("lobbyId", lobbyId);
    
    return this.httpClient.get
    (
      this.configService.configData.api.base + 
      this.configService.configData.api.lobby.base +
      this.configService.configData.api.lobby.connect,
      {
        params: params, 
        responseType: 'json'
      }
    )
    .pipe
    (
      map
      (
        (obj) => 
        {
          if (isUser(obj)) 
          {
            return {name: obj.name, id: obj.id};
          }
          else
          {
            throw new Error("Invalid response");
          }
        }
      )
    )
  }

  public StartGame(lobbyId: string): Observable<string>
  {
    let params = new HttpParams().set("lobbyId", lobbyId);
    
    return this.httpClient.get
    (
      this.configService.configData.api.base + 
      this.configService.configData.api.lobby.base +
      this.configService.configData.api.lobby.startGame,
      {
        params: params, 
        responseType: 'text'
      }
    );
  }
}
