import { inject, Injectable } from '@angular/core';
import { LobbyApiService } from '../../networking/services/lobby-api/lobby-api.service';
import { firstValueFrom } from 'rxjs';
import { PlayerDataService } from '../player-data/player-data.service';
import { just } from '@sweet-monads/maybe';
import { GameApiService } from '../../networking/services/game-api/game-api.service';
import { GameIdService } from '../gameId/game-id.service';
import { LobbyUserTypeService } from '../lobby-user-type/lobby-user-type.service';
import { LobbyUserType } from '../lobby-user-type/LobbyUserType';

@Injectable({
  providedIn: null
})
export class StartService 
{
  private lobbyApiService: LobbyApiService = inject(LobbyApiService);
  private playerDataService: PlayerDataService = inject(PlayerDataService);
  private gameApiService: GameApiService = inject(GameApiService);
  private gameIdService: GameIdService = inject(GameIdService);
  private lobbyUserTypeService: LobbyUserTypeService = inject(LobbyUserTypeService);

  public async TryJoinOrStartGame(lobbyId: string, asOwner: boolean): Promise<void>
  {
    const userData = await firstValueFrom(this.lobbyApiService.ConnectToLobby(lobbyId));

    this.playerDataService.PlayerData.next
    (
      {Username: userData.name, Id: userData.id, Score: 0}
    );

    this.lobbyUserTypeService.LobbyUserType.next(asOwner ? LobbyUserType.Owner : LobbyUserType.NonOwner);

    this.gameIdService.GameId.next(lobbyId);

    return this.gameApiService.Start(userData.id);
  }
}
