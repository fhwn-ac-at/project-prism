import { inject, Injectable } from '@angular/core';
import { map, Observable } from 'rxjs';
import { User } from '../../dtos/shared/User';
import { HttpClient, HttpParams } from '@angular/common/http';
import { isUser } from '../../dtos/shared/User.guard';
import { environment } from '../../../../environment/environment';

@Injectable({
  providedIn: null
})
export class LobbyApiService 
{
  private httpClient: HttpClient = inject(HttpClient);

  public ConnectToLobby(lobbyId: string): Observable<User>
  {
    let params = new HttpParams().set("lobbyId", lobbyId);
    
    return this.httpClient.get
    (
      environment.api.base + 
      environment.api.lobby.base +
      environment.api.lobby.connect,
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
      environment.api.base + 
      environment.api.lobby.base +
      environment.api.lobby.startGame,
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
