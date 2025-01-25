import { inject, Injectable } from '@angular/core';
import { LobbyApiService } from '../../networking/services/lobby-api/lobby-api.service';
import { firstValueFrom, Observable } from 'rxjs';
import { PlayerData } from '../player-data/PlayerData';
import { PlayerDataService } from '../player-data/player-data.service';
import { just } from '@sweet-monads/maybe';
import { PlayerType } from '../player-data/PlayerType';
import { GameApiService } from '../../networking/services/game-api/game-api.service';
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

  public async TryJoinOrStartGame(lobbyId: string): Promise<void>
  {
    const userData = await firstValueFrom(this.lobbyApiService.ConnectToLobby(lobbyId));

    this.playerDataService.PlayerData.next
    (
      just({Username: userData.name, Id: userData.id, Role: PlayerType.NotSet, Score: 0})
    );

    this.gameIdService.GameId.next(just(lobbyId));

    return this.gameApiService.Start(userData.id);
  }
}
