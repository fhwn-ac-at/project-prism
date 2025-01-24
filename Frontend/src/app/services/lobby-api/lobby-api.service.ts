import { inject, Injectable } from '@angular/core';
import { ApiService } from '../../networking/services/api/api.service';
import { map, Observable } from 'rxjs';
import { ConnectToLobbyResponse, isConnectToLobbyResponse } from '../../networking/services/api/dto/ConnectToLobbyResponse';
import { User } from '../../networking/dtos/shared/User';

@Injectable({
  providedIn: null
})
export class LobbyApiService 
{
  private apiService: ApiService = inject(ApiService);

  public ConnectToLobby(lobbyId: string): Observable<User>
  {
    return this.apiService.ConnectToLobby(lobbyId);
  }

  public StartGame(lobbyId: string): Observable<void>
  {
    return this.apiService.StartGame(lobbyId);
  }
}
