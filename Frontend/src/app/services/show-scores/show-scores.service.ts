import { inject, Injectable } from '@angular/core';
import { GameApiService } from '../../networking/services/game-api/game-api.service';
import { ActivePlayersService } from '../current-players/active-players.service';

@Injectable({
  providedIn: null
})
export class ShowScoresService 
{
  private gameApiService: GameApiService = inject(GameApiService);
  private activePlayersService: ActivePlayersService = inject(ActivePlayersService);
 
}
