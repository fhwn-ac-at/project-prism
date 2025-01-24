import { inject, Injectable } from '@angular/core';
import { LobbyApiService } from '../lobby-api/lobby-api.service';
import { firstValueFrom, Observable } from 'rxjs';
import { PlayerData } from '../player-data/PlayerData';
import { PlayerDataService } from '../player-data/player-data.service';
import { just } from '@sweet-monads/maybe';
import { PlayerType } from '../player-data/PlayerType';
import { GameApiService } from '../gameApi/game-api.service';
import { GameIdService } from '../gameId/game-id.service';

@Injectable({
  providedIn: null
})
export class StartService 
{
  private lobbyApiService: LobbyApiService = inject(LobbyApiService);
  private playerDataService: PlayerDataService = inject(PlayerDataService);
  private gameApiService: GameApiService = inject(GameApiService);
  private gameIdService: GameIdService = inject(GameIdService);

  public async TryJoinGame(lobbyToJoin: string): Promise<void>
  {
    const userData = await firstValueFrom(this.lobbyApiService.ConnectToLobby(lobbyToJoin));

    this.playerDataService.PlayerData.next
    (
      just({Username: userData.name, Id: userData.id, Role: PlayerType.NotSet, Score: 0})
    );

    this.gameIdService.GameId.next(just(lobbyToJoin));

    return this.gameApiService.Start(userData.id);
  }
}
