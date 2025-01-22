import { inject, Injectable } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { ConfigService } from '../../../services/config/config.service';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: null
})
export class ApiService 
{
  private httpClient: HttpClient = inject(HttpClient);
  private configService: ConfigService = inject(ConfigService);

  public ConnectToLobby(lobbyId: string): Observable<string>
  {
    let params = new HttpParams().set("lobbyId", lobbyId);
    
    return this.httpClient.get
    (
      this.configService.configData.api.base + 
      this.configService.configData.api.lobby.base +
      this.configService.configData.api.lobby.connect,
      {
        params: params, 
        responseType: 'text'
      }
    );
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
