import { inject, Injectable } from '@angular/core';
import { BehaviorSubject, filter } from 'rxjs';
import { PlayerData } from './PlayerData';
import { just, Maybe, none } from '@sweet-monads/maybe';
import { GameApiService } from '../../networking/services/game-api/game-api.service';
import { isSetDrawer } from '../../networking/dtos/game/game-flow/setDrawer.guard';
import { isSetNotDrawer } from '../../networking/dtos/game/game-flow/setNotDrawer.guard';
import { SetDrawer } from '../../networking/dtos/game/game-flow/setDrawer';
import { SetNotDrawer } from '../../networking/dtos/game/game-flow/setNotDrawer';
import { PlayerType } from './PlayerType';

@Injectable({
  providedIn: null
})
export class PlayerDataService 
{
  private gameApiService: GameApiService = inject(GameApiService);

  public constructor()
  {
    this.gameApiService.ObserveGameFlowEvent()
    .pipe(filter((val) => isSetDrawer(val) || isSetNotDrawer(val)))
    .subscribe(this.OnPlayerTypeChanged)
  }

  public PlayerData: BehaviorSubject<Maybe<PlayerData>> = new BehaviorSubject<Maybe<PlayerData>>(none());

  private OnPlayerTypeChanged(value: SetDrawer | SetNotDrawer) 
  {
    if (this.PlayerData.value.isNone()) return;

    const role = isSetDrawer(value) ? PlayerType.Drawer : PlayerType.Guesser;

    this.PlayerData.next
    (
      just
      (
        {
          Username: this.PlayerData.value.value.Username,
          Id: this.PlayerData.value.value.Id,
          Role: role, 
          Score: this.PlayerData.value.value.Score
        }
      )
    );
  
  }
}
