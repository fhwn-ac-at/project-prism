import { inject, Injectable } from '@angular/core';
import { map, Observable } from 'rxjs';
import { User } from '../../dtos/shared/User';
import { HttpClient, HttpParams } from '@angular/common/http';
import { isUser } from '../../dtos/shared/User.guard';
import { ConfigService } from '../../../services/config/config.service';

@Injectable({
  providedIn: null
})
export class LobbyApiService 
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

  public StartGame(lobbyId: string): Observable<void>
  {
    let params = new HttpParams().set("lobbyId", lobbyId);
    
    return this.httpClient.get
    (
      this.configService.configData.api.base + 
      this.configService.configData.api.lobby.base +
      this.configService.configData.api.lobby.startGame,
      {
        params: params, 
        responseType: "text"
      }
    ).
    pipe
    (
      map(() => {})
    );
  }
}
