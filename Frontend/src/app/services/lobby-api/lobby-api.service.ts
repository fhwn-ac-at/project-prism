import { inject, Injectable } from '@angular/core';
import { ApiService } from '../../networking/services/api/api.service';
import { map, Observable } from 'rxjs';
import { ConnectToLobbyResponse, isConnectToLobbyResponse } from '../../networking/services/api/dto/ConnectToLobbyResponse';

@Injectable({
  providedIn: null
})
export class LobbyApiService 
{
  private apiService: ApiService = inject(ApiService);

  public ConnectToLobby(lobbyId: string): Observable<ConnectToLobbyResponse>
  {
    return this.apiService.ConnectToLobby(lobbyId);
  }

  public StartGame(lobbyId: string): Observable<string>
  {
    return this.apiService.StartGame(lobbyId);
  }
}
